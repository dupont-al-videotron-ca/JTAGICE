
#include <string.h>
#include "Log4Usb.h"
#include "usb_inEndpoint.h"
#include "usb_api.h"


extern const char* DeviceName;
extern uint32_t GetTimerTick();

#ifdef DEBUG
static uint8_t LogLevel = Log4UsbLevelAll;
#else
static uint8_t LogLevel = Log4UsbLevelNone;
#endif

/// <summary>
/// Applies the USB logger configuration based on the provided device request.
/// </summary>
bool ApplyUsbLoggerCfg(USB_DeviceRequest* request)
{
    // is request valid?
    if (UsbIsVendorRequest(request->bmRequestType) && 
        (request->bRequest == AppLoggerId) && 
        (LSB(request->wValue) <= Log4UsbLogEnableLastRequest) &&
        (LSB(request->wIndex) <= Log4UsbLevelAll))
    {
        // yes 
        if(LSB(request->wValue) == Log4UsbLogDisableRequest)
        {
            // valid request, apply configuration
            LogLevel = Log4UsbLevelNone;
            return true;
        }
        else if (LSB(request->wValue) == Log4UsbLogEnableRequest)
        {
            LogLevel = LSB(request->wIndex);
            return true;
        }        
    }
    // invalid request
    return false;
}

bool SendLog(uint8_t level, const char *message, const char* fileName)
{
    if(!IsUsbDeveiceConfigured() || level == Log4UsbLevelNone || level > LogLevel)
    {
        return false;
    }
    UsbLoggingEventData_t appender;
    memset(&appender, 0, sizeof(appender));

    appender.tickms = GetTimerTick();
    uint16_t strLength = strlen(message)+1;
    appender.length = sizeof(UsbLoggingEventData_t) + strLength;
    appender.version = Log4UsbFrameVersion;
    appender.level = level;

    Usb_InEndpointWriteFifoAndFlush(Log4UsbEndpointNumber, &appender, sizeof(UsbLoggingEventData_t));
    
    return true;
    Usb_InEndpointWriteFifo(Log4UsbEndpointNumber, &appender, sizeof(UsbLoggingEventData_t));
    Usb_InEndpointWriteFifo(Log4UsbEndpointNumber, (void*)DeviceName, strlen(DeviceName) + 1);
    Usb_InEndpointWriteFifo(Log4UsbEndpointNumber, (void*)fileName, strlen(fileName) + 1);
    Usb_InEndpointWriteFifoAndFlush(Log4UsbEndpointNumber, (void*)message, strlen(message) + 1);

    return true;
}
