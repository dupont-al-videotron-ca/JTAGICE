// AT90USB/usart_debug.h
// A few macros for debugging by sending messages over the serial port
// S. Salewski 09-SEP-2007

#ifndef _USART_DEBUG_H_
#define _USART_DEBUG_H_

#include "defines.h"
#include "usart_drv.h"
#include "usb_spec.h"

#ifdef DEBUG
#define Debug(msg) USART_WriteString(msg)
//#define ReqDebug(msg) USART_WriteString(msg " (" __FILE__ ")\r\n")
#define ReqDebug(msg) USART_WriteString("|" msg "\r\n")
#else
#define Debug(msg)
#define ReqDebug(msg)
#endif

#ifdef DEBUG
#define Assert(exp) if ((exp) == 0) USART_WriteString("Error: (" #exp ") == 0 (" __FILE__ ")\r\n");
#else
#define Assert(exp)
#endif

#ifdef DEBUG
void UsbDumpDeviceDescriptor(USB_DeviceDescriptor *d);
void UsbDumpConfigurationDescriptor(USB_ConfigurationDescriptor *c);
void UsbDumpInterfaceDescriptor(USB_InterfaceDescriptor *i);
void UsbDumpEndpointDescriptor(USB_EndpointDescriptor *e);
void UsbDumpStringDescriptor(char *s);
void UsbDumpSetupRequest(USB_DeviceRequest *req);
#else
#define UsbDumpDeviceDescriptor(d)
#define UsbDumpConfigurationDescriptor(c)
#define UsbDumpInterfaceDescriptor(i)
#define UsbDumpEndpointDescriptor(e)
#define UsbDumpStringDescriptor(s)
#define UsbDumpSetupRequest(req)
#endif

#endif
