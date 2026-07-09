
#include "Usb_InEndpoint.h"
#include "Usb_api.h"
#include "com_def.h"
#include "At90UsbKey.h"
//#include "usb_drv.h"

extern const UsbEndpointCfg_t UsbEndpointCfg[ENDPOINT_MAX_CFG];
extern UsbEndpointData_t UsbEPData[ENDPOINT_MAX_CFG];
extern volatile bool FatalError;

static bool WaitInBankReadyToWrite(UsbInEndpointData_t* inData);
/// <summary>
/// Triggers sending IN data and wait for completion.
/// </summary>
/// <param name="inData">Pointer to the IN endpoint data structure.</param>
/// <returns>true if the IN FIFO was ready and inData->TxOut was incremented; false otherwise.</returns>
static bool SendInFifo(UsbInEndpointData_t* inData)
{
    // clear bank free.
    UsbDevSendControlIn();
    
    // Start sending data and switch bank if required.
    UsbDevSendInData();
    
    // Is Next bank reday?
    if (WaitInBankReadyToWrite(inData))
    {
        // update stats of previous bank.
        inData->TxOut += inData->InFifoCnt;
        
        // clear FifoCnt current bank.
        inData->InFifoCnt = 0;
        
        return true;
    }

    return false;
}

/// <summary>
///  Waits for the IN FIFO to be ready to write.
/// </summary>
/// <param name="inData"></param>
/// <returns></returns>
static bool WaitInBankReadyToWrite(UsbInEndpointData_t* inData)
{
    for (inData->InRetryCnt = 1; inData->InRetryCnt != 0; inData->InRetryCnt++)
    {
//        if (UsbDevIsFifoEmpty() && UsbDevIsFifoControllBitSet())
        if (UsbDevIsFifoEmpty() && UsbDevIsFifoControllBitSet() && UsbDevWriteAllowed())
        {
//            UsbDevClearTransmitterReady();
            return true;
        }
    }               

    return false;   
}

/// <summary>
/// Attempts to flush the IN FIFO for a USB endpoint by sending any pending data.
/// </summary>
/// <param name="inData">Pointer to the endpoint's UsbInEndpointData_t structure. The function checks inData->InFifoCnt and, if nonzero, calls SendInFifo(inData) and clears InFifoCnt on success.</param>
/// <returns>true if there was no data to send or if sending the pending data succeeded; false if sending failed.</returns>
static bool FlushInFifo(UsbInEndpointData_t* inData)
{
    if (inData->InFifoCnt != 0)
    {
        if (SendInFifo(inData))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    return true;
}

/// <summary>
/// Writes data into the specified USB IN endpoint FIFO and flushes the FIFO; returns true if both the write and the flush succeed.
/// </summary>
/// <param name="epNumber">USB IN endpoint number.</param>
/// <param name="pSrc">Pointer to the source buffer containing the data to write to the endpoint FIFO.</param>
/// <param name="dataLength">Number of bytes to write from pSrc into the FIFO.</param>
/// <returns>true if the FIFO write succeeded and the IN FIFO was successfully flushed; otherwise false.</returns>
bool Usb_InEndpointWriteFifoAndFlush(uint8_t epNumber, void* pSrc, uint8_t dataLength)
{
    bool result = true;

    if (Usb_InEndpointWriteFifo(epNumber, pSrc, dataLength))
    {
        if (!FlushInFifo(&UsbEPData[epNumber - 1].In))
        {
            result = false;
        }
    }
    else
    {
        result = false;
    }

    return result;
}

void Usb_InEnpoint_HandleInterrupt(uint8_t epNumber)
{
    uint8_t sep = UsbDevGetEndpoint();
    UsbDevSelectEndpoint(epNumber);
    if(UsbDevNAK_ResponseSendToInRequest())
    { 
        // compiler internal error.
        //UsbInEndpointData_t* inData = &UsbEPData[ep - 1].In;      
        //inData->InNAKCnt++;
        // compiler internal error.
        //UsbEndpointData_t* inData = &UsbEPData[ep - 1];      
        //inData->.In.InNAKCnt++;

        // hope it is working?
        
        uint16_t* inData2 = &(UsbEPData[epNumber - 1].In.InNAKCnt);      
        inData2++;

        UsbDevClearNAK_ResponseInBit();
        
    }

    UsbDevSelectEndpoint(sep);
}

/// <summary>
/// Writes up to dataLength bytes from pSrc into the USB IN endpoint FIFO for the specified endpoint. 
/// Validates the endpoint, selects it, waits for FIFO readiness, writes bytes, and triggers FIFO transmission when full. 
/// On error the global FatalError is set and the function returns false.
/// </summary>
/// <param name="epNumber">The USB IN endpoint number to write to (must be non-zero and match the endpoint configuration).</param>
/// <param name="pSrc">Pointer to the source buffer containing the bytes to write into the FIFO.</param>
/// <param name="dataLength">Number of bytes to write from pSrc into the endpoint FIFO.</param>
/// <returns>true if all bytes were written and any required FIFO sends succeeded; false on failure (in which case FatalError is set).</returns>
bool Usb_InEndpointWriteFifo(uint8_t epNumber, void* pSrc, uint8_t dataLength)
{
    if(epNumber == 0)
    {
        FatalError = true;
        return false;
    }

    const UsbEndpointCfg_t* cfgInfo = &UsbEndpointCfg[epNumber - 1];
    if(cfgInfo->bEndpointAddress != UsbInEndpointAdress(epNumber))
    {
        FatalError = true;
        return false;
    }

    UsbInEndpointData_t* inData = &UsbEPData[epNumber - 1].In;

    UsbDevSelectEndpoint(epNumber);

    if (!WaitInBankReadyToWrite(inData))
    {
        // shown busy
        BoardPortD2RedOn();

        //FatalError = true;
        return false;
    }
    else
    {
        BoardPortD2RedOff();       
    }

    uint8_t* src = (uint8_t*)pSrc;
    while (dataLength--)
    {
        UsbDevWriteByte(*src); // put byte to FIFO
        src++;
        inData->TxIn++;
        inData->InFifoCnt++;

        // is bank's FIFO full ?
        if (!UsbDevWriteAllowed()) 
        {
            // Yes send it and wait for completion before writing more data.
            if (!SendInFifo(inData))
            {
                // no Error!
                BoardPortD2RedOn();
                return false;
            }
            else
            {
                BoardPortD2RedOff();       
            }
        }
    }
    return true;
}
