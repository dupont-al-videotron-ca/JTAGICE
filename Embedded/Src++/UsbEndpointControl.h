#pragma once

#include <avr/interrupt.h>
#include <stdint.h>
#include "usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h"
#include "usb_requests.h"
#include "UsbEndpointBase.h"
#include "RingBuffer.h"

#ifndef USBENDPOINTCONTROL
#define USBENDPOINTCONTROL



class UsbEndpointControl : public UsbEndpointBase
{
    protected:
    enum StateEnum
    {
        ST_RESET,
        ST_IDLE,
        ST_WAIT_SETUP_REQ,
        ST_WAIT_SETUP_INDATA,
        ST_WAIT_SETUP_OUTDATA,
        ST_WAIT_INSTATUS,
        ST_WAIT_OUTSTATUS,
        ST_WAIT_STALLED,        
    };
    
    private:
    uint16_t _nakInCnt;
    uint16_t _nakOutCnt;
    uint16_t _stallSentCnt;
    
    USB_DeviceRequest _setupRequest;
    
    RingBufferByte* _txBuffer;
    
    StateEnum _state;
    
    void GoWaitSetupReq()
    {
        _state = ST_WAIT_SETUP_REQ;
    }
    
    void GoWaitInData()
    {
        _state = ST_WAIT_SETUP_INDATA;
    }
    
    void GoReset() {_state = ST_RESET;}
    void GoIdle() {_state = ST_IDLE;}
    void GoWaitOutData() {_state = ST_WAIT_SETUP_OUTDATA;}
    void GoWaitInStatus() {_state = ST_WAIT_INSTATUS;}
    void GoWaitOutStatus() {_state = ST_WAIT_OUTSTATUS;}
    void GoWaitStalled() {_state = ST_WAIT_STALLED;}
    
    void RequestStallHandshake();
    void ProcessStandardRequest();
    
    bool WriteTxBuffer(void *d, size_t requested);  
    bool WriteTxBuffer(uint8_t *d, size_t requested);
    bool WriteTxBuffer(uint8_t d);
    
    bool FillFifoIn();
    bool SendDescriptorsNeverCall(uint8_t cdi);

// public:    
    public:
    
    uint8_t RemoteWakeupActive;

    UsbEndpointControl();

    bool InitializeEndpoint(uint16_t size, uint8_t banks);
    
    virtual void ProcessInterrupt();
    void HandleInterrupt();
   
    
    void OnRxSetupInterrupt();
    void OnVBusChange(bool active);

};

#endif