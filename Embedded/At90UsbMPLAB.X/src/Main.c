// AT90USB/SUDD.c 
// Simple Usb Data-acquisition Device 
// S. Salewski 10-SEP-2007
// Start minicom with 8N1 configuration to see debug messages
// The real work happens inside of interrupt-service-routines

#include <avr/io.h>
#include <avr/interrupt.h>
#include <string.h>
#include "usart_drv.h"
#include "usb_drv.h"
#include "AT90usbkey.h"
#include "log4usb.h"
#include "Timer2CTC.h"
#include "usb_ControlEndpoint.h"


extern void Sleep(uint32_t ms);

static void MainLoop(void);
static void ProcessUsbControl();
static void ProcessUnitTest();

const char DeviceName[] = "AT90USB";
uint8_t AppState;

int main(void) {
    AppState = State_WaitInit;
    cli();
    CLKPR = (1 << 7);
    CLKPR = 0; // clock pre-scaler == 0, so we have 16 MHz mpu frequency with our 16 MHz crystal

    BoardPortDLedInit();
    BoardPortDLedsOff();
    SetBit(DDRB, DDB4); // output

    Timer2CTC_Initialize();
    Timer2CTC_StartTick(true);

    //USART_Init();
    //USART_WriteString("\r\n----------------------------------------------------------\r\n");
    UsbDrv_DeviceLaunch(false);
    // UsbDevWaitStartupFinished(); // no reason to wait
    AppState = State_WaitUsb;
    sei();
    MainLoop();
    return 0;
}

// a LED connected to PORTA0 will toggle to indicate the unused processing power

static void MainLoop(void) {
    Log_Debug("MainLoop");
    while (!IsUsbDeveiceConfigured()) {
        Sleep(10);
    }

    while (1) 
    {
        ProcessUnitTest();
    }
}

static void ProcessUsbControl() 
{
    if (UsbDevHasReceivedSETUP()) 
    {
        BoardPortD2GreenOn();
        //UsbProcessSetupRequest();
        BoardPortD2GreenOff();
    }
    if (UsbDevNAK_ResponseSendToOutRequest()) {
        UsbDevClearNAK_ResponseOutBit();
    }

    if (UsbDevSTALLHandshakeSend()) 
    {
        UsbDevClearSTALLHandshakeSend();
    }
}

static void ProcessUnitTest()
{
    Log_Fatal("Log_Fatal");
    Sleep(1);
    //Log_Error("Log_Error");
    //Log_Warn("Log_Warn");
    //Log_Info("Log_Info");
    //Log_Debug("Log_Debug");
 }
