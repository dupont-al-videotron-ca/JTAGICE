// AT90USB/com_def.h
// Common defines for firmware and PC program
// S. Salewski, 23-MAR-2007

#ifndef _COM_DEF_H_
#define _COM_DEF_H_

#define MyUSB_VendorID 0x03eb // Atmel code
#define MyUSB_ProductID 0x0001 // arbitrary value
#define USB_VendorRequestCode (1<<6)

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

#define MaxSamples 65535

// Status codes
#define UsbDevStatusOK 0
#define UsbDevStatusDAQ_BufferOverflow 1
#define UsbDevStatusDAQ_TimerOverflow 2
#define UsbDevStatusDAQ_ConversionNotFinished 3

#endif
