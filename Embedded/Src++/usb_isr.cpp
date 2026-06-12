// AT90USB/usb_isr.c
// USB Interrupt Service Routines
// S. Salewski 22-MAR-2007

#ifdef __XC
#include <xc.h>
#else
#include <avr/interrupt.h>
#endif

#include <stdint.h>
#include "usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h"
#include "usb_requests.h"
#include "at90UsbDevice.h"

extern AT90UsbDevice At90UsbDevice;

// USB General Interrupt Handler (Figure 21.11)
// USB Registers: USBINT.0, USBINT.1, UDINT
ISR(USB_GEN_vect)
{
	Debug("ISR(USB_GEN_vect)\r\n");
	At90UsbDevice.HandleDeviceInterrupt();
}

// USB Endpoint/Pipe Interrupt Handler (Figure 21.12)
// Endpoint Registers: UEINTX, UESTAX.6 and UESTAX.5
// Setup-Request may reset endpoints, so we process data endpoints first!
// User defined actions have to acknowledge the interrupt!
ISR(USB_COM_vect)
{
	
	At90UsbDevice.HandleEndpointInterrupt();
	//uint8_t mask;
	//uint8_t ep;
	//Debug("ISR(USB_COM_vect)\r\n");
	//mask = UsbDevGetEndpointIntBits();
	//while(mask != 0)
	//{
		//UsbDevGetEndpointIntBits()
		//ep = UsbNumEndpointsAT90USB;
		//while (ep-- > 0)
		//{
			//if (mask & (1<<ep))
			//{
				//UsbDevSelectEndpoint(ep);
				//switch (ep)
				//{
					//case 0:
					//PORTD |= (1<<PORTD6);
					//
					//if UsbDevHasReceivedSETUP()
						//At90UsbDevice._UsbEndpointControl.
						//UsbProcessSetupRequest();
					//
					//PORTD &= (~(1<<PORTD6));
					//break;
					//case 1:
					//UsbDevEP1IntAction();
					//break;
					//case 2:
					//UsbDevEP2IntAction();
					//break;
					//case 3:
					//UsbDevEP3IntAction();
					//break;
					//case 4:
					//UsbDevEP4IntAction();
					//break;
					//case 5:
					//UsbDevEP5IntAction();
					//break;
					//case 6:
					//UsbDevEP6IntAction();
					//break;
					//default:
					//Debug("Error in ISR(USB_COM_vect)\r\n");
				//}
			//}
		//}
//
		//mask = UsbDevGetEndpointIntBits();
	//}
}
