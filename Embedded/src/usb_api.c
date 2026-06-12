// AT90USB/usb_api.c
// This file contains functions which are project specific and should be modified
// to customize the USB-driver.
// S. Salewski 21-MAR-2007

#include <stdint.h>
#include <stdbool.h>
#include "defines.h"
#include "com_def.h"
#include "usart_debug.h"
#include "usb_spec.h"
#include "usb_drv.h"
#include "daq_dev.h" // ProcessUserCommand(), DAQ_Result
#include "usb_api.h"
#include "usb_ControlEndpoint.h"

// The behavior of USB devices is specified by different descriptors defined by www.usb.org.
// In the simplest case a device has one Device-Descriptor, one Configuration-Descriptor,
// one Interface-Descriptor and a few Endpoint-Descriptors. For more advanced devices
// there may exist multiple Configuration- and Interface-Descriptors, each with multiple Endpoint-
// Descriptors. We may store all these structures in arrays or linked lists, which takes
// much storage in RAM and is not easy to handle.
// My currently preferred solution to handle the descriptors is to generate them on the fly if
// they are requested by the host. This is simple and consumes very few RAM.
// If a descriptor is needed during enumeration process, one of the functions
// of this file is called to initialize the structure according to specific parameters,
// and then the structure is passed to the USB host. The user has to modify the constants
// and the functions of this file for a specific device. For simple devices this is very 
// easy -- for devices with multiple configurations/interfaces it is more demanding.
// You may add constants, variables or functions to this file, i.e. to remember the state of an ep.

// First let us write down the parameters of the endpoints which we want to use.
// We have to remember these facts during enumeration, when filling or reading the FIFO,
// and for the communication of the host with our device.

// The current version of this file is adapted to a simple Data-Acquisition-Device.
// Our DAQ device has only one configuration with one single interface.
// Numbers of configurations, interfaces, alternate settings and endpoints start with 0,
// configuration 0 indicates unconfigured (addressed) state, and endpoint 0 is the control endpoint.
// Our device use control endpoint 0 and data endpoints 1, 2 and 3 of interface 0.
// Commands are send to our device by vendor-requests using endpoint 0.
// Endpoint 1 is configured as Bulk-IN-endpoint and used to transfer status information
// from device to the host. Whenever the host tries to read data from empty endpoint 1,
// a NAK-interrupt is generated, causing firmware to write status information to this ep.
// Endpoint 2 is configured as Bulk-IN-Endpoint and used to send DAQ-data to the host.
// DAQ operation starts when host sends a vendor request. Controlled by a periodically
// activated timer interrupt, daq data is put to the FIFO of endpoint 2.
// Endpoint 3 is configured as Bulk-OUT-Endpoint and used to send a single byte from the
// host to the device. This byte sets the pins of digital port B of our AT90USB. 

// To keep it simple: Only one configuration, only one interface.
// EP1: Bulk-IN, 8 bytes FIFO, one bank.
// EP2: Bulk-IN, 64 bytes FIFO, dual bank.
// EP3: Bulk-OUT, 8 bytes FIFO, one bank.

const uint8_t USB_Interfaces[USB_MaxConfigurations] = {1, 0, 0, 0}; // number of interfaces in each configuration
const uint8_t USB_MaxPower_2mA[USB_MaxConfigurations] = {50, 0, 0, 0}; // power consumption of each configuration in 2mA units
const uint8_t USB_AltSettings[USB_MaxConfigurations][USB_MaxInterfaces] =
	{{1, 0, 0, 0}, // number of alt. settings of interfaces of first configuration 
	 {0, 0, 0, 0}, // number of alt. settings of interfaces of second configuration 
	 {0, 0, 0, 0},
	 {0, 0, 0, 0}};
const uint8_t USB_Endpoints[USB_MaxConfigurations][USB_MaxInterfaces] =
	{{3, 0, 0, 0}, // number of endpoints of interfaces of first configuration 
	 {0, 0, 0, 0}, // number of endpoints of interfaces of second configuration 
	 {0, 0, 0, 0},
	 {0, 0, 0, 0}};

static void UsbDevDisableAndFreeEndpoint(uint8_t i);
static void UsbDevSetUnconfiguredState(void);

void
UsbGetDeviceDescriptor(USB_DeviceDescriptor *d)
{
  d->bLength = USB_DeviceDescriptorLength;
  d->bDescriptorType = USB_DeviceDescriptorType;
  d->bcdUSB = USB_Spec1_1;
  d->bDeviceClass = UsbNoDeviceClass;
  d->bDeviceSubClass = UsbNoDeviceSubClass;
  d->bDeviceProtocoll = UsbNoDeviceProtokoll;
  d->bMaxPacketSize0 = EP0_FIFO_Size;
  d->idVendor = MyUSB_VendorID;
  d->idProduct = MyUSB_ProductID;
  d->bcdDevice = MyUSB_DeviceBCD;
  d->iManufacturer = USB_ManufacturerStringIndex;
  d->iProduct = USB_ProductStringIndex;
  d->iSerialNumber = USB_SerialNumberStringIndex;
  d->bNumConfigurations = USB_NumConfigurations;
}

bool UsbGetConfigurationDescriptor(USB_ConfigurationDescriptor *c, uint8_t confIndex)
{
  uint8_t i;
  if (confIndex >= USB_NumConfigurations) return false; 
  c->bLength = USB_ConfigurationDescriptorLength;
  c->bDescriptorType = USB_ConfigurationDescriptorType;
  c->wTotalLength = USB_ConfigurationDescriptorLength;
  i = USB_Interfaces[confIndex];
  c->bNumInterfaces = i;
  while (i-- > 0)
    c->wTotalLength += (USB_InterfaceDescriptorLength+USB_EndpointDescriptorLength*USB_Endpoints[confIndex][i])*USB_AltSettings[confIndex][i];
  c->bConfigurationValue = UsbConfigurationValue(confIndex);
  c->iConfiguration = UsbNoDescriptionString; // no textual configuration description
  c->bmAttributes = UsbConfDesAttrBusPowered; // bus-powered, no remote wakeup
  c->MaxPower = USB_MaxPower_2mA[confIndex];
  return true;
}

bool
UsbGetInterfaceDescriptor(USB_InterfaceDescriptor *i, uint8_t confIndex, uint8_t intIndex, uint8_t altSetting)
{
  if ((confIndex>=USB_NumConfigurations)||(intIndex>=USB_Interfaces[confIndex])||(altSetting>=USB_AltSettings[confIndex][intIndex])) return false;
  i->bLength = USB_InterfaceDescriptorLength;
  i->bDescriptorType = USB_InterfaceDescriptorType;
  i->bInterfaceNumber = intIndex;
  i->bAlternateSetting = altSetting;
  i->bNumEndpoints = USB_Endpoints[confIndex][intIndex];
  i->bInterfaceClass = UsbNoInterfaceClass;
  i->bInterfaceSubClass = UsbNoInterfaceSubClass;
  i->bInterfaceProtocol = UsbNoInterfaceProtokoll;
  i->iInterface = UsbNoDescriptionString; // no textual interface description
  return true;
}

// Not used for EP0, so 1 <= endIndex <= USB_NumEndpoints <= 6
bool
UsbGetEndpointDescriptor(USB_EndpointDescriptor *e, uint8_t confIndex, uint8_t intIndex, uint8_t altSetting, uint8_t endIndex)
{
  if ((confIndex>=USB_NumConfigurations)||(intIndex>=USB_Interfaces[confIndex])||(altSetting>=USB_AltSettings[confIndex][intIndex])||
      (endIndex>USB_Endpoints[confIndex][intIndex])) return false;
  // components identical for all of our endpoints
  e->bLength = USB_EndpointDescriptorLength;
  e->bDescriptorType = USB_EndpointDescriptorType;
  e->bmAttributes = USB_BulkTransfer;
  e->bInterval = 0; // bulk endpoint, no polling
  // components which differ
  switch (endIndex) // only endpoints for interface 0 in our application
  {
    case 1: 
      e->bEndpointAddress = UsbInEndpointAdress(1);
      e->wMaxPacketSize = EP1_FIFO_Size;
    break;
    case 2: 
      e->bEndpointAddress = UsbInEndpointAdress(2);
      e->wMaxPacketSize = EP2_FIFO_Size;
    break;
    case 3: 
      e->bEndpointAddress = UsbOutEndpointAdress(3);
      e->wMaxPacketSize = EP3_FIFO_Size;
    break;
  }
  return true;
}

void
UsbGetStringDescriptor(char s[], uint8_t index)
{
  uint8_t i;
#if (USB_MaxStringDescriptorLength < 18)
#error USB_MaxStringDescriptorLength too small!
#endif
  i = USB_MaxStringDescriptorLength;
  while (i--) *s++ = '\0';
  s -= USB_MaxStringDescriptorLength;
  s[1] = USB_StringDescriptorType;
  switch (index)
  {
    case USB_LanguageDescriptorIndex: // == 0
      s[0] = 4;
      s[2] = 9; // two byte language code, only support for English
      s[3] = 4;
      break;
    case USB_ManufacturerStringIndex:
      s[2] = 'S';
      s[4] = 'A';
      s[6] = 'L';
      s[8] = 'E';
      s[10] = 'W';
      s[12] = 'S';
      s[14] = 'K';
      s[16] = 'I';
      s[0] = 18; // length of descriptor
      break;
    case USB_ProductStringIndex:
      s[2] = 'A';
      s[4] = 'T';
      s[6] = '9';
      s[8] = '0';
      s[10] = 'U';
      s[12] = 'S';
      s[14] = 'B';
      s[0] = 16;
      break;
    case USB_SerialNumberStringIndex:
      s[2] = '0';
      s[4] = '0';
      s[6] = '1';
      s[0] = 8;
      break;
    default:
      s[2] = '?';
      s[0] = 4;
  }
}

static void
UsbDevDisableAndFreeEndpoint(uint8_t i)
{
  UsbDevSelectEndpoint(i);
  UsbDevDisableEndpoint();
  UsbDevClearEndpointAllocBit();
}

static void
UsbDevSetUnconfiguredState(void)
{
  uint8_t i;
  UsbDevConfValue = UsbUnconfiguredState;
  i =  UsbNumEndpointsAT90USB;
  while (--i > 0) // free all endpoints but EP0
    UsbDevDisableAndFreeEndpoint(i);
  UsbAllocatedEPs = 1;
}

// A device can have multiple configurations. In the simplest case configurations may differ only in power consumption.
// But configurations can be totally different (differ in number of interfaces, endpoints, ...
bool
UsbDevSetConfiguration(uint8_t c)
{
  uint8_t i;
  switch (c)
  {
    case 0: // go back to unconfigured (addressed) state
      UsbDevSetUnconfiguredState();
      return true;
      break;
    case 1: // set configuration 1
      if (UsbDevConfValue != c)
      {
        i = USB_MaxInterfaces;
        while (i-- > 0) AltSettingOfInterface[i] = UsbInterfaceUnconfigured;
      }
      for (i = 0; i < USB_Interfaces[c-1]; i++)
      {
        if (!UsbDevSetInterface(c, i , 0))
        {
          UsbDevSetUnconfiguredState();
          return false;
        }
      }
      UsbDevConfValue = c;
      return true;
      break;
    default:
      Debug("UsbDevSetConfiguration(): configuration does not exist!\r\n");
      return false;
  }
}

// Multiple interfaces can exist at the same time! These interfaces have to use different endpoints.
// For example interface 1 can use ep1 and ep2, and interface 2 can use some of the other endpoints (ep3, ep4, ep5, ep6).
// A special problem occurs from FIFO memory management of AT90USB: Memory for FIFO has to be allocated in growing order.
// If we use multiple interfaces with multiple alternate settings there may occur memory conflicts concerning FIFO memory:
// If we allocate memory for endpoint i, there may be an overlap with FIFO of endpoint i+1 or memory of endpoint i+1 may slide.
// This is a result of internal design of AT90USB, see section 21.7 in datasheet.
// But from USB design interfaces should be independent, changes of interface i should not affect interface j#i
// To prevent conflicts, following strategy is useful:
// Use endpoints 1 to n for interface 1, endpoints n+1 to m for interface 2, and endpoints m+1, m+2 ... for interface 3.
// Give only more than one alternate setting to the interface with the highest number. So changes of alternate setting
// with new FIFO sizes can not disturb other interfaces.
// If you really need more than one interface with different alternate settings (endpoint FIFO sizes) you may try to
// insert unused dummy endpoints to prevent memory slides or overlaps. Or use different configurations or force reallocation of all endpoints.
// Our application uses only interface 0 with endpoints ep1, ep2, ep3. But the code is designed to support more interfaces.
bool UsbDevSetInterface(uint8_t conf, uint8_t inf, uint8_t as)
{
  uint8_t i;
  if (conf-- == UsbUnconfiguredState) // each interface should be bound to a configuration
  {
    Debug("UsbDevSetInterface(): called from unconfigured (addressed) state!\r\n");
    return false;
  }
  if ((inf >= USB_Interfaces[conf]) || (as >= USB_AltSettings[conf][inf]))
  {
    Debug("UsbDevSetInterface(): interface not supported!\r\n"); 
    return false;
  }
  if ((as > 0) && (inf != (USB_Interfaces[conf]-1)))
  {
    Debug("UsbDevSetInterface(): Multiple interfaces with more than one alternate setting => FIFO memory conflicts may occur!\r\n"); 
    return false;
  }
  if (AltSettingOfInterface[inf] == as) // no changes, reset toggle bits of endpoints of this interface
  {
    if (conf == 0)
    {
      if (inf == 0) // first interface of first configuration 
      {
        for (i = 1; i < 4; i++) // reset toggle bit of all endpoints of this interface; an endpoint reset may be necessary too
        {
          UsbDevSelectEndpoint(i);
          UsbDevResetEndpoint(i);
          UsbDevResetDataToggleBit();
        }
      }
      else if (inf == 1) // second interface of first configuration
      {
        // reset endpoints of second interface
      }
    }
    else if (conf == 2) // similar operations 
    {
    }
    return true;
  }
  if (conf == 0)
  {
    if (inf == 0) // first allocation or reallocation with new alternate setting
    {
      UsbDevDisableAndFreeEndpoint(3);
      UsbDevDisableAndFreeEndpoint(2);
      UsbDevDisableAndFreeEndpoint(1);
      UsbAllocatedEPs = 1;
      AltSettingOfInterface[0] = UsbInterfaceUnconfigured;
      if (as == 0) // use alternate setting 0 
      {
        if (UsbDevEP_Setup(1, UsbEP_TypeBulk, EP1_FIFO_Size, 1, UsbEP_DirIn))
        {
          Debug("!!! Successful set up EP1!\r\n");
          //UsbDevSelectEndpoint(1); this ep is already selected by UsbDevEP_Setup()
          UsbDevEnableNAK_IN_Int(); // trigger interrupt when host got a NAK as a result of a read request
        }
        else
        {
          Debug("!!!  Setup of EP1 failed!\r\n"); // should not occur ;-)
          return false;
        }
        if (UsbDevEP_Setup(2, UsbEP_TypeBulk, EP2_FIFO_Size, 2, UsbEP_DirIn))
        {
          Debug("!!! Successful set up EP2!\r\n");
        }
        else
        {
          Debug("!!!  Setup of EP2 failed!\r\n");
          return false;
        }
        if (UsbDevEP_Setup(3, UsbEP_TypeBulk, EP3_FIFO_Size, 1, UsbEP_DirOut))
        {
          Debug("!!! Successful set up EP3!\r\n");
          UsbDevEnableReceivedOUT_DATA_Int(); // trigger interrupt when out data is available
        }
        else
        {
          Debug("!!!  Setup of EP3 failed!\r\n");
          return false;
        }
      }
      else if (as == 1) // alternate setting 1
      {
        // set up same endpoints with different parameters (FIFO-size)
      }
      AltSettingOfInterface[0] = as;
      UsbStartupFinished = 1;
      return true;
    }
    else if (inf == 1) // setup interface 1 
    {
    }
  }
  else if (conf == 1) // similar setup if configuration 2 with other interfaces is selected
  {
  }
  return false; // dummy to suppress compiler warning 
}

void
UsbDevProcessVendorRequest(USB_DeviceRequest *req)
{
  ProcessUserCommand(req->bRequest, req->wValue, req->wIndex);
}

// This function is called whenever host tries to read data from ep1
// We send a status byte which indicates success of DAQ operation
void
UsbDevFillEP1FIFO(void)
{
  if UsbDevIsFifoEmpty()
  {
    UsbDevClearTransmitterReady();
    UsbDevClearNAK_ResponseInBit();
    UsbDevWriteByte(DAQ_Result);
    UsbDevSendInData();
  }
}

// This function is called whenever OUT FIFO has data for us
void
UsbDevReadEP3FIFO(void)
{
  if (UsbDevHasReceivedOUT_Data())
  {
    UsbDevClearHasReceivedOUT_Data();
    if (UsbDevReadAllowed())
    {
      DDRB = 0xFF;
      PORTB = UsbDevReadByteFromFifo();
      UsbDevClearFifoControllBit(); // maybe we should use an alias for this macro
    }
  }
}
