
#include <string.h>
#include "Log4Usb.h"
#include "usb_inEndpoint.h"
#include "usb_api.h"


extern const char* DeviceName;
extern uint32_t GetTimerTick();

#ifdef DEBUG
static uint8_t LogLevel = Log4UsbLevelFatal;
#else
static uint8_t LogLevel = Log4UsbLevelNone;
#endif

/// <summary>
/// Applies the USB logger configuration based on the provided device request.
/// </summary>
bool ApplyUsbLoggerCfg_Intr(USB_DeviceRequest* request)
{
    bool retval = false;
    
    // is request valid?
    if (UsbIsVendorRequest(request->bmRequestType) && 
        (request->bRequest == AppLoggerId) && 
        (LSB(request->wValue) <= Log4UsbLogEnableLastRequest) &&
        (LSB(request->wIndex) <= Log4UsbLevelAll))
    {
        // yes 
        uint8_t prevEp =  UsbDevGetEndpoint();
        UsbDevSelectEndpoint(Log4UsbEndpointNumber);

        if(LSB(request->wValue) == Log4UsbLogDisableRequest)
        {
            // valid request, apply configuration
            LogLevel = Log4UsbLevelNone;
            retval = true;
        }
        else if (LSB(request->wValue) == Log4UsbLogEnableRequest)
        {
            LogLevel = LSB(request->wIndex);
            if(!UsbDevGetNumberOfBusyBanks() == 0)
            {
                Usb_InEndpointAbort(Log4UsbEndpointNumber);
            }
            
            retval = true;
        }        
        
        UsbDevSelectEndpoint(prevEp);
    }
    
    // invalid request
    return retval;
}

bool SendLog(uint8_t level, char *message, char* fileName)
{
    if(!IsUsbDeveiceConfigured() || level == Log4UsbLevelNone || level > LogLevel)
    {
        return false;
    }
    
    UsbLoggingEventData_t appender;
    memset(&appender, 0, sizeof(appender));

    appender.tickms = GetTimerTick();
    uint16_t msgLength = strlen(message)+1;
    uint16_t fileNameLength = strlen(fileName)+1;
    uint16_t deviceNsmeLength = strlen(DeviceName)+1;
       
    
    uint16_t strLength =  msgLength + fileNameLength + deviceNsmeLength;
    appender.length = sizeof(UsbLoggingEventData_t) + strLength;
    appender.version = Log4UsbFrameVersion;
    appender.level = level;

    Usb_InEndpointWriteFifo(Log4UsbEndpointNumber, &appender, sizeof(UsbLoggingEventData_t));    
    Usb_InEndpointWriteFifo(Log4UsbEndpointNumber, DeviceName, deviceNsmeLength);
    Usb_InEndpointWriteFifo(Log4UsbEndpointNumber, fileName, fileNameLength);
    Usb_InEndpointWriteFifoAndFlush(Log4UsbEndpointNumber, message, msgLength);

    return true;
}
