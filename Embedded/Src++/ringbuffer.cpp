// AT90USB/ringbuffer.c
// Simple Ring-Buffer (FIFO) for Elements of type Q

/*
t-> o
o <-w
x
x <-r
b-> x
*/



#include <stdint.h>
#include "ringbuffer.h"


RingBufferByte::RingBufferByte(size_t size)
{
    _buffer = new uint8_t[size];
    _bufferSize = size;
    Clear();
}

void RingBufferByte::IncHead()
{
    _nbByte++;
    _head++;
    if(_head == GetBufferEnd())
    {
        _head = _buffer;
    }
}

void RingBufferByte::IncTail()
{
    _nbByte--;
    _tail++;
    if(_tail == GetBufferEnd())
    {
        _tail  = _buffer;
    }
}

void RingBufferByte::Clear()
{
    _head = _buffer;
    _tail = _buffer;
    _nbByte = 0;
}

bool RingBufferByte::IsFull()
{
    
    if (IsEmpty())
    {
        return false;
    }
    else
    {
        return _nbByte >= (_bufferSize-1);        
    }       
}

bool RingBufferByte::IsEmpty()
{
    return _head == _tail;
}

size_t RingBufferByte::Write(uint8_t src)
{
    return Write(&src, (size_t) 1);
}

size_t RingBufferByte::Write(void* src, size_t length)
{
    return Write(((uint8_t*) src), length);
}

size_t RingBufferByte::Write(uint8_t* src, size_t length)
{
    size_t writen = 0;
    
    while(length-- && !IsFull())
    {
        *_head = *src++;
        writen ++;
        IncHead();
    }
    
    return writen;
}


bool RingBufferByte::Read(uint8_t* dst, size_t length)
{
    size_t read = 0;
    size_t l = length;
    
    while(length-- && !IsEmpty())
    {
        *dst++ = *_tail;
        read++;
        IncTail();
    }
    
    return read == l;
    
}

bool RingBufferByte::Read(void* dst, size_t length)
{
    return Read(((uint8_t*) dst), length);
}

