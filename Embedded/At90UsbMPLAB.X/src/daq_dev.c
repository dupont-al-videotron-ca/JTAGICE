// AT90USB/daq_dev.c
// This file implements basic functionality of a simple data-acquisition device
// S. Salewski 23-MAR-2007

#include <avr/io.h>
#include <avr/interrupt.h>
#include "macros.h"
#include "usb_drv.h"
#include "usart_debug.h"
#include "ringbuffer.h"
#include "daq_dev.h"
#include "com_def.h"

uint8_t DAQ_Result;
static uint16_t SamplesToRead;

void
ProcessUserCommand(uint8_t command, uint16_t par1, uint16_t par2)
{
  uint8_t prescalerADC;
  uint8_t prescalerT0;
  uint8_t counterT0Max;
  switch (command)
  {
    case UC_ADC_Read:
      switch ((uint8_t) par1) 
      {
        case ADC20us:
          prescalerADC = ((1<<ADPS2)); // mpu clock / 16
          prescalerT0 = (1<<CS01); // == 8
          counterT0Max = 40;
          break;
        case ADC50us:
          prescalerADC = ((1<<ADPS2) | (1<<ADPS0)); // mpu clock / 32
          prescalerT0 = (1<<CS01); // == 8
          counterT0Max = 100;
          break;
        case ADC100us:
          prescalerADC =  ((1<<ADPS2) | (1<<ADPS1)); // mpu clock / 64
          prescalerT0 = (1<<CS01); // == 8
          counterT0Max = 200;
          break;
        case ADC200us:
          prescalerADC =  ((1<<ADPS2) | (1<<ADPS1)  | (1<<ADPS0)); // mpu clock / 128
          prescalerT0 = ((1<<CS01) | (1<<CS00)); // == 64
          counterT0Max = 50; // 400 / 8
          break;
        case ADC500us:
          prescalerADC =  ((1<<ADPS2) | (1<<ADPS1)  | (1<<ADPS0)); // mpu clock / 128
          prescalerT0 = ((1<<CS01) | (1<<CS00)); // == 64
          counterT0Max = 125;
          break;
        case ADC1ms:
          prescalerADC =  ((1<<ADPS2) | (1<<ADPS1)  | (1<<ADPS0)); // mpu clock / 128
          prescalerT0 = ((1<<CS01) | (1<<CS00)); // == 64
          counterT0Max = 250;
          break;
        default:
          Debug("DaqDev: Unsupported sampling rate!\r\n");
          return;
      }
      ADMUX = (1<<REFS1) | (1<<REFS0); // ADC0 single ended input, internal 2.56V reference with external capacitor on AREF pin
      ADCSRA = prescalerADC | (1<<ADEN) | (1<<ADSC); // set prescaler, enable ADC and start conversion
      TCCR0A = (1<<WGM01); // set timer registers: clear on compare match
      TCCR0B = prescalerT0; // set prescaler
      OCR0A = counterT0Max; // set output compare register
      SamplesToRead = par2;
      RB_Init();
      DAQ_Result = UsbDevStatusOK;
      prescalerT0 = 20;  // do a few conversions to warm up
      while (prescalerT0-- > 0)
      {
        while (ADCSRA & (1<<ADSC));
        ADCSRA |= (1<<ADSC);
      }
      TCNT0 = counterT0Max + 1; // set start value behind compare match -- we need some time to finish this USB-ISR
      TIMSK0 = (1<<OCIE0A); // activate compare match interrupt
      TIFR0 |= (1<<OCF0A); // clear compare match flag -- we will wait for next compare match
      break;
    default:
      Debug("DaqDev: Unsupported user command!\r\n");
  }
}

ISR(TIMER0_COMPA_vect)
{    
  uint16_t w;
  //static uint8_t cnt;
  if (ADCSRA & (1<<ADSC)) // conversion finished?
    DAQ_Result = UsbDevStatusDAQ_ConversionNotFinished;
  w = ADCW;
  ADCSRA |= (1<<ADSC); // start next conversion as soon as possible
  if (SamplesToRead > 0)
  {
    if (RB_IsFull())
    {
      DAQ_Result = UsbDevStatusDAQ_BufferOverflow;
    }
    else
    {
      if (DAQ_Result == UsbDevStatusOK)
        RB_Write(w);
      else
        RB_Write(0xFFFF);
      SamplesToRead--;
    }
  }
  UsbDevSelectEndpoint(2);
  if (UsbDevIsFifoEmpty())
  {
    uint8_t i;
    if ((SamplesToRead == 0) && (RB_Entries == 0))
    {
      TIMSK0 = 0;
      i = 64;
    }
    else
      i = UsbDevGetByteCountLow();
    switch (i)
    {
      default:
        w = RB_Read();
        //w = cnt; dump counter to FIFO for testing
        UsbDevWriteByte(LSB(w));
        UsbDevWriteByte(MSB(w));
      case 62:
        if (RB_Entries > 0)
        {
          w = RB_Read();
          UsbDevWriteByte(LSB(w));
          UsbDevWriteByte(MSB(w));
        }
        break;
      case 64:
        UsbDevClearTransmitterReady();
        UsbDevSendInData();
    }
  }
  if (TIFR0 & (1<<OCF0A)) // is interrupt flag already set again?
  {
    DAQ_Result = UsbDevStatusDAQ_TimerOverflow;
  }
  // cnt = TCNT0; if there are too many instructions in this ISR, we may get timer-counter overflow 
}

