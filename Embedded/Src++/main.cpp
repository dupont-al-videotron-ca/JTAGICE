/*
* AT90USB++.cpp
*
* Created: 05/05/2026 16:18:12
* Author : alain
*/

#ifdef __XC
#include <xc.h>
#else
#include <avr/io.h>
#include <avr/interrupt.h>
#endif

#include "usart_drv.h"
#include "usb_drv.h"
#include "UsbEndpointControl.h"
#include "at90usbDevice.h"
#include "AT90UsbKey.h"
#include "usb_spec.h"
#include "ringBuffer.h"

static int mainLocal();
//static void TestRingBuffer();

UsbEndpointControl *pUsbEndpointControl;

AT90UsbDevice At90UsbDevice;

int main(void)
{
    cli();
    
    At90UsbDevice.InitHardware(false);

    // UsbDevWaitStartupFinished(); // no reason to wait
    sei();
    mainLocal();
    return 0;
}

//static void TestRingBuffer()
//{
    //USB_DeviceRequest req;
    //USB_DeviceRequest reqRead;
    //req.bmRequestType = 0x01;
    //req.bRequest =0x02;
    //req.wIndex = 0;
    //req.wLength = sizeof(USB_DeviceRequest);
    //req.wValue = 0x55;
    //
    //size_t MaxSize = 10;
    //RingBufferByte* testBuffer = new RingBufferByte(MaxSize);
//
    //volatile bool resultIsEmpty = testBuffer->IsEmpty();
    //volatile bool resultIsFull = testBuffer->IsFull();
    //volatile size_t NbByte = testBuffer->GetNbByte();
    //
    //volatile size_t WrittenBytes = testBuffer->Write(&req, req.wLength);
    //volatile bool resultRead = testBuffer->Read(&reqRead, sizeof(USB_DeviceRequest));
    //volatile int resultCmp = memcmp(&req, &reqRead, sizeof(USB_DeviceRequest));   
    //// true
    //resultIsEmpty = testBuffer->IsEmpty();    
    //
    //WrittenBytes  = testBuffer->Write(0x55);
    //uint8_t data = 0;
    //resultRead = testBuffer->Read(&data, (int)1) ;
    //resultIsEmpty = testBuffer->IsEmpty();
//
    //testBuffer->Clear();
    //resultIsEmpty = testBuffer->IsEmpty();    
//
    //data = 0;
    //while(!testBuffer->IsFull())    
    //{
        //testBuffer->Write(data++);
    //}
//
    //while(!testBuffer->IsEmpty())
    //{
        //testBuffer->Read(&data, 1);
    //}
    //
    //while(!testBuffer->IsFull())
    //{
        //testBuffer->Write(data++);
    //}
    //
    //volatile int resultNbByte = testBuffer->GetNbByte();
    //testBuffer->Clear();
//}

static int mainLocal()
{
    int i = 0; // to suppress warning of avr-gcc
    // Led

    while (1)
    {
        while (i++)
        {
            At90UsbDevice.ProcessDeviceInterrupt();
            At90UsbDevice._UsbEndpointControl.ProcessInterrupt();
            
            //for (uint8_t ep = 1; ep <= UsbNumEndpointsAT90USB; ep++)
            //{
            //
            //UsbEndpointBase* pUsbEndpoint = At90UsbDevice.GetUsbEndpoint(ep);
            //if(pUsbEndpoint == NULL)
            //{
            //UsbDevSelectEndpoint(ep);
            //UsbDevDisableAllInt();
            //continue;
            //}
            //else
            //{
            //// TODO:: check virtual call;
            //pUsbEndpoint->ProcessInterrupt();
            //}
            //}
            
            //asm volatile ("nop"); // necessary for avr-gcc 4.x to prevent removing of empty delay loop
        }
        
        BoardPortD2RedToggle();
    }
    
}