/*
 * AT90UsbKey.h
 *
 * Created: 05/12/2026 15:01:12
 *  Author: alain
 */ 


#ifndef AT90USBKEY_H_
#define AT90USBKEY_H_

#include <avr\io.h>
#include <stdint.h>
#include <stdbool.h>
#include "defines.h"

// D2 D5
#define BoardPortDLedInit() 	DDRD |= (1 << DDD4) | (1 << DDD5) | (1 << DDD6) | (1 << DDD7);
#define BoardPortDLedsOff() 	PORTD &= ~((1 << DDD4) | (1 << DDD5) | (1 << DDD6) | (1 << DDD7));
#define BoardPortDLedsOn()      PORTD |= ((1 << DDD4) | (1 << DDD5) | (1 << DDD6) | (1 << DDD7));

//D2
#define BoardPortD2RedToggle()  ToggleBit(PORTD , DDD4)
#define BoardPortD2RedOn()		SetBit(PORTD , DDD4)
#define BoardPortD2RedOff()		ClearBit(PORTD , DDD4) 

#define BoardPortD2GreenOn()	SetBit(PORTD , DDD5)
#define BoardPortD2GreenOff()	ClearBit(PORTD , DDD5)

#define BoardPortD2AmberOn()	(PORTD  |=  (1<<DDD4) | (1<<DDD5))
#define BoardPortD2AmberOff()	(PORTD &= ~((1<<DDD4) | (1<<DDD5)))

//D5
#define BoardPortD5RedToggle() ToggleBit(PORTD , DDD7)
#define BoardPortD5RedOn()		SetBit(PORTD , DDD7)
#define BoardPortD5RedOff()		ClearBit(PORTD , DDD7)

#define BoardPortD5GreenToggle() ToggleBit(PORTD , DDD6)
#define BoardPortD5GreenOn()	SetBit(PORTD , DDD6)
#define BoardPortD5GreenOff()	ClearBit(PORTD , DDD6)

#define BoardPortD5AmberOn()	(PORTD  |=  (1<<DDD7) | (1<<DDD6))
#define BoardPortD5AmberOff()	(PORTD &= ~((1<<DDD7) | (1<<DDD6)))

#endif /* AT90USBKEY_H_ */