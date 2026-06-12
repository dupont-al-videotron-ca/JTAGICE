/*
 * new.cpp
 *
 * Created: 05/05/2026 21:24:47
 *  Author: alain
 */ 
#include <stdlib.h>
#include <string.h>

static void* _allocNew(size_t size);

void* operator new(size_t size) 
{
    return _allocNew(size);
}

void* operator new[](size_t size) 
{
    return _allocNew(size);
}

void operator delete(void* ptr) 
{
    free(ptr);
}

void operator delete[](void* ptr) 
{
    free(ptr);
}

//// Optional: Required if using virtual destructors
//void operator delete(void* ptr, unsigned int size) {
    //free(ptr);
//}

static void* _allocNew(size_t size)
{
	void* p = malloc(size);
	memset(p, 0, size);
	return p;
}
