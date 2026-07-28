#ifndef _USB_IN_ENDPOINT_H_
#define _USB_IN_ENDPOINT_H_

#include "usb_drv.h" 

bool Usb_InEndpointWriteFifoAndFlush(uint8_t epNumber, void* pSrc, uint8_t dataLength);
bool Usb_InEndpointWriteFifo(uint8_t epNumber, void* pSrc, uint8_t dataLength);

bool Usb_InEndpointAbort(uint8_t epNumber);

#endif

