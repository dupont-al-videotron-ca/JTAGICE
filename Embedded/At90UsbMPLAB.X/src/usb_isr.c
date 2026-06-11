// AT90USB/usb_isr.c
// USB Interrupt Service Routines
// S. Salewski 22-MAR-2007

#include <avr/interrupt.h>
#include <stdint.h>
#include "usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h"
#include "usb_requests.h"
#include "at90usbkey.h"
#include "Timer2CTC.h"

extern uint8_t USB_COM_mask;
uint8_t ResetCnt = 0;

// USB General Interrupt Handler (Figure 21.11)
// USB Registers: USBINT.0, USBINT.1, UDINT
ISR(USB_GEN_vect)
{
  Debug("ISR(USB_GEN_vect)\r\n");
  if UsbIsIDTI_FlagSet()
  {
    UsbClearIDTI_Flag();
    Debug("=== IDTI_FlagSet\r\n");
  }
  if UsbIsVBUSTI_FlagSet()
  {
    UsbClearVBUSTI_Flag();
    if(!UsbIsVBUS_PinHigh())
    {
        FatalError = false;
        BoardPortDLedsOff();
    }
    
    Debug("=== VBUSTI_FlagSet\r\n");
  }
  if UsbDevIsEndOfResetFlagSet()
  {
    ResetCnt++;
    FatalError = false;
    BoardPortDLedsOff();
    UsbDevClearEndOfResetFlag();
    UsbDevStartDeviceEP0();
    Debug("=== EndOfResetFlagSet\r\n");
  }
  if UsbDevIsWakeupCPU_FlagSet()
  {
    UsbDevDisableWakeupCPU_Int();
    UsbDevClearWakeupCPU_Flag();
   // UsbStartPLL();
    Debug("=== WakeupCPU_FlagSet\r\n");
  }
}

// USB Endpoint/Pipe Interrupt Handler (Figure 21.12)
// Endpoint Registers: UEINTX, UESTAX.6 and UESTAX.5
// Setup-Request may reset endpoints, so we process data endpoints first!
// User defined actions have to acknowledge the interrupt!
ISR(USB_COM_vect)
{
  uint8_t mask;
  uint8_t ep;
  Debug("ISR(USB_COM_vect)\r\n");
  USB_COM_mask = UsbDevGetEndpointIntBits();  
  mask = UsbDevGetEndpointIntBits();
  ep = UsbNumEndpointsAT90USB;
  while (ep-- > 0)
  {
      
    if (mask & (1<<ep))
    {
      UsbDevSelectEndpoint(ep);
      UEIENX = 0;
#if 0      
      switch (ep)
      {
        case 0:
          if UsbDevHasReceivedSETUP()
          {
            SetBit(PORTB,DDB5);
            BoardPortD5GreenOn();
            UsbProcessSetupRequest();
            BoardPortD5GreenOff();
            ClearBit(PORTB,DDB5);
          }
          break;
        case 1:
          UsbDevEP1IntAction();
          break;
        case 2:
          UsbDevEP2IntAction();
          break;
        case 3:
          UsbDevEP3IntAction();
          break;
        case 4:
          UsbDevEP4IntAction();
          break;
        case 5:
          UsbDevEP5IntAction();
          break;
        case 6:
          UsbDevEP6IntAction();
          break;
        default:
          Debug("Error in ISR(USB_COM_vect)\r\n");
      }
#endif    
    }
  }
}
