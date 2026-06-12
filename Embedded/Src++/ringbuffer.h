// AT90USB/ringbuffer.h
// Simple Ring-Buffer (FIFO) for Elements of type Q

#ifndef _RING_BUFFER_H_
#define _RING_BUFFER_H_

#ifdef __XC
#else
#endif

#include <stdint.h>
#include <string.h>
#include "defines.h"

class RingBufferByte
{
    private:
    
    size_t _bufferSize;
    uint8_t* _buffer;
    uint8_t* _head;
    uint8_t* _tail;
    size_t _nbByte;
    
    void IncHead();
    void IncTail();
    uint8_t* GetBufferEnd() { return _buffer+_bufferSize;}
    
    
    public:
    
    RingBufferByte(size_t size);
    
    size_t GetNbByte() { return _nbByte;}
    
    void Clear();
    bool IsFull();
    bool IsEmpty();
    size_t Write(void* src, size_t length);
    size_t Write(uint8_t* src, size_t length);
    size_t Write(uint8_t src);
    bool Read(uint8_t* dst, size_t length);
    bool Read(void* dst, size_t length);
};

#endif

