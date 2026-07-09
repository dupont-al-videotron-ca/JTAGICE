#ifndef _MACROS_H_
#define _MACROS_H_

#define MSB(w) ((char*) &w)[1]
#define LSB(w) ((char*) &w)[0]

// A few macros for bit fiddling
#define ToggleBit(adr, bit)			(adr =  adr ^ (1<<bit))
#define SetBit(adr, bit)			(adr |=  (1<<bit))
#define ClearBit(adr, bit)			(adr &= ~(1<<bit))
#define BitIsSet(adr, bit)			(adr & (1<<bit))
#define BitIsClear(adr, bit)		(!(adr & (1<<bit)))

#endif
