#ifndef _USB_OUT_ENDPOINT_H_
#define _USB_OUT_ENDPOINT_H_

#include "usb_drv.h" 

bool Usb_OutEndpoint_ReadFifoFromBuffer(uint8_t epNumber, void* dst, uint8_t dataLength, uint8_t timeoutMs);

#endif

