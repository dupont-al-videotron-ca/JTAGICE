
#include <stdlib.h>
#include "usbEndpointIn.h"


UsbEndpointIn::UsbEndpointIn(uint8_t endpointNumber) : UsbEndpointBase(endpointNumber)
{
}

bool UsbEndpointIn::IsUsbDevFifoEmpty()
{
    this->SelectEndpoint();
    return UsbDevIsFifoControllBitSet();
}

void UsbEndpointIn::ClearFifoControllBit()
{
    this->SelectEndpoint();
    UsbDevClearFifoControllBit();
}

void UsbEndpointIn::SetNextToggleData0()
{
    this->SelectEndpoint();
    UsbDevResetDataToggleBit();
}

bool UsbEndpointIn::InitializeEndpoint(uint8_t type, uint16_t size, uint8_t banks)
{
    this->SelectEndpoint();

	if UsbDevIsEndpointEnabled() 
	{
		Debug("~~~ EPx already enabled!\r\n");
	}
	else
	{
		if(!this->InitializeUECFG(type, size, banks, UsbEP_DirIn))
		{
			Debug("~~~ EPx failed to initialize!\r\n");
		}
	}
    
    return false;
}

void UsbEndpointIn::ProcessInterrupt()
{

}

