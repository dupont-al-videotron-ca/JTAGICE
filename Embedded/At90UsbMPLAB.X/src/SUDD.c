// AT90USB/SUDD.c 
// Simple Usb Data-acquisition Device 
// S. Salewski 10-SEP-2007
// Start minicom with 8N1 configuration to see debug messages
// The real work happens inside of interrupt-service-routines

#include <avr/io.h>
#include <avr/interrupt.h>
#include "usart_drv.h"
#include "usb_drv.h"
#include "AT90usbkey.h"
#include "Timer2CTC.h"
#include "usb_requests.h"

uint8_t USB_COM_mask;

static void blinkforever(void);

int main(void)
{
  cli();
  CLKPR = (1<<7);
  CLKPR = 0; // clock pre-scaler == 0, so we have 16 MHz mpu frequency with our 16 MHz crystal

  BoardPortDLedInit();
  BoardPortDLedsOff();
  SetBit(DDRB, DDB4); // output
  
  Timer2CTC_Initialize();
  Timer2CTC_StartTick(true);  
  
  //USART_Init();
  //USART_WriteString("\r\n----------------------------------------------------------\r\n");
  UsbDevLaunchDevice(false);
  // UsbDevWaitStartupFinished(); // no reason to wait
  sei();
  blinkforever();
  return 0;
}

// a LED connected to PORTA0 will toggle to indicate the unused processing power
static void blinkforever(void)
{
  while (1)
  {
    
    UsbDevSelectEndpoint(0);
    UEIENX = 0;

    if UsbDevHasReceivedSETUP()
    {      
        BoardPortD2GreenOn();
        UsbProcessSetupRequest();
        BoardPortD2GreenOff();
        continue;
    }

    if (UsbDevNAK_ResponseSendToOutRequest())
    {
        UsbDevClearNAK_ResponseOutBit();
        continue;
    }

    if (UsbDevSTALLHandshakeSend())
    {
        UsbDevClearSTALLHandshakeSend();
        continue;
    }   
  }
}
