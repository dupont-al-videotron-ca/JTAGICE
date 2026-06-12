
#include <stdlib.h>
#include "usbEndpointOut.h"

UsbEndpointOut::UsbEndpointOut(uint8_t endpointNumber) : UsbEndpointBase(endpointNumber) {}

bool UsbEndpointOut::IsUsbDevHasReceivedOUT()
{
    this->SelectEndpoint();
    return UsbDevHasReceivedOUT_Data();
}

bool UsbEndpointOut::IsNextToggleData0()
{
    this->SelectEndpoint();
    return UsbDevIsDataToggleBit();
}

bool UsbEndpointOut::InitializeEndpoint(uint8_t type, uint16_t size, uint8_t banks)
{
    this->SelectEndpoint();

	if UsbDevIsEndpointEnabled()
	{
		Debug("~~~ EPx already enabled!\r\n");
	}
	else
	{
		if(!this->InitializeUECFG(type, size, banks, UsbEP_DirOut))
		{
			Debug("~~~ EPx failed to initialize!\r\n");
		}
	}
    
    return false;
}

void UsbEndpointOut::ProcessInterrupt()
{

}

