#pragma once

#ifndef _AT90USBDEVICE_H_
#define _AT90USBDEVICE_H_

#include <stdint.h>
#include "usb_drv.h"
#include "UsbEndpointControl.h"
#include "UsbEndpointIn.h"
#include "UsbEndpointOut.h"

// Endpoint 1, used for transferring status information:
// Bulk IN, 8 byte FIFO
// Filled by "NAK-IN-WAS-SEND" ISR
#define EP1_FIFO_Size 8

// Endpoint 2, used for transferring DAQ data:
// Bulk IN, 64 byte dual bank FIFO
// Filled from within timer ISR
#define EP2_FIFO_Size 64

// Endpoint 3, used to set digital port B
// Bulk OUT, 8 byte FIFO
// Read by "OUT-FIFO-IS-FILLED" ISR
#define EP3_FIFO_Size 8

#define MyUSB_VendorID 0x03eb // Atmel code
#define MyUSB_ProductID 0x0001 // arbitrary value
#define USB_VendorRequestCode (1<<6)


struct DeviceInterruptStatus
{
    union
    {
        struct
        {
            // LSB
            uint8_t SUSP_flg : 1;
            uint8_t reserved : 1;
            uint8_t SOF_flg : 1;
            uint8_t EORST_flg : 1;
            uint8_t WAKEUP_flg : 1;
            uint8_t EORSM_flg : 1;
            uint8_t UPRSM_flg : 1;
            uint8_t reserved2 : 1;
        } Flags;
        uint8_t Data;
    } U_UDINT;
    
    union
    {
        struct
        {
            uint8_t VBUS_flg: 1;
            uint8_t ID_flg : 1;
            uint8_t reserved : 6;
        } Flags;
        uint8_t Data;
        
    }U_USBINT;
};

class AT90UsbDevice;
extern AT90UsbDevice At90UsbDevice;

class AT90UsbDevice
{

   
    protected:

    uint8_t _deviceStatus;
    uint8_t _endOfResetCnt;
    uint8_t _crcErrorCnt;
    
    DeviceInterruptStatus _interruptDeviceStatus;
    UsbEndpointBase* _UsbEndpoints[UsbNumEndpointsAT90USB];


    public:


    /// <summary>
    ///
    /// </summary>
    AT90UsbDevice();

    UsbEndpointControl _UsbEndpointControl;
    
    // Config.
    uint8_t UsbDevConfValue ;
    uint8_t AltSettingOfInterface[USB_MaxInterfaces];
    uint8_t RemoteWakeupActive;
    bool IsSuspendResumingActive;
    
    bool DoPowerOn();
    void DoPowerOff();

    void HandleDeviceInterrupt();
    void ProcessDeviceInterrupt();

    void HandleEndpointInterrupt();
    void ProcessEndpointInterrupt();

    void InitHardware(bool lowSpeed);

    bool StartPLL(void);
    void StopPLL(void);

    void FatalError();
    
    UsbEndpointBase* GetUsbEndpoint(uint8_t index);
    
    void FillDeviceDescriptor(USB_DeviceDescriptor* d);
    bool FillConfigurationDescriptor(USB_ConfigurationDescriptor* c, uint8_t confIndex);
    bool FillInterfaceDescriptor(USB_InterfaceDescriptor* i, uint8_t confIndex, uint8_t intIndex, uint8_t altSetting);
    bool FillEndpointDescriptor(USB_EndpointDescriptor* e, uint8_t confIndex, uint8_t intIndex, uint8_t altSetting, uint8_t endIndex);
    void FillStringDescriptor(char s[], uint8_t index);

    void SetUnconfiguredState(void);
    bool SetConfiguration(uint8_t c);
    bool SetInterface(uint8_t conf, uint8_t inf, uint8_t as);
    
    void DisableAndFreeEndpoint(uint8_t i);
};

#endif