
//#include <avr/interrupt.h>
#include <stdint.h>
#include "usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h"
#include "usb_requests.h"
#include "UsbEndpointBase.h"

#ifndef USBENDPOINTIN 
#define USBENDPOINTIN

class UsbEndpointIn : public UsbEndpointBase
{
public:
    UsbEndpointIn(uint8_t endpointNumber);

    bool InitializeEndpoint(uint8_t type, uint16_t size, uint8_t banks);

    virtual void ProcessInterrupt();

    bool IsUsbDevFifoEmpty();

    void ClearFifoControllBit();

    void SetNextToggleData0();

};

#endif