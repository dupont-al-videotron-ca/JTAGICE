// AT90USB/daq_dev.h
// Very simple data-acquisition device
// S. Salewski 19-MAR-2007

#ifndef _DAQ_DEV_H_
#define _DAQ_DEV_H_

#include <stdint.h>

// user commands
#define UC_ADC_Read	1

// sampling time
#define ADC20us		1
#define ADC50us		2
#define ADC100us	3
#define ADC200us	4
#define ADC500us	5
#define ADC1ms		6

extern uint8_t DAQ_Result;

void ProcessUserCommand(uint8_t command, uint16_t par1, uint16_t par2);

#endif
