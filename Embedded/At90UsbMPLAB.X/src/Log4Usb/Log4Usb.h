#ifndef LOG_FOR_USB_H
#define	LOG_FOR_USB_H


#include <stdint.h>
#include <stdbool.h>
#include "usb_spec.h"
#include "macros.h"
#include "defines.h"

// logger
#define Log4UsbLevelNone 0
#define Log4UsbLevelFatal 1
#define Log4UsbLevelError 2
#define Log4UsbLevelWarning 3
#define Log4UsbLevelInfo 4
#define Log4UsbLevelDebug 5
#define Log4UsbLevelAll 6
#define Log4UsbMaxMessage (128-sizeof(UsbLoggerAppender_t))

#define Log4UsbLogDisableRequest 0
#define Log4UsbLogEnableRequest 1
#define Log4UsbLogEnableLastRequest 1

#define Log4UsbFrameVersion 1

// configuration

#ifdef	__cplusplus
extern "C" {
#endif /* __cplusplus */

typedef struct
{
    uint16_t length;    // length of the data in bytes
    uint8_t  Reseved;
    uint8_t version;    // version of the Usb log appender with message
    uint8_t level;
    uint32_t tickms;
    //uint8_t loggerName[1]; 
    //uint8_t fileName[1]; 
    //uint8_t message[1]; 

} UsbLoggingEventData_t;

bool ApplyUsbLoggerCfg(USB_DeviceRequest* request);
bool SendLog(uint8_t level, char* message, char* fileName);

#define Log_Fatal(msg) SendLog(Log4UsbLevelFatal,msg, __FILE__)
#define Log_Error(msg) SendLog(Log4UsbLevelError,msg, __FILE__)
#define Log_Warn(msg) SendLog(Log4UsbLevelWarning,msg, __FILE__)
#define Log_Info(msg) SendLog(Log4UsbLevelInfo,msg, __FILE__)
#define Log_Debug(msg) SendLog(Log4UsbLevelDebug,msg, __FILE__)


#ifdef	__cplusplus
}
#endif /* __cplusplus */

#endif

