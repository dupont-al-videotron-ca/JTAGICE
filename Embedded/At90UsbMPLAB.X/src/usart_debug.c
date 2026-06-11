// AT90USB/usart_debug.c
// Debugging by sending messages over the serial port
// S. Salewski 22-MAR-2007

#include "defines.h"

#ifdef DEBUG

#include "usart_debug.h"
#include "usb_spec.h"
#include "usart_drv.h"

void
UsbDumpDeviceDescriptor(USB_DeviceDescriptor *d)
{
  USART_WriteString("USB Device-Descriptor:");
  USART_WriteString("\r\n  d->bLength: "); USART_WriteHex(d->bLength);
  USART_WriteString("\r\n  d->bDescriptorType: "); USART_WriteHex(d->bDescriptorType);
  USART_WriteString("\r\n  d->bcdUSB: "); USART_WriteHexW(d->bcdUSB);
  USART_WriteString("\r\n  d->bDeviceClass: "); USART_WriteHex(d->bDeviceClass);
  USART_WriteString("\r\n  d->bDeviceSubClass: "); USART_WriteHex(d->bDeviceSubClass);
  USART_WriteString("\r\n  d->bDeviceProtocoll: "); USART_WriteHex(d->bDeviceProtocoll);
  USART_WriteString("\r\n  d->bMaxPacketSize0: "); USART_WriteHex(d->bMaxPacketSize0);
  USART_WriteString("\r\n  d->idVendor: "); USART_WriteHexW(d->idVendor);
  USART_WriteString("\r\n  d->idProduct: "); USART_WriteHexW(d->idProduct);
  USART_WriteString("\r\n  d->bcdDevice: "); USART_WriteHexW(d->bcdDevice);
  USART_WriteString("\r\n  d->iManufacturer: "); USART_WriteHex(d->iManufacturer);
  USART_WriteString("\r\n  d->iProduct: "); USART_WriteHex(d->iProduct);
  USART_WriteString("\r\n  d->iSerialNumber: "); USART_WriteHex(d->iSerialNumber);
  USART_WriteString("\r\n  d->bNumConfigurations: "); USART_WriteHex(d->bNumConfigurations);
  USART_NewLine();
}

void
UsbDumpConfigurationDescriptor(USB_ConfigurationDescriptor *c)
{
  USART_WriteString(" -USB Configuration-Descriptor:");
  USART_WriteString("\r\n    c->bLength: "); USART_WriteHex(c->bLength);
  USART_WriteString("\r\n    c->bDescriptorType: "); USART_WriteHex(c->bDescriptorType);
  USART_WriteString("\r\n    c->wTotalLength: "); USART_WriteHexW(c->wTotalLength);
  USART_WriteString("\r\n    c->bNumInterfaces: "); USART_WriteHex(c->bNumInterfaces);
  USART_WriteString("\r\n    c->bConfigurationValue: "); USART_WriteHex(c->bConfigurationValue);
  USART_WriteString("\r\n    c->iConfiguration: "); USART_WriteHex(c->iConfiguration);
  USART_WriteString("\r\n    c->bmAttributes: "); USART_WriteHex(c->bmAttributes);
  USART_WriteString("\r\n    c->MaxPower: "); USART_WriteHex(c->MaxPower);
  USART_NewLine();
}

void
UsbDumpInterfaceDescriptor(USB_InterfaceDescriptor *i)
{
  USART_WriteString("   -USB Interface-Descriptor:");
  USART_WriteString("\r\n      i->bLength: "); USART_WriteHex(i->bLength);
  USART_WriteString("\r\n      i->bDescriptorType: "); USART_WriteHex(i->bDescriptorType);
  USART_WriteString("\r\n      i->bInterfaceNumber: "); USART_WriteHex(i->bInterfaceNumber);
  USART_WriteString("\r\n      i->bAlternateSetting: "); USART_WriteHex(i->bAlternateSetting);
  USART_WriteString("\r\n      i->bNumEndpoints: "); USART_WriteHex(i->bNumEndpoints);
  USART_WriteString("\r\n      i->bInterfaceClass: "); USART_WriteHex(i->bInterfaceClass);
  USART_WriteString("\r\n      i->bInterfaceSubClass: "); USART_WriteHex(i->bInterfaceSubClass);
  USART_WriteString("\r\n      i->bInterfaceProtocol: "); USART_WriteHex(i->bInterfaceProtocol);
  USART_WriteString("\r\n      i->iInterface: "); USART_WriteHex(i->iInterface);
  USART_NewLine();
}

void
UsbDumpEndpointDescriptor(USB_EndpointDescriptor *e)
{
  USART_WriteString("     -USB Endpoint-Descriptor:");
  USART_WriteString("\r\n        e->bLength: "); USART_WriteHex(e->bLength);
  USART_WriteString("\r\n        e->bDescriptorType: "); USART_WriteHex(e->bDescriptorType);
  USART_WriteString("\r\n        e->bEndpointAddress: "); USART_WriteHex(e->bEndpointAddress);
  USART_WriteString("\r\n        e->bmAttributes: "); USART_WriteHex(e->bmAttributes);
  USART_WriteString("\r\n        e->wMaxPacketSize: "); USART_WriteHexW(e->wMaxPacketSize);
  USART_WriteString("\r\n        e->bInterval: "); USART_WriteHex(e->bInterval);
  USART_NewLine();
}

void
UsbDumpStringDescriptor(char *s)
{
  uint8_t i;
  i = *s;
  USART_WriteString("USB String-Descriptor:\r\n");
  while (i--) USART_WriteHex(*s++);
  USART_NewLine();
}

void
UsbDumpSetupRequest(USB_DeviceRequest *req)
{
  USART_WriteString("Setup Request:");
  USART_WriteString("  \r\nbmRequestType: "); USART_WriteHex(req->bmRequestType);
  USART_WriteString("  \r\nbRequest:      "); USART_WriteHex(req->bRequest);
  USART_WriteString("  \r\nwValue:        "); USART_WriteHexW(req->wValue);
  USART_WriteString("  \r\nwIndex:        "); USART_WriteHexW(req->wIndex);
  USART_WriteString("  \r\nwLength:       "); USART_WriteHexW(req->wLength);
  USART_NewLine();
}

#endif
