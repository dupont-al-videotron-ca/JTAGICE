// AT90USB/usb_requests.h
// Handling of USB (standard) requests
// S. Salewski 22-MAR-2007

#ifndef _USB_REQUESTS_H_
#define _USB_REQUESTS_H_

#include <stdint.h>
#include <stdbool.h>
#include "usb_spec.h"
#include "usb_drv.h"
#include "usb_api.h" // USB_MaxInterfaces

// USB-Standard-Device-Requests, H.J. Kelm USB 2.0, section 2.9.1, page 108
#define USB_StdDevReqMaxBuffer      255 // maximum buffer size


extern uint8_t UsbDevConfValue; // current configuration of our device; 0 is unconfigured state
extern uint8_t AltSettingOfInterface [USB_MaxInterfaces]; // current alternate setting of active interfaces

void UsbProcessSetupRequest_Intr(void);
void UsbDevStartDeviceEP0(void);


#endif
