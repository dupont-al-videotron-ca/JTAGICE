/* Microchip Technology Inc. and its subsidiaries.  You may use this software 
 * and any derivatives exclusively with Microchip products. 
 * 
 * THIS SOFTWARE IS SUPPLIED BY MICROCHIP "AS IS".  NO WARRANTIES, WHETHER 
 * EXPRESS, IMPLIED OR STATUTORY, APPLY TO THIS SOFTWARE, INCLUDING ANY IMPLIED 
 * WARRANTIES OF NON-INFRINGEMENT, MERCHANTABILITY, AND FITNESS FOR A 
 * PARTICULAR PURPOSE, OR ITS INTERACTION WITH MICROCHIP PRODUCTS, COMBINATION 
 * WITH ANY OTHER PRODUCTS, OR USE IN ANY APPLICATION. 
 *
 * IN NO EVENT WILL MICROCHIP BE LIABLE FOR ANY INDIRECT, SPECIAL, PUNITIVE, 
 * INCIDENTAL OR CONSEQUENTIAL LOSS, DAMAGE, COST OR EXPENSE OF ANY KIND 
 * WHATSOEVER RELATED TO THE SOFTWARE, HOWEVER CAUSED, EVEN IF MICROCHIP HAS 
 * BEEN ADVISED OF THE POSSIBILITY OR THE DAMAGES ARE FORESEEABLE.  TO THE 
 * FULLEST EXTENT ALLOWED BY LAW, MICROCHIP'S TOTAL LIABILITY ON ALL CLAIMS 
 * IN ANY WAY RELATED TO THIS SOFTWARE WILL NOT EXCEED THE AMOUNT OF FEES, IF 
 * ANY, THAT YOU HAVE PAID DIRECTLY TO MICROCHIP FOR THIS SOFTWARE.
 *
 * MICROCHIP PROVIDES THIS SOFTWARE CONDITIONALLY UPON YOUR ACCEPTANCE OF THESE 
 * TERMS. 
 */

/* 
 * File:   
 * Author: Alain Dupont
 * Comments:
 * Revision history: 
 * Initial
 */

// This is a guard condition so that contents of this file are not included
// more than once.  
#ifndef TIMER_2_CTC_H
#define	TIMER_2_CTC_H

//#include <xc.h> // include processor files - each processor file is guarded.  
#include <avr/io.h>
#include <avr/interrupt.h>
#include <stdint.h>
#include <stdbool.h>
#include "macros.h" 

// TODO Insert appropriate #include <>

// TODO Insert C++ class definitions if appropriate

// TODO Insert declarations

// Comment a function and leverage automatic documentation with slash star star
/**
    <p><b>Function prototype:</b></p>
  
    <p><b>Summary:</b></p>

    <p><b>Description:</b></p>

    <p><b>Precondition:</b></p>

    <p><b>Parameters:</b></p>

    <p><b>Returns:</b></p>

    <p><b>Example:</b></p>
    <code>
 
    </code>

    <p><b>Remarks:</b></p>
 */
// TODO Insert declarations or function prototypes (right here) to leverage 
// live documentation
#define ComOutputModeDisconnected   (0)
#define ComOutputModeOne            (1)
#define ComOutputModeTwo            (2)
#define ComOutputModeThree          (3)

#define WaveformModeMaskLSB         (0x03)
#define WaveformModeMaskMSB         (0x04)
#define WaveformModeClearLSB()      (TCCR2A &= ~(WaveformModeMaskLSB << WGM20))
#define WaveformModeClearMSB()      (TCCR2B &= ~(0x1 << WGM22))

#define WaveformModeNormal (0)
#define WaveformModePWMPhaseCorrect (1)
#define WaveformModeCTC (2)
#define WaveformModeFastPWM (3)
#define WaveformModeReserve1 (4)
#define WaveformModePWMPhaseCorrectOCRA (5)
#define WaveformModeReserved2 (6)
#define WaveformModeFastPWMOCRA (7)

    
#define Timer2PreScaleOff   0
#define Timer2PreScale1     1
#define Timer2PreScale8     2
#define Timer2PreScale32    3
#define Timer2PreScale64    4
#define Timer2PreScale128   5
#define Timer2PreScale256   6
#define Timer2PreScale1024  7

#define Timer2SetPresacle(p) (TCCR2B |= (p << CS20))

#ifdef	__cplusplus
extern "C" {
#endif /* __cplusplus */

extern volatile bool FatalError;  
extern uint32_t GetTimerTick();
extern uint32_t GetElapseTime(uint32_t oldTick);

void Timer2CTC_Initialize();
void Timer2CTC_Reset();
void Timer2CTC_StartTick(bool enableIntr);
void Timer2CTC_StartB(uint8_t ocr, bool enableIntr);

    // TODO If C++ is being used, regular C code needs function names to have C 
    // linkage so the functions can be used by the c code. 

#ifdef	__cplusplus
}
#endif /* __cplusplus */

#endif	/* TIMER_2_CTC_H */

