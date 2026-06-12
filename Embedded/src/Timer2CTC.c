/*
 * File:   Timer2CTC.c
 * Author: alain
 *
 * Created on June 3, 2026, 11:57 AM
 */


#include "Timer2CTC.h"
#include "AT90UsbKey.h"

uint32_t volatile Timertick; // tick 1 ms
volatile bool FatalError; 

static void WaveformModeClear()
{
    WaveformModeClearLSB();
    WaveformModeClearMSB();
    
}


uint32_t GetTimerTick()
{
    uint32_t retval;
    bool intFlg =  BitIsSet(TIMSK2, OCIE2A);
    ClearBit(TIMSK2, OCIE2A);
    retval = Timertick;
    if(intFlg)
    {
        SetBit(TIMSK2, OCIE2A);        
    }
    
    return retval;
}

uint32_t GetElapseTime(uint32_t oldTick)
{
    return GetTimerTick() - oldTick;
}

static void Timer2SetWaveFormeMode(uint8_t mode) 
{
    
    WaveformModeClear();
    TCCR2A |= (WaveformModeMaskLSB & mode) << WGM20;
    
    if((mode & WaveformModeMaskMSB) == 0)
    {
        ClearBit(TCCR2B, WGM22);
    }
    else
    {
        SetBit(TCCR2B, WGM22);        
    }
    
}

void Timer2CTC_Reset()
{
    TCCR2B = 0;
    TCCR2A = 0;
    TCNT2 = 0;
    OCR2A = 0;
    OCR2B = 0;
    ASSR = 0;
    TIMSK2 = 0;
    TIFR2 = 0;    
    FatalError = false;
}


static void Timer2OutputADisconnected() 
{
    TCCR2A = (TCCR2A & (~ComOutputModeDisconnected << COM2A0)) | (TCCR2A | (ComOutputModeDisconnected << COM2A0));  
}
static void Timer2OutputAToggle()
{
    SetBit(DDRB, DDB4); // output
    TCCR2A = (TCCR2A & (~ComOutputModeOne << COM2A0)) | (TCCR2A | (ComOutputModeOne << COM2A0));  
}

static void Timer2OutputAClear()
{
    SetBit(DDRB, DDB4); // output
    TCCR2A = (TCCR2A & (~ComOutputModeTwo << COM2A0)) | (TCCR2A | (ComOutputModeTwo << COM2A0));  
}

static void Timer2OutputASet()
{
    SetBit(DDRB, DDB4); // output
    TCCR2A = (TCCR2A & (~ComOutputModeThree << COM2A0)) | (TCCR2A | (ComOutputModeThree << COM2A0));  
}

static void Timer2OutputBDisconnected() 
{
    TCCR2B = (TCCR2B & (~ComOutputModeDisconnected << COM2B0)) | (TCCR2B | (ComOutputModeDisconnected << COM2B0));
}
static void Timer2OutputBToggle()
{
    SetBit(DDRD, DDD1); // output
    TCCR2B = (TCCR2B & (~ComOutputModeOne << COM2B0)) | (TCCR2B | (ComOutputModeOne << COM2B0));
}

static void Timer2OutputBClear()
{
    SetBit(DDRD, DDD1); // output
    TCCR2B = (TCCR2B & (~ComOutputModeTwo << COM2B0)) | (TCCR2B | (ComOutputModeTwo << COM2B0));
}

static void Timer2OutputBSet()
{
    SetBit(DDRD, DDD1); // output
    TCCR2B = (TCCR2B & (~ComOutputModeThree << COM2B0)) | (TCCR2B | (ComOutputModeThree << COM2B0));
}

void Timer2CTC_Initialize()
{
    Timer2CTC_Reset();
    Timer2OutputADisconnected();
    Timer2OutputBDisconnected();
    Timer2SetWaveFormeMode(WaveformModeCTC);
    Timer2SetPresacle(Timer2PreScale32);
}

void Timer2CTC_StartTick(bool enableIntr)
{
    ClearBit(TIMSK2, OCIE2A);
    OCR2A = 240; // adjusted with scope
    
    if(enableIntr)
    {
        SetBit(TIMSK2, OCIE2A);
    }
}

void Timer2CTC_StartB(uint8_t ocr, bool enableIntr)
{
    ClearBit(TIMSK2, OCIE2B);
    OCR2B = ocr;
    
    if(enableIntr)
    {
        SetBit(TIMSK2, OCIE2B);
    }    
}

// timer tick interrupt
ISR(TIMER2_COMPA_vect)
{
    // clear intr source
    SetBit(TIFR2, OCF2A);
    Timertick++;
    
    if(FatalError)
    {
        if((Timertick % 200) == 0)
        {
            BoardPortD2RedToggle();
            BoardPortD5RedToggle();
        }
    }
    
}

ISR(TIMER2_COMPB_vect)
{
    // clear intr source
    SetBit(TIFR2, OCF2B);   
}