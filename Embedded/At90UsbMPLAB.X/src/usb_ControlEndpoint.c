// AT90USB/usb_requests.c
// Handling of USB (standard) requests
// S. Salewski 22-MAR-2007

#include <string.h>
#include <stdint.h>
#include "macros.h"
#include "usb_spec.h"
#include "usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h" // EP0_FIFO_Size and USB_NumInterfaces
#include "usb_ControlEndpoint.h"
#include "at90usbkey.h"
#include "timer2ctc.h"

extern bool ApplyUsbLoggerCfg_Intr(USB_DeviceRequest* request);
extern uint8_t ResetCnt;

uint8_t UsbDevConfValue = UsbUnconfiguredState;
uint8_t AltSettingOfInterface[4] = {UsbInterfaceUnconfigured, UsbInterfaceUnconfigured, UsbInterfaceUnconfigured, UsbInterfaceUnconfigured};
uint8_t RemoteWakeupActive = 0;
USB_DeviceRequest SetupRequest;




uint8_t TxIn = 0;
uint8_t TxOut = 0;
uint8_t InFifoCnt = 0;
uint16_t InRetryCnt = 0;
uint32_t ElapsedTime = 0;
uint8_t BufferDescriptor[USB_StdDevReqMaxBuffer + 1];

static bool UsbDevWriteIntoFifo(void *pSrc, uint8_t dataLength, uint8_t *written, uint8_t hostBufferSize);
static bool WaitAckOrSetupFromHost();


// Called by USB-END-OF-RESET interrupt
// USB reset resets EP0! (see section 22.5)

void UsbDevStartDeviceEP0(void)
{
    BoardPortD2GreenOff();
    UsbDevSelectEndpoint(0);
    if UsbDevIsEndpointEnabled()
    {
        Debug("~~~ EP0 already enabled!\r\n");
        return;
    }

    Debug("~~~ Setting up EP0...\r\n");
    UsbAllocatedEPs = 0;
    if (UsbDrv_EndpointSetup(0, UsbEP_TypeControl, EP0_FIFO_Size, 1, UsbEP_DirControl))
        Debug("~~~ Successful set up EP0!\r\n");
    else
        Debug("~~~  Setup of EP0 failed!\r\n");

    // Enable interrupt
    UsbDevEnableReceivedSETUP_Int();
}

static bool SendInFifo()
{
    UsbDevSendControlIn(); // send FIFO

    for (InRetryCnt = 1; InRetryCnt != 0; InRetryCnt++)
    {
        if (UsbDevIsFifoEmpty())
        {
            TxOut += InFifoCnt;
            return true;
        }
    }

    return false;
}

static bool UsbDevFlushInFifo()
{
    if (InFifoCnt != 0)
    {
        if (SendInFifo())
        {
            InFifoCnt = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    return true;
}

static bool UsbDevWriteIntoFifoAndFlush(void *pSrc, uint8_t dataLength, uint8_t *written, uint8_t hostBufferSize)
{
    bool result = true;

    if (UsbDevWriteIntoFifo(pSrc, dataLength, written, hostBufferSize))
    {
        if (!UsbDevFlushInFifo())
        {
            result = false;
        }
    }
    else
    {
        result = false;
    }

    WaitAckOrSetupFromHost();
    return result;
}
// Send buffer to host, but don't send more total bytes than requested (host buffer).
// Stop if there is a ZLP (abort) from host or if all bytes are send, else
// fill FIFO and send FIFO if FIFO is full.

static bool UsbDevWriteIntoFifo(void *pSrc, uint8_t dataLength, uint8_t *written, uint8_t hostBufferSize)
{
    uint8_t len = hostBufferSize - *written; // we may send l bytes more

    if (len > dataLength)
    {
        len = dataLength; // clip to size of this buffer
    }
        // is host buffer full or Flush?
    else if (len == 0 && InFifoCnt != 0)
    {
        if (SendInFifo())
        {
            InFifoCnt = 0;
            return true;
        }
        else
        {
            InFifoCnt = 0;
            (*written) = hostBufferSize;
            FatalError = true;
            return false;
        }
    }

    if (UsbDevIsFifoEmpty())
    {
        InFifoCnt = 0;
    }
    else
    {
        *written = hostBufferSize;
        FatalError = true;
        return false;
    }

    while (len--)
    {

        UsbDevWriteByte(*(uint8_t*) pSrc++); // put byte to FIFO
        TxIn++;
        InFifoCnt++;
        // should use UEBCL
        bool toSend = InFifoCnt >= EP0_FIFO_Size;
        //|| (UsbDevGetByteCountHigh() == 0 && UsbDevGetByteCountLow() >= EP0_FIFO_Size);

        if (toSend) // if FIFO full
        {
            if (SendInFifo())
            {
                InFifoCnt = 0;
            }
            else
            {
                *written = hostBufferSize;
                FatalError = true;
                return false;
            }
        }
    }

    *written += dataLength;
    return true;
}

// Read n bytes from FIFO; FIFO should contain exactly n bytes
// Limited to n < 256

void UsbDevReadBytesN(void *c, uint8_t n)
{
    Assert(UsbDevGetByteCountLow() == n);
    while (n--) *(uint8_t*) c++ = UsbDevReadByteFromFifo();
}

static bool UsbSendZLP(bool waitTXINI)
{
    // yes.
    if (UsbDevIsFifoEmpty())
    {
        UsbDevSendControlIn(); // send ZLP
        if (waitTXINI)
        {
            for (InRetryCnt = 1; InRetryCnt != 0; InRetryCnt++)
            {
                if (UsbDevIsFifoEmpty())
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }
    else
    {
        FatalError = true;
    }

    return false;
}

static bool WaitAckOrSetupFromHost()
{
    for (InRetryCnt = 1; InRetryCnt != 0; InRetryCnt++)
    {
        // is request aborted?
        if (UsbDevHasReceivedOUT_Data()) // ZLP from host -- abort
        {
            UsbDevClearHasReceivedOUT_Data();
            return true;
        }

        else if (UsbDevHasReceivedSETUP())
        {
            return true;
        }
    }
    return false;
}

static bool UsbSendDescriptors(uint8_t cdi, uint8_t requested)
{
    memset(BufferDescriptor, 0, sizeof (BufferDescriptor));

    uint8_t written = 0;
    uint8_t nbByte = 0;

    uint8_t idi; // interface descriptor index
    uint8_t as; // alternate setting of interface
    uint8_t edi; // endpoint descriptor index

    USB_ConfigurationDescriptor* pConfDes = (USB_ConfigurationDescriptor *) & BufferDescriptor[nbByte];
    USB_ConfigurationDescriptor confDes;
    UsbGetConfigurationDescriptor(&confDes, cdi);
    if (UsbGetConfigurationDescriptor(pConfDes, cdi))
    {
        nbByte += pConfDes->bLength;

        //        UsbDumpConfigurationDescriptor(&u.confDes);
        idi = 0;
        do
        {
            as = 0;
            USB_InterfaceDescriptor *pInterfaceDesc = (USB_InterfaceDescriptor *) & BufferDescriptor[nbByte];
            while (UsbGetInterfaceDescriptor(pInterfaceDesc, cdi, idi, as))
            {
                nbByte += pInterfaceDesc->bLength;
                //            UsbDumpInterfaceDescriptor(&u.intDes);

                USB_EndpointDescriptor *pEndpointDesc = (USB_EndpointDescriptor *) & BufferDescriptor[nbByte];

                for (edi = 1; edi < UsbNumEndpointsAT90USB; edi++)
                {
                    if (UsbGetEndpointDescriptor(pEndpointDesc, cdi, idi, as, edi))
                    {
                        nbByte += pEndpointDesc->bLength;
                        pEndpointDesc = (USB_EndpointDescriptor *) & BufferDescriptor[nbByte];
                        //UsbDumpEndpointDescriptor(&u.endDes);
                    }
                }
                as++;
            }
            idi++;
            pInterfaceDesc = (USB_InterfaceDescriptor *) & BufferDescriptor[nbByte];
        }
        while (as > 0);

        return UsbDevWriteIntoFifoAndFlush(BufferDescriptor, nbByte, &written, requested);
    }
    else
    {
        return false;
    }
}

void UsbProcessSetupRequest_Intr(void)
{

    union // we can use the same piece of memory for these descriptors
    {
        USB_DeviceDescriptor devDes;
        USB_ConfigurationDescriptor confDes;
        char strDes[USB_MaxStringDescriptorLength];
    } u;

    uint8_t b;
    UsbDevSelectEndpoint(0);

    UsbDevReadBytesN(&SetupRequest, sizeof(SetupRequest));

    //UsbDumpSetupRequest(&SetupRequest);
    if (UsbIsVendorRequest(SetupRequest.bmRequestType))
    {
        UsbDevAcknowledgeSETUP();
        // is vendor request and for log4usb?
        if (UsbIsVendorRequest(SetupRequest.bmRequestType) && SetupRequest.bRequest == AppLoggerId)
        {        
            // Yes,
            ApplyUsbLoggerCfg_Intr(&SetupRequest);
            
            // should call SendZLP
            UsbDevSendControlIn(); // send ZLP
        }
        else
        {
            // no stall!
            UsbDevRequestStallHandshake();
        }
    }
    else if (UsbIsStandardRequest(SetupRequest.bmRequestType))
    {
        // Caution: We have to delay the AcknowledgeSETUP() if request is a 3 stage-transfer with out data
        // because host may send out data immediately after our acknowledge and may not see our stall request!
        if (!(SetupRequest.bRequest == USB_StdDevReqSET_DESCRIPTOR)) // ! 3 stage-transfer with out data
            UsbDevAcknowledgeSETUP();

        switch (SetupRequest.bRequest)
        {
            case USB_StdDevReqGET_STATUS: // 3 stages with 2 byte IN-data -- not tested yet
                ReqDebug("USB_StdDevReqGET_STATUS");
                Assert(SetupRequest.wValue == 0);
                Assert(SetupRequest.wLength == 2);
                switch (SetupRequest.bmRequestType)
                {
                    case 128: // device: Self-Power-Bit, Remote-Wakeup-Bit set?
                        Assert(SetupRequest.wIndex == 0);
                        if (UsbDevConfValue == UsbUnconfiguredState)
                        {
                            ReqDebug("Error: USB_StdDevReqGET_STATUS device in un-configured state!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        UsbGetConfigurationDescriptor(&u.confDes, UsbDevConfValue);
                        if (u.confDes.bmAttributes & (1 << 6)) b = 1;
                        else b = 0; // self powered?
                        if ((u.confDes.bmAttributes & (1 << 5)) && (RemoteWakeupActive)) b |= (1 << 1);
                        UsbDevWriteByte(b);
                        break;
                    case 129: // interface: Always return 0
                        if (UsbDevConfValue == UsbUnconfiguredState)
                        {
                            ReqDebug("Error: USB_StdDevReqGET_STATUS interface in un-configured state!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        //Assert(SetupReuqest.wIndex < USB_Interfaces[UsbDevConfValue]);
                        UsbDevWriteByte(0);
                        break;
                    case 130: // endpoint: is this endpoint stalled?
                        Assert((MSB(SetupRequest.wIndex) == 0));
                        b = LSB(SetupRequest.wIndex) & 127; // endpoint number
                        if (b >= UsbNumEndpointsAT90USB)
                        {
                            ReqDebug("Error: USB_StdDevReqGET_STATUS endpoint does not exist!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        if ((UsbDevConfValue == UsbUnconfiguredState) && (b > 0))
                        {
                            ReqDebug("Error: USB_StdDevReqGET_STATUS for ep > 0 in un-configured state!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        UsbDevSelectEndpoint(b);
                        if (UsbDevIsEndpointStalled()) // stalled bit is marked write-only in data sheet -- do we need a separate state variable?
                            UsbDevWriteByte(1);
                        else
                            UsbDevWriteByte(0);
                        UsbDevSelectEndpoint(0);
                        break;
                    default:
                        ReqDebug("Error: USB_StdDevReqGET_STATUS!");
                        UsbDevRequestStallHandshake();
                        return;
                }
                UsbDevWriteByte(0);
                UsbDevSendControlIn();
                WaitAckOrSetupFromHost();
                break;
            case USB_StdDevReqCLEAR_FEATURE: // 2 stages (no data-stage) -- not tested yet
            case USB_StdDevReqSET_FEATURE:
                ReqDebug("USB_StdDevReqCLEAR/SET_FEATURE");
                Assert(SetupRequest.wLength == 0);
                switch (SetupRequest.bmRequestType)
                {
                    case 0: // device
                        BoardPortD5RedOff();
                        Assert(SetupRequest.wIndex == 0);
                        if (SetupRequest.wValue != 1) // Feature selector != DEVICE_REMOTE_WAKEUP
                        {
                            ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE wrong feature selector for device!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        if (SetupRequest.bRequest == USB_StdDevReqCLEAR_FEATURE)
                        {
                            RemoteWakeupActive = 0;
                        }
                        else
                        {
                            UsbGetConfigurationDescriptor(&u.confDes, UsbDevConfValue);
                            if (!(u.confDes.bmAttributes & (1 << 5))) // Remote-Wakeup support set in configuration descriptor?
                            {
                                ReqDebug("Error: USB_StdDevReqSET_FEATURE, Remote-Wakeup not supported!");
                                UsbDevRequestStallHandshake();
                                return;
                            }
                            RemoteWakeupActive = 1; // here we only set a flag -- of course this is not enough to reactivate the sleeping bus
                        }
                        break;
                    case 1: // interface -- nothing to do. Should we request a stall handshake?
                        ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE with receiver == interface!");
                        UsbDevRequestStallHandshake();
                        return;
                        break;
                    case 2: // endpoint
                        Assert(MSB(SetupRequest.wIndex) == 0);
                        if (SetupRequest.wValue != 0) // Feature selector != ENDPOINT_STALL
                        {
                            ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE wrong feature selector for endpoint!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        b = LSB(SetupRequest.wIndex) & 127; // endpoint address
                        if (b >= UsbNumEndpointsAT90USB)
                        {
                            ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE endpoint does not exist!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        if ((UsbDevConfValue == UsbUnconfiguredState) && (b > 0))
                        {
                            ReqDebug("Error: USB_StdDevReqClearSetFeature for ep > 0 in un-configured state!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        // Caution: what shall we do if (b == 0)?
                        UsbDevSelectEndpoint(b);
                        if (SetupRequest.bRequest == USB_StdDevReqCLEAR_FEATURE)
                        {
                            UsbDevClearStallRequest();
                            UsbDevResetEndpoint(b); // should we do an endpoint reset?
                            UsbDevResetDataToggleBit();
                        }
                        else
                        {
                            UsbDevRequestStallHandshake(); // for endpoint b
                        }
                        UsbDevSelectEndpoint(0);
                        break;
                    default:
                        ReqDebug("Error: USB_StdDevReqClearSetFeature!");
                        UsbDevRequestStallHandshake();
                        return;
                }
                UsbSendZLP(false);
                break;
            case USB_StdDevReqSET_ADDRESS: // 2 stages (no data-stage)
                ReqDebug("USB_StdDevReqSET_ADDRESS");
                if ((LSB(SetupRequest.wValue) == 0) && (UsbDevConfValue != UsbUnconfiguredState))
                {
                    ReqDebug("Error: USB_StdDevReqSET_ADDRESS address 0 in configured state!");
                    UsbDevRequestStallHandshake();
                    return;
                }

                Assert(SetupRequest.bmRequestType == 0);
                Assert(MSB(SetupRequest.wValue) == 0);
                Assert(SetupRequest.wIndex == 0);
                Assert(SetupRequest.wLength == 0);
                UsbDevSetAddress(LSB(SetupRequest.wValue));

                UsbSendZLP(true); // send ZLP
                //UsbDevWaitTransmitterReady();
                UsbDevEnableAddress();
                break;
            case USB_StdDevReqSET_DESCRIPTOR: // 3 stage-transfer with out data
                ReqDebug("Error: USB_StdDevReqSET_DESCRIPTOR currently not supported!");
                UsbDevRequestStallHandshake(); // RequestStallHandshake() before UsbDevAcknowledgeSETUP()
                UsbDevAcknowledgeSETUP(); // to ensure that host will not send out data!
                // you may implement this request if you have any idea for what you can use it
                break;
            case USB_StdDevReqGET_CONFIGURATION: // 3 stages with 1 byte IN-data -- not tested yet
                ReqDebug("USB_StdDevReqGET_CONFIGURATION");
                Assert(SetupRequest.bmRequestType == 128);
                Assert(SetupRequest.wValue == 0);
                Assert(SetupRequest.wIndex == 0);
                Assert(SetupRequest.wLength == 1);
                UsbDevWriteByte(UsbDevConfValue);
                UsbDevSendControlIn();
                WaitAckOrSetupFromHost();
                break;
            case USB_StdDevReqSET_CONFIGURATION: // 2 stages (no data-stage)
                ReqDebug("USB_StdDevReqSET_CONFIGURATION");
                Assert(SetupRequest.bmRequestType == 0);
                Assert(MSB(SetupRequest.wValue) == 0);
                Assert(SetupRequest.wIndex == 0);
                Assert(SetupRequest.wLength == 0);
                if (UsbDevSetConfiguration(LSB(SetupRequest.wValue)))
                {
                    UsbDevSelectEndpoint(0); // UsbDevSetConfiguration() may select other ep
                    UsbSendZLP(false); // send ZLP
                    BoardPortD5GreenOn();

                }
                else
                {
                    ReqDebug("Error: USB_StdDevReqSET_CONFIGURATION unsupported configuration!");
                    UsbDevSelectEndpoint(0);
                    UsbDevRequestStallHandshake();
                }
                break;
            case USB_StdDevReqGET_INTERFACE: // 3 stages with 1 byte IN-data -- not tested yet
                ReqDebug("USB_StdDevReqGET_INTERFACE");
                if (UsbDevConfValue == UsbUnconfiguredState)
                {
                    ReqDebug("Error: USB_StdDevReqGET_INTERFACE in unconfigured state!");
                    UsbDevRequestStallHandshake();
                    return;
                }
                Assert(SetupRequest.bmRequestType == 129);
                Assert(SetupRequest.wValue == 0);
                Assert(MSB(SetupRequest.wIndex) == 0);
                Assert(SetupRequest.wLength == 1);
                UsbDevWriteByte(AltSettingOfInterface[(uint8_t) LSB(SetupRequest.wIndex)]);
                UsbDevSendControlIn();
                WaitAckOrSetupFromHost();
                break;
            case USB_StdDevReqSET_INTERFACE: // 2 stages (no data-stage)
                ReqDebug("USB_StdDevReqSET_INTERFACE");
                if (UsbDevConfValue == UsbUnconfiguredState)
                {
                    ReqDebug("Error: USB_StdDevReqSET_INTERFACE in unconfigured state!");
                    UsbDevRequestStallHandshake();
                    return;
                }
                Assert(SetupRequest.bmRequestType == 1);
                Assert(MSB(SetupRequest.wValue) == 0);
                Assert(MSB(SetupRequest.wIndex) == 0);
                Assert(SetupRequest.wLength == 0);
                if (UsbApi_SetInterface(UsbDevConfValue, LSB(SetupRequest.wIndex), LSB(SetupRequest.wValue)))
                {
                    UsbDevSelectEndpoint(0); // UsbDevSetInterface() may select other ep
                    UsbSendZLP(false); // send ZLP
                }
                else
                {
                    UsbDevSelectEndpoint(0);
                    ReqDebug("Error: USB_StdDevReqSET_INTERFACE alt. setting does not exist!");
                    UsbDevRequestStallHandshake();
                }
                break;
            case USB_StdDevReqSYNCH_FRAME: // 3 stages with 2 byte IN-data -- not supported
                ReqDebug("USB_StdDevReqSYNCH_FRAME not supported!");
                UsbDevRequestStallHandshake();
                return;
            case USB_StdDevReqGET_DESCRIPTOR: // 3 stages transfer
                ReqDebug("USB_StdDevReqGET_DESCRIPTOR");
                Assert(SetupRequest.bmRequestType == 128);
            {
                uint8_t requested = LSB(SetupRequest.wLength); // we will never send more than 255 bytes
                uint8_t written = 0;
                if (MSB(SetupRequest.wLength) != 0)
                {
                    requested = USB_StdDevReqMaxBuffer;
                }

                switch (MSB(SetupRequest.wValue))
                {
                    case 1: // Device-Descriptor
                        Assert(SetupRequest.wIndex == 0);
                        Assert(LSB(SetupRequest.wValue) == 0);
                        UsbGetDeviceDescriptor(&u.devDes);
                        UsbDumpDeviceDescriptor(&u.devDes);
                        UsbDevWriteIntoFifoAndFlush(&u.devDes, u.devDes.bLength, &written, requested);
                        break;
                    case 2: // Configuration-Descriptor
                        if (!UsbSendDescriptors(LSB(SetupRequest.wValue), requested))
                        {
                            ReqDebug("Error: USB_StdDevReqGET_DESCRIPTOR: can not sent Configuration-Descriptor!");
                            UsbDevRequestStallHandshake();
                            return;
                        }
                        break;
                    case 3: // String-Descriptor
                        UsbGetStringDescriptor(u.strDes, LSB(SetupRequest.wValue)); // strDes is an array, so no & is necessary!
                        UsbDumpStringDescriptor(u.strDes);
                        UsbDevWriteIntoFifoAndFlush(u.strDes, u.strDes[0], &written, requested);
                        break;
                    default:
                        ReqDebug("USB_StdDevReqGET_DESCRIPTOR: Unknown type!");
                        return;
                }
            }
                break;
            default:
                ReqDebug("Received unsupported UsbStandardRequest!");
                UsbDevRequestStallHandshake();
        }
    }
    else
    {
        ReqDebug("Unsupported nonstandard request!");
        UsbDevRequestStallHandshake();
    }
}

