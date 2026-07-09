
#include <avr/interrupt.h>
#include "Usb_InEndpoint.h"
#include "Usb_api.h"
#include "com_def.h"
#include "usb_drv.h"

extern const UsbEndpointCfg_t UsbEndpointCfg[ENDPOINT_MAX_CFG];
extern UsbEndpointData_t UsbEPData[ENDPOINT_MAX_CFG];
extern volatile bool FatalError;
extern uint32_t GetTimerTick();
extern bool IsTimeExpired(uint32_t oldtick, uint8_t timeout);

/// <summary>
/// Triggers sending IN data and wait for completion.
/// </summary>
/// <param name="outData">Pointer to the IN endpoint data structure.</param>
/// <returns>true if the IN FIFO was ready and outData->TxOut was incremented; false otherwise.</returns>
bool Usb_OutEndpoint_ReadFifoFromBuffer(uint8_t epNumber, void* dst, uint8_t dataLength, uint8_t timeoutMs)
{
    UsbOutEndpointData_t* outData = &UsbEPData[epNumber - 1].Out;
    UsbDevSelectEndpoint(epNumber);
    uint8_t* dstPtr = (uint8_t*)dst;
    uint32_t startTick = GetTimerTick();
    
    uint8_t rxincli;
    cli();
    rxincli = outData->RxIn;
    sei();

    // read requested number of byte.
    while (dataLength && !IsTimeExpired(startTick, timeoutMs))
    {
        while (outData->RxOut != rxincli)
        {
            *dstPtr = outData->Buffer[outData->RxOut % sizeof(outData->Buffer)]; // put byte to FIFO
            dstPtr++;
            outData->RxOut++;
            dataLength--;
        }
    }
    
    return dataLength == 0;
}

void Usb_OutEnpoint_HandleInterrupt(uint8_t epNumber)
{
    uint8_t sep = UsbDevGetEndpoint();
    UsbDevSelectEndpoint(epNumber);

    UsbOutEndpointData_t* outData = &UsbEPData[epNumber - 1].Out;
    while (UsbDevHasReceivedOUT_Data())
    {
        UsbDevClearHasReceivedOUT_Data(); // clear flag and FIFO, see section 22.13, page 274 

        if (UsbDevIsFifoControllBitSet())
        {
            while (UsbDevReadAllowed())
            {
                outData->Buffer[outData->RxIn % sizeof(outData->Buffer)] = UsbDevReadByteFromFifo();
                outData->RxIn++;
            }

            UsbDevClearFifoControllBit();
        }
    }

    UsbDevSelectEndpoint(sep);

}
