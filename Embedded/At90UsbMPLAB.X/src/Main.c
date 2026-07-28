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
static void ProcessUnitTest();

const char *DeviceName = "AT90USB";
uint8_t AppState;

int main(void) {
    AppState = State_WaitInit;
    cli();
    CLKPR = (1 << 7);
    CLKPR = 0; // clock pre-scaler == 0, so we have 16 MHz mpu frequency with our 16 MHz crystal

    BoardPortDLedInit();
    BoardPortDLedsOff();
    SetBit(DDRB, DDB4); // output
    SetBit(DDRB, DDB6); // output

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
    while (!IsUsbDeveiceConfigured()) 
    {
        Sleep(10);
    }
    
    Log_Info("MainLoop started");
    while(1)
        ProcessUnitTest();
}

static void ProcessUnitTest()
{
    
    //SendLog(Log4UsbLevelFatal,"a1", "b1");

    Sleep(50);
    Log_Fatal("Log_Fatal");
#if 0
    Log_Fatal("Log_Fatal");
    Sleep(500);
    Log_Error("Log_Error");
    Sleep(500);
    Log_Warn("Log_Warn");
    Sleep(500);
    Log_Info("Log_Info");
    Sleep(500);
    Log_Debug("Log_Debug");
#endif    
 }
