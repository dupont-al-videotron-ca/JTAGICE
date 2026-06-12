
#include <stdlib.h>
#include <string.h>
#include "macros.h"
#include "At90UsbKey.h"
#include "usbEndpointControl.h"
#include "At90UsbDevice.h"

// TODO:: obsolete WaitZLP_FromHost
//extern void WaitZLP_FromHost();
// TODO:: obsolete UsbDevWriteDescriptor
//extern void UsbDevWriteDescriptor(void *d, uint8_t *written, uint8_t requested);
// TODO:: obsolete UsbSendDescriptors
//extern bool UsbSendDescriptors(uint8_t cdi, uint8_t *written, uint8_t requested);

UsbEndpointControl::UsbEndpointControl() : UsbEndpointBase(0)
{
    _txBuffer = new RingBufferByte((USB_DeviceDescriptorLength +USB_ConfigurationDescriptorLength+USB_InterfaceDescriptorLength+USB_EndpointDescriptorLength)*UsbNumEndpointsAT90USB);

    memset(&_setupRequest, 0, sizeof(USB_DeviceRequest));
    RemoteWakeupActive = 0;
    GoReset();
    _nakInCnt = 0;
    _nakOutCnt = 0;
    _stallSentCnt = 0;
}

bool UsbEndpointControl::InitializeEndpoint(uint16_t size, uint8_t banks)
{
    // Base on usb_drc::UsbDevStartDeviceEP0
    this->SelectEndpoint();
    if UsbDevIsEndpointEnabled() // can this be true?
    {
        Debug("~~~ EP0 already enabled!\r\n");
    }
    else
    {
        // TODO:: to be move in AT90UsbDevice class.
        UsbAllocatedEPs = 0;
        if(!this->InitializeUECFG(UsbEP_TypeControl, size, banks, UsbEP_DirControl))
        {
            // TODO:: disable endpoint
            Debug("~~~ EP0 failed to initialize!\r\n");
            return false;
        }
        
        // enable interrupt
        UsbDevEnableReceivedSETUP_Int();
        UsbDevEnableReceivedOUT_DATA_Int();
        //UsbDevEnableTransmitterReadyInt();
    }
    
    GoWaitSetupReq();
    return true;
}


void UsbEndpointControl::OnVBusChange(bool active)
{
    if(active)
    {
        GoWaitSetupReq();
    }
    else
    {
        // TODO::
    }
}

void UsbEndpointControl::RequestStallHandshake()
{
    UsbDevRequestStallHandshake();
    GoWaitStalled();
}

// beware that race condition on endpoint number selection
void UsbEndpointControl::HandleInterrupt()
{
    uint8_t epSaved = UsbDevGetSelectedEndpoint();
    volatile StateEnum intrState = this->_state;
    
    UsbEndpointBase::HandleInterrupt();

    if(_endpointInterruptStatus.U_UEIENX.Flags.RXSTP_flg)
    {
        UsbDevReadBytesN(&_setupRequest, USB_DeviceRequestSize);
        UsbDevDisableReceivedSETUP_Int();
    }
    
    if(_endpointInterruptStatus.U_UEIENX.Flags.TXIN_flg)
    {
        _endpointInterruptStatus.U_UEIENX.Flags.TXIN_flg = 0;

        // sending done by pulling
        UsbDevDisableTransmitterReadyInt();
        
        //switch (intrState)
        //{
        //case ST_WAIT_SETUP_INDATA:
        ////UsbDevClearTransmitterReady();
        ////FillFifoIn();
        //break;
        //
        //case ST_WAIT_INSTATUS:
        //case ST_WAIT_OUTSTATUS:
        //case ST_WAIT_SETUP_REQ:
        //default:
        //break;
        //}
    }
    
    if(_endpointInterruptStatus.U_UEIENX.Flags.RXOUT_flg)
    {
        _endpointInterruptStatus.U_UEIENX.Flags.RXOUT_flg = 0;

        if(intrState == ST_WAIT_INSTATUS || intrState == ST_WAIT_SETUP_INDATA)
        {
            GoWaitSetupReq();
            UsbDevClearHasReceivedOUT_Data();
        }
        
        if(intrState == ST_WAIT_SETUP_OUTDATA)
        {
            //TODO:: Read Fifo!
        }
    }

    UsbDevSelectEndpoint(epSaved);
}

void UsbEndpointControl::ProcessInterrupt()
{
    cli();
    volatile EndpointInterruptStatus epInterruptStatus = _endpointInterruptStatus;
    sei();
    
    this->SelectEndpoint();
    
    if(epInterruptStatus.U_UEIENX.Data != 0)
    {
        if(epInterruptStatus.U_UEIENX.Flags.RXSTP_flg)
        {
            // Wouach!
            if(_state != ST_WAIT_SETUP_REQ)
            {
                GoWaitSetupReq();
            }
            
            //if(!UsbDevReadBytesN(&_setupRequest, USB_DeviceRequestSize))
            //{
            //UsbDevAcknowledgeSETUP();
            //RequestStallHandshake();
            //return;
            //}

            // TODO: remove next if after completed test.
            // Caution: We have to delay the AcknowledgeSETUP() if request is a 3 stage-transfer with out data
            // because host may send out data immediately after our acknowledge and may not see our stall request!
            if (!(_setupRequest.bRequest == USB_StdDevReqSET_DESCRIPTOR)) // ! 3 stage-transfer with out data
            {
                // clear RXSTP to send ACK to Host
                UsbDevAcknowledgeSETUP();
                UsbDevEnableReceivedSETUP_Int();
            }

            if (UsbIsStandardRequest(_setupRequest.bmRequestType))
            {
                ProcessStandardRequest();
            }
            else if (UsbIsVendorRequest(_setupRequest.bmRequestType))
            {
                // TODO::
                ReqDebug("Received UsbVendorRequest");
                RequestStallHandshake();
                return;
                //UsbDevProcessVendorRequest(&_setupRequest);
                //GoWaitStatus();
                //UsbDevSendControlIn(); // send ZLP
            }
            else if(UsbIsClassRequest(_setupRequest.bmRequestType))
            {
                ReqDebug("Class request not implemented");
                RequestStallHandshake();
                return;
            }
            else
            {
                ReqDebug("Unsupported nonstandard request!");
                RequestStallHandshake();
            }
        }
        
        if(epInterruptStatus.U_UEIENX.Flags.RXOUT_flg)
        {
            epInterruptStatus.U_UEIENX.Flags.RXOUT_flg = 0;
            UsbDevClearHasReceivedOUT_Data();
            if(_state == ST_WAIT_INSTATUS || _state == ST_WAIT_SETUP_INDATA)
            {
                // Request completed or cancel
                _txBuffer->Clear();
                GoWaitSetupReq();
            }
            else if(_state == ST_WAIT_SETUP_OUTDATA)
            {
                // TODO:: Read data.
                GoWaitOutStatus();
            }
        }

        if(epInterruptStatus.U_UEIENX.Flags.TXIN_flg)
        {
            epInterruptStatus.U_UEIENX.Flags.TXIN_flg = 0;
            if(_state == ST_WAIT_SETUP_INDATA)
            {
                if(!_txBuffer->IsEmpty())
                {
                    FillFifoIn();
                }
                else
                {
                    //What to do with interrupt?
                    UsbDevClearTransmitterReady();
                    GoWaitInStatus();
                }
            }
            else if(_state == ST_WAIT_OUTSTATUS)
            {
                // Request completed
                
                //What to do with interrupt?
                UsbDevClearTransmitterReady();
                
                GoWaitSetupReq();
            }
        }
        
        if(epInterruptStatus.U_UEIENX.Flags.NAKIN_flg)
        {
            epInterruptStatus.U_UEIENX.Flags.NAKIN_flg = 0;
            UsbDevClearNAK_ResponseInBit();
            _nakInCnt++;
        }
        
        if(epInterruptStatus.U_UEIENX.Flags.NAKOUT_flg)
        {
            epInterruptStatus.U_UEIENX.Flags.NAKOUT_flg = 0;
            UsbDevClearNAK_ResponseOutBit();
            _nakOutCnt++;
        }
        
        if(epInterruptStatus.U_UEIENX.Flags.STALLED_flg)
        {
            epInterruptStatus.U_UEIENX.Flags.STALLED_flg = 0;
            UsbDevClearStallRequest();
            _stallSentCnt++;
            GoWaitSetupReq();
        }
        
        
    }
}

void UsbEndpointControl::ProcessStandardRequest()
{
    uint8_t data = 0;
    ReqDebug("Received UsbStandardRequest");
    
    // Device To Host request
    if(UsbIsDataDeviceToHost(_setupRequest.bmRequestType))
    {
        // TODO :: CLI
        _txBuffer->Clear();
        GoWaitInData();
        
        switch (_setupRequest.bRequest)
        {
            case USB_StdDevReqGET_STATUS: // 3 stages with 2 byte IN-data -- not tested yet
            {
                
                ReqDebug("USB_StdDevReqGET_STATUS");
                Assert(_setupRequest.wValue == 0);
                Assert(_setupRequest.wLength == 2);

                if(UsbIsRequestForDevice(_setupRequest.bmRequestType))
                {
                    Assert(_setupRequest.wIndex == 0);
                    if (At90UsbDevice.UsbDevConfValue == UsbUnconfiguredState)
                    {
                        ReqDebug("Error: USB_StdDevReqGET_STATUS device in unconfigured state!");
                        _txBuffer->Clear();
                        RequestStallHandshake();
                        return;
                    }
                    
                    USB_ConfigurationDescriptor confDes;
                    At90UsbDevice.FillConfigurationDescriptor(&confDes, At90UsbDevice.UsbDevConfValue);
                    if (confDes.bmAttributes & (1<<6))
                    {
                        data = 1;
                    }
                    else
                    {
                        data = 0; // self powered?
                    }

                    if ((confDes.bmAttributes & (1<<5)) && (this->RemoteWakeupActive))
                    {
                        data |= (1<<1);
                    }

                    WriteTxBuffer(data);
                }
                else if(UsbIsRequestForInterface(_setupRequest.bmRequestType))
                {

                    if (At90UsbDevice.UsbDevConfValue == UsbUnconfiguredState)
                    {
                        ReqDebug("Error: USB_StdDevReqGET_STATUS interface in unconfigured state!");
                        _txBuffer->Clear();
                        RequestStallHandshake();
                        return;
                    }

                    //Assert(req.wIndex < USB_Interfaces[UsbDevConfValue]);
                    WriteTxBuffer(0);
                }
                else if (UsbIsRequestForEndpoint(_setupRequest.bmRequestType))
                {
                    Assert((MSB(_setupRequest.wIndex) == 0));
                    data = LSB(_setupRequest.wIndex) & 127; // endpoint number
                    
                    if (data >= UsbNumEndpointsAT90USB)
                    {
                        ReqDebug("Error: USB_StdDevReqGET_STATUS endpoint does not exist!");
                        _txBuffer->Clear();
                        RequestStallHandshake();
                        return;
                    }
                    
                    if ((At90UsbDevice.UsbDevConfValue == UsbUnconfiguredState) && (data > 0))
                    {
                        ReqDebug("Error: USB_StdDevReqGET_STATUS for ep > 0 in unconfigured state!");
                        _txBuffer->Clear();
                        RequestStallHandshake();
                        return;
                    }

                    // TODO:: Get UsbEndpointBase object instead and move to UsbEndpointBase .
                    
                    UsbDevSelectEndpoint(data);
                    bool stalled = UsbDevSTALLHandshakeSend();
                    
                    SelectEndpoint();
                    
                    if (stalled) // stalled bit is marked write-only in data sheet -- do we need a separate state variable?
                    {
                        WriteTxBuffer(1); // lsb
                    }
                    else
                    {
                        WriteTxBuffer(0); // lsb
                    }
                }
                else
                {
                    ReqDebug("Error: USB_StdDevReqGET_STATUS!");
                    _txBuffer->Clear();
                    RequestStallHandshake();
                    return;
                }
                
                WriteTxBuffer(0); // msb
                FillFifoIn();// start sending
                break; // USB_StdDevReqGET_STATUS
            }

            case USB_StdDevReqGET_CONFIGURATION: // 3 stages with 1 byte IN-data -- not tested yet
            {
                
                ReqDebug("USB_StdDevReqGET_CONFIGURATION");
                Assert(_setupRequest.wValue == 0);
                Assert(_setupRequest.wIndex == 0);
                Assert(_setupRequest.wLength == 1);
                
                WriteTxBuffer(At90UsbDevice.UsbDevConfValue);
                FillFifoIn();// start sending
                break; // USB_StdDevReqGET_CONFIGURATION
            }
            
            case USB_StdDevReqGET_INTERFACE: // 3 stages with 1 byte IN-data -- not tested yet
            {
                
                ReqDebug("USB_StdDevReqGET_INTERFACE");
                if (At90UsbDevice.UsbDevConfValue == UsbUnconfiguredState)
                {
                    ReqDebug("Error: USB_StdDevReqGET_INTERFACE in unconfigured state!");
                    _txBuffer->Clear();
                    RequestStallHandshake();
                    return;
                }
                
                Assert(_setupRequest.wValue == 0);
                Assert(MSB(_setupRequest.wIndex) == 0);
                Assert(_setupRequest.wLength == 1);
                
                WriteTxBuffer(At90UsbDevice.AltSettingOfInterface[(uint8_t) LSB(_setupRequest.wIndex)]);

                GoWaitInData();
                WriteTxBuffer(At90UsbDevice.UsbDevConfValue);
                FillFifoIn();// start sending
                break; // USB_StdDevReqGET_INTERFACE
            }


            case USB_StdDevReqGET_DESCRIPTOR: // 3 stages transfer
            {
                
                ReqDebug("USB_StdDevReqGET_DESCRIPTOR");
                if (UsbIsRequestForDevice(_setupRequest.bmRequestType));
                {
                    
                    switch (MSB(_setupRequest.wValue))
                    {
                        case USB_DescriptorTypeDevice: // Device-Descriptor
                        {
                            USB_DeviceDescriptor devDes;
                            Assert(_setupRequest.wIndex == 0);
                            Assert(LSB(_setupRequest.wValue) == 0);
                            At90UsbDevice.FillDeviceDescriptor(&devDes);
                            UsbDumpDeviceDescriptor(&devDes);
                            WriteTxBuffer(&devDes, devDes.bLength);
                            FillFifoIn();
                        }
                        return;
                        break;
                        
                        case USB_DescriptorTypeConfiguration: // Configuration-Descriptor
                        {
                            
                            uint8_t cfgIndex = LSB(_setupRequest.wValue);
                            volatile bool result = true;
                            result = this->SendDescriptorsNeverCall(cfgIndex);
                            if (result)
                            {
                                FillFifoIn();
                                return;
                            }
                            else
                            {
                                ReqDebug("Error: USB_StdDevReqGET_DESCRIPTOR: can not sent Configuration-Descriptor!");
                                _txBuffer->Clear();
                                RequestStallHandshake();
                                return;
                            }
                            
                            return;
                        }
                        
                        return;
                        break;
                        case USB_DescriptorTypeString: // String-Descriptor
                        {
                            char strDes[USB_MaxStringDescriptorLength];
                            
                            At90UsbDevice.FillStringDescriptor(strDes, LSB(_setupRequest.wValue)); // strDes is an array, so no & is necessary!
                            UsbDumpStringDescriptor(strDes);
                            WriteTxBuffer(&strDes, strDes[0]);
                            FillFifoIn();
                            break;
                        }
                        
                        default:
                        {
                            ReqDebug("USB_StdDevReqGET_DESCRIPTOR: Unknown type or not supported!");
                            _txBuffer->Clear();
                            RequestStallHandshake();
                            return;
                            
                        }
                    }
                    
                    if UsbDevHasReceivedOUT_Data() // got ZLP from host -- abort!
                    {
                        UsbDevClearHasReceivedOUT_Data();
                        ReqDebug("USB_StdDevReqGET_DESCRIPTOR: Abort from host!");
                    }
                }
                break; // USB_StdDevReqGET_DESCRIPTOR
            }
            default:
            {
                At90UsbDevice.FatalError();
                ReqDebug("Received unsupported UsbStandardRequest!");
                RequestStallHandshake();
                break;
            }
        }
    }
    //  Host to Device
    else if (UsbIsDataDeviceToHost(_setupRequest.bmRequestType))
    {
        GoWaitOutData();
        
        switch (_setupRequest.bRequest)
        {

            case USB_StdDevReqCLEAR_FEATURE: // 2 stages (no data-stage) -- not tested yet
            case USB_StdDevReqSET_FEATURE:
            {
                
                ReqDebug("USB_StdDevReqCLEAR/SET_FEATURE");
                Assert(_setupRequest.wLength == 0);

                if(UsbIsRequestForDevice(_setupRequest.bmRequestType))
                {
                    Assert(_setupRequest.wIndex == 0);
                    if (_setupRequest.wValue != 1) // Feature selector != DEVICE_REMOTE_WAKEUP
                    {
                        ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE wrong feature selector for device!");
                        RequestStallHandshake();
                        return;
                    }
                    if (_setupRequest.bRequest == USB_StdDevReqCLEAR_FEATURE)
                    {
                        // TODO:: move RemoteWakeupActive to at90UsbDevice
                        RemoteWakeupActive = 0;
                    }
                    else
                    {
                        USB_ConfigurationDescriptor confDes;
                        At90UsbDevice.FillConfigurationDescriptor(&confDes, At90UsbDevice.UsbDevConfValue);
                        if (!(confDes.bmAttributes & (1<<5))) // Remote-Wakeup support set in configuration descriptor?
                        {
                            ReqDebug("Error: USB_StdDevReqSET_FEATURE, Remote-Wakeup not supported!");
                            RequestStallHandshake();
                            return;
                        }

                        // TODO:: move RemoteWakeupActive to at90UsbDevice
                        RemoteWakeupActive = 1; // here we only set a flag -- of course this is not enough to reactivate the sleeping bus
                    }
                }
                else if UsbIsRequestForInterface(_setupRequest.bmRequestType)
                {
                    ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE with receiver == interface!");
                    RequestStallHandshake();
                    return;
                }
                else if UsbIsRequestForEndpoint(_setupRequest.bmRequestType)
                {
                    Assert(MSB(_setupRequest.wIndex) == 0);
                    if (_setupRequest.wValue != 0) // Feature selector != ENDPOINT_STALL
                    {
                        ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE wrong feature selector for endpoint!");
                        RequestStallHandshake();
                        return;
                    }
                    data = LSB(_setupRequest.wIndex) & 127; // endpoint address
                    if (data >= UsbNumEndpointsAT90USB)
                    {
                        ReqDebug("Error: USB_StdDevReqSET/CLEAR_FEATURE endpoint does not exist!");
                        RequestStallHandshake();
                        return;
                    }
                    if ((At90UsbDevice.UsbDevConfValue == UsbUnconfiguredState) && (data > 0))
                    {
                        ReqDebug("Error: USB_StdDevReqClearSetFeature for ep > 0 in unconfigured state!");
                        RequestStallHandshake();
                        return;
                    }

                    // Caution: what shall we do if (b == 0)?
                    // TODO:: get UsbEndpointBase object.
                    UsbDevSelectEndpoint(data);
                    if (_setupRequest.bRequest == USB_StdDevReqCLEAR_FEATURE)
                    {
                        // TODO:: get UsbEndpointBase object.
                        UsbDevClearStallRequest();
                        UsbDevResetEndpoint(data); // should we do an endpoint reset?
                        UsbDevResetDataToggleBit();
                    }
                    else
                    {
                        RequestStallHandshake(); // for endpoint b
                    }

                    SelectEndpoint();
                }
                else
                {

                    ReqDebug("Error: USB_StdDevReqClearSetFeature!");
                    RequestStallHandshake();
                    return;
                }
                UsbDevSendControlIn(); // send ZLP
                break; // USB_StdDevReqSET_FEATURE, USB_StdDevReqCLEAR_FEATURE

                case USB_StdDevReqSET_ADDRESS: // 2 stages (no data-stage)
                ReqDebug("USB_StdDevReqSET_ADDRESS");
                if ((LSB(_setupRequest.wValue) == 0) && (At90UsbDevice.UsbDevConfValue != UsbUnconfiguredState))
                {
                    ReqDebug("Error: USB_StdDevReqSET_ADDRESS address 0 in configured state!");
                    RequestStallHandshake();
                    return;
                }
                Assert(_setupRequest.bmRequestType == 0);
                Assert(MSB(_setupRequest.wValue) == 0);
                Assert(_setupRequest.wIndex == 0);
                Assert(_setupRequest.wLength == 0);
                UsbDevSetAddress(LSB(_setupRequest.wValue));

                UsbDevSendControlIn(); // send ZLP
                //UsbDevWaitTransmitterReady();
                UsbDevEnableAddress();
                break; // USB_StdDevReqSET_ADDRESS
            }

            case USB_StdDevReqSET_DESCRIPTOR: // 3 stage-transfer with out data
            {
                
                ReqDebug("Error: USB_StdDevReqSET_DESCRIPTOR currently not supported!");
                RequestStallHandshake(); // RequestStallHandshake() before UsbDevAcknowledgeSETUP()
                UsbDevAcknowledgeSETUP(); // to ensure that host will not send out data!
                UsbDevEnableReceivedSETUP_Int();
                // you may implement this request if you have any idea for what you can use it
                break; // USB_StdDevReqSET_DESCRIPTOR
            }


            case USB_StdDevReqSET_CONFIGURATION: // 2 stages (no data-stage)
            {
                
                ReqDebug("USB_StdDevReqSET_CONFIGURATION");
                Assert(UsbIsDataHostToDevice(_setupRequest.bmRequestType)  == 0);
                Assert(UsbIsStandardRequest(_setupRequest.bmRequestType)  == 0);
                Assert(MSB(_setupRequest.wValue) == 0);
                Assert(_setupRequest.wIndex == 0);
                Assert(_setupRequest.wLength == 0);

                if (At90UsbDevice.SetConfiguration(LSB(_setupRequest.wValue)))
                {
                    SelectEndpoint(); // UsbDevSetConfiguration() may select other ep
                    UsbDevSendControlIn(); // send ZLP
                }
                else
                {
                    ReqDebug("Error: USB_StdDevReqSET_CONFIGURATION unsupported configuration!");
                    SelectEndpoint();
                    RequestStallHandshake();
                }
                break; // USB_StdDevReqSET_CONFIGURATION

            }

            case USB_StdDevReqSET_INTERFACE: // 2 stages (no data-stage)
            {
                
                ReqDebug("USB_StdDevReqSET_INTERFACE");
                if (At90UsbDevice.UsbDevConfValue == UsbUnconfiguredState)
                {
                    ReqDebug("Error: USB_StdDevReqSET_INTERFACE in unconfigured state!");
                    RequestStallHandshake();
                    return;
                }
                
                Assert(UsbIsDataHostToDevice(_setupRequest.bmRequestType) == 0);
                Assert(UsbIsRequestForInterface(_setupRequest.bmRequestType) != 0);

                Assert(MSB(_setupRequest.wValue) == 0);
                Assert(MSB(_setupRequest.wIndex) == 0);
                Assert(_setupRequest.wLength == 0);

                // TODO:: get UsbEndpointBase
                if (At90UsbDevice.SetInterface(At90UsbDevice.UsbDevConfValue, LSB(_setupRequest.wIndex), LSB(_setupRequest.wValue)))
                {
                    UsbDevSendControlIn(); // send ZLP
                }
                else
                {
                    ReqDebug("Error: USB_StdDevReqSET_INTERFACE alt. setting does not exist!");
                    RequestStallHandshake();
                }
                break; // USB_StdDevReqSET_INTERFACE
            }

            case USB_StdDevReqSYNCH_FRAME: // 3 stages with 2 byte IN-data -- not supported
            {
                
                ReqDebug("USB_StdDevReqSYNCH_FRAME not supported!");
                RequestStallHandshake();
                return; //USB_StdDevReqSYNCH_FRAME
            }

            default:
            {
                At90UsbDevice.FatalError();
                ReqDebug("Received unsupported UsbStandardRequest!");
                RequestStallHandshake();
            }
            break;
        }
        
    }
}

bool UsbEndpointControl::SendDescriptorsNeverCall(uint8_t cdIndex)
{ 
    //uint8_t cdi; // configuration descriptor index
    uint8_t idIndex = 0; // interface descriptor index
    uint8_t as = 0;  // alternate setting of interface
    uint8_t edIndex = 0; // endpoint descriptor index
    BufferDescriptors_U descriptorsBuffer;
    

    if (At90UsbDevice.FillConfigurationDescriptor(&descriptorsBuffer.confDes, cdIndex))
    {
        UsbDumpConfigurationDescriptor(&descriptorsBuffer.confDes);
        WriteTxBuffer(((void*)&descriptorsBuffer.confDes), descriptorsBuffer.confDes.bLength);
        idIndex = 0;
        do
        {
            as = 0;
            while (At90UsbDevice.FillInterfaceDescriptor(&descriptorsBuffer.intDes, cdIndex, idIndex, as))
            {
                UsbDumpInterfaceDescriptor(&descriptorsBuffer.intDes);
                WriteTxBuffer(&descriptorsBuffer.intDes, descriptorsBuffer.intDes.bLength);
                for (edIndex = 1; edIndex < UsbNumEndpointsAT90USB; edIndex++)
                {
                    if (At90UsbDevice.FillEndpointDescriptor(&descriptorsBuffer.endDes, cdIndex, idIndex, as, edIndex))
                    {
                        UsbDumpEndpointDescriptor(&descriptorsBuffer.endDes);
                        WriteTxBuffer(&descriptorsBuffer.endDes, descriptorsBuffer.endDes.bLength);
                    }
                }
                as++;
            }
            idIndex++;
        } while (as > 0);
        
        return true;
    }
    else
    {
        At90UsbDevice.FatalError();
        return false;
    }
}

bool UsbEndpointControl::WriteTxBuffer(void *d, size_t requested)
{
    return WriteTxBuffer(((uint8_t *)d), requested);
}

// todo:: move to base class.
bool UsbEndpointControl::WriteTxBuffer(uint8_t *d, size_t requested)
{
    volatile size_t length = requested;
    volatile size_t writen = 0;
    
    do
    {
        writen = _txBuffer->Write(d, length);
        length -= writen;
        
    } while(length > 0) ;
    
    return true;
}

// todo:: move to base class.
bool UsbEndpointControl::WriteTxBuffer(uint8_t data)
{
    return _txBuffer->Write(data) == 1;
}

bool UsbEndpointControl::FillFifoIn()
{
    //uint8_t intrReg = DisableInterrupt();
    uint8_t written = 0;
    uint8_t data = 0;
    
    volatile uint8_t msbCnt = UEBCHX;
    volatile uint8_t lsbCnt = UEBCLX;
    
    // fill FIFO
    while (!_txBuffer->IsEmpty())
    {
        BoardPortD5GreenOn();
        
        if (_txBuffer->Read(&data, 1) == 1)
        {
            UsbDevWriteByte(data); // put byte to FIFO
            lsbCnt = UEBCLX;
            
            //if (((written++) % EP0_FIFO_Size) == 0) // if FIFO full
            if ((lsbCnt >= EP0_FIFO_Size) || _txBuffer->IsEmpty()) // if FIFO full
            {
                if(!UsbDevIsTransmitterReady())
                {
                    At90UsbDevice.FatalError();
                }
                
                BoardPortD5RedOn();

                
                //EnableInterrupt(intrReg);
                GoWaitInData();
                UsbDevSendControlIn();  // Clear TXINI
                UsbDevSendInData();  // Clear FIFOCON
                // wait interrupt will fill FIFO
                
                //                return true;
                //              while (!(UsbDevHasReceivedOUT_Data() || UsbDevTransmitterReady()));
                while (1)
                {
                    lsbCnt = UEBCLX;
                    if UsbDevHasReceivedOUT_Data() // ZLP from host -- abort
                    {
                        // prevent further writes
                        //EnableInterrupt(intrReg);
                        
                        UsbDevClearHasReceivedOUT_Data();
                        BoardPortD5AmberOn();
                        
                        return false;
                    }
                    
                    if(UsbDevIsTransmitterReady())
                    {
                        break;
                    }
                    else
                    {
                        BoardPortD5RedToggle();
                    }
                }
                
                BoardPortD5RedOff();
                //intrReg = DisableInterrupt();
            }
        }
        else
        {
            //EnableInterrupt(intrReg);
            At90UsbDevice.FatalError();
            return false;
        }
    }
    
    BoardPortD5GreenOff();

    //EnableInterrupt(intrReg);
    return true;
}