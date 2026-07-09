// AT90USB/usb_isr.c
// USB Interrupt Service Routines
// S. Salewski 22-MAR-2007
// TODO: move to usb_drv.c, make more generic and support more endpoints
#include <avr/interrupt.h>
#include <stdint.h>
#include "usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h"
#include "usb_ControlEndpoint.h"
#include "at90usbkey.h"
#include "Timer2CTC.h"

extern USB_DeviceRequest SetupRequest;
uint8_t ResetCnt = 0;

extern void UsbDevReadBytesN(void *c, uint8_t n);
extern void Usb_OutEnpoint_HandleInterrupt(uint8_t epNumber);
extern void Usb_InEnpoint_HandleInterrupt(uint8_t ep);
            
uint8_t Vbuscnt = 0;
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
        if (!UsbIsVBUS_PinHigh())
        {
            Vbuscnt--;
        }
        else
        {
            Vbuscnt++;
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
    mask = UsbDevGetEndpointIntBits();
    ep = UsbNumEndpointsAT90USB;
    uint8_t current_ep = UsbDevGetEndpoint();

    while (ep-- > 0)
    {

        if (mask & (1 << ep))
        {
            switch (ep)
            {
                case UsbNumEmpointControl:
                    UsbDevSelectEndpoint(ep);
                    if (UsbDevHasReceivedSETUP())
                    {
                        BoardPortD2GreenOn();
                        UsbProcessSetupRequest_Intr();
                        BoardPortD2GreenOff();
                    }
                    
                    if (UsbDevNAK_ResponseSendToOutRequest())
                    {
                        UsbDevClearNAK_ResponseOutBit();
                    }

                    if (UsbDevSTALLHandshakeSend())
                    {
                        UsbDevClearSTALLHandshakeSend();
                    }

                    break;
                case 1:
                    Usb_InEnpoint_HandleInterrupt(ep);
                    break;
                case 2:
                    Usb_InEnpoint_HandleInterrupt(ep);
                    break;
                case 3:
//                    Usb_OutEnpoint_HandleInterrupt(ep);
                    break;
                case 4:
                case 5:
                case 6:
                    UsbDevSelectEndpoint(ep);
                    UEIENX = 0; // disable all interrupts of this endpoint
                    break;
                default:
                    Debug("Error in ISR(USB_COM_vect)\r\n");
            }
        }
    }

    UsbDevSelectEndpoint(current_ep);
}
