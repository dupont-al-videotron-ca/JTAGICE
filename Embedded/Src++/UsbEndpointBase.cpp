
#include <stdlib.h>
#include <string.h>
#include "usbEndpointBase.h"

UsbEndpointBase::UsbEndpointBase(uint8_t endpointNumber)
{
    this->_endpointNumber = endpointNumber;
    memset(&this->_endpointInterruptStatus, 0, sizeof(EndpointInterruptStatus));
}

void UsbEndpointBase::ProcessInterrupt()
{
}

bool UsbEndpointBase::IsUsbDevHasReceivedSETUP()
{
    this->SelectEndpoint();
    return UsbDevHasReceivedSETUP();
}

void UsbEndpointBase::SelectEndpoint()
{
    UsbDevSelectEndpoint(this->_endpointNumber);
}

void UsbEndpointBase::ToggleResetBit()
{
    this->SelectEndpoint();
    UsbDevResetEndpoint(this->_endpointNumber);
}

void UsbEndpointBase::SetResetBit()
{
    this->SelectEndpoint();
    UsbDevSetResetEndpoint(this->_endpointNumber);
}

void UsbEndpointBase::ClearResetBit()
{
    this->SelectEndpoint();
    UsbDevClearResetEndpoint(this->_endpointNumber);
}

void UsbEndpointBase::StallRequest()
{
    this->SelectEndpoint();
    UsbDevRequestStallHandshake();
}

void UsbEndpointBase::ClearStallRequest()
{
    this->SelectEndpoint();
    UsbDevClearStallRequest();
}

void UsbEndpointBase::EnableEndpoint()
{
    this->SelectEndpoint();
    UsbDevEnableEndpoint();
}

bool UsbEndpointBase::IsUsbDevHasReceivedOUT()
{
    this->SelectEndpoint();
    return UsbDevHasReceivedOUT_Data();
}

uint8_t UsbEndpointBase::GetDataToggle()
{
    this->SelectEndpoint();
    return UsbDevGetDataToggle();
}

bool UsbEndpointBase::UsbDevReadBytesN(void* c, uint8_t n)
{
    volatile uint8_t msbCnt = UEBCHX;
    volatile uint8_t lsbCnt = UEBCLX;
    
    // if no data available  it is because the interrupt is still on, i.e. not processed! 
    if(msbCnt == 0 && lsbCnt == n)
    {
        uint8_t* ptr = (uint8_t*)c;
        while (n--)
        {
            *ptr++ = UsbDevReadByte();
        }

        return true;       
    }
    else
    {
        return false;
    }
}

// based on usb_drv::UsbDevEP_Setup.
bool UsbEndpointBase::InitializeUECFG(uint8_t type, uint16_t size, uint8_t banks, uint8_t dir)
{
    uint8_t i, j;
    uint8_t num = _endpointNumber;
    
    banks--;
    if ((num > 6) || (dir > 1) || (banks > 1) || (type > 3) || (size & 0xFC07)) return false;
    i = (uint8_t) (size / 8);
    if ((UsbDevIsLowSpeedSelected()) && ((type == UsbEP_TypeBulk) || (type == UsbEP_TypeIso) || (i > 1))) return false;
    if (!((i == 1) || (i == 2) || (i == 4) || (i == 8)))
    if (!(((i == 16) || (i == 32) || (i == 64)) && (type == UsbEP_TypeIso))) return false;
    if ((type == UsbEP_TypeControl) && ((dir != UsbEP_DirControl) || (banks > 0))) return false;
    if (num == 0)
    {
        if ((dir != UsbEP_DirControl) || (type != UsbEP_TypeControl)) return false; // EP0 is always control ep
    }
    else
    {
        if ((num != UsbAllocatedEPs) || // allocate eps in growing order, see section 21.7
        (type == UsbEP_TypeControl)) return false; // more than one control ep may be ok?
    }
    UsbDevSelectEndpoint(num);
    UsbDevEnableEndpoint(); // enable ep before memory is allocated? Yes, see figure 22-2
    j = 0;
    while ((i = (i >> 1))) j++;
    UECFG0X = ((type << 6) | (dir));
    UECFG1X = ((j << 4) | (banks << 2));
    UECFG1X |= (1 << ALLOC);
    if (UESTA0X & (1 << CFGOK))
    {
        UsbAllocatedEPs++;
        return true;
    }
    else
    {
        //          UsbDevDisableEndpoint();
        return false;
    }
    //
    //uint8_t i, j;
    //banks--;
    //if ((_endpointNumber > 6) || (dir > 1) || (banks > 1) || (type > 3) || (size & 0xFC07))
    //{
    //return false;
    //
    //}
    //
    //i = (uint8_t) (size / 8);
    //if ((UsbDevIsLowSpeedSelected()) && ((type == UsbEP_TypeBulk) || (type == UsbEP_TypeIso) || (i > 1)))
    //{
    //return false;
    //
    //}
    //
    //if (!((i == 1) || (i == 2) || (i == 4) || (i == 8)|| (i == 16) || (i == 32) || (i == 64)) && (type == UsbEP_TypeIso))
    //{
    //return false;
    //
    //}
    //
    //if ((type == UsbEP_TypeControl) && ((dir != UsbEP_DirControl) || (banks > 0)))
    //{
    //return false;
    //}
    //
    //if (_endpointNumber == 0)
    //{
    //if ((dir != UsbEP_DirControl) || (type != UsbEP_TypeControl))
    //{
    //return false; // EP0 is always control ep
    //}
    //}
    //
    //// allocate eps in growing order, see section 21.7
    //else if ((_endpointNumber != UsbAllocatedEPs) || (type == UsbEP_TypeControl))
    //{
    //return false; // more than one control ep may be ok?
    //}
    //
    //
    //SelectEndpoint();
    //UsbDevEnableEndpoint(); // enable ep before memory is allocated? Yes, see figure 22-2
    //j = 0;
    //while ((i = (i >> 1))) j++;
    //
    //UECFG0X = ((type << 6) | (dir));
    //UECFG1X = ((j << 4) | (banks << 2));
    //UECFG1X |= (1 << ALLOC);
    //if (UESTA0X & (1 << CFGOK))
    //{
    //UsbAllocatedEPs++;
    //return true;
    //}
    //else
    //{
    //UsbDevDisableEndpoint();
    //return false;
    //}
}

uint8_t UsbEndpointBase::DisableInterrupt()
{
    uint8_t retval = UsbDevGetAllInt();
    //UsbDevDisableAllInt();
    return retval;
}

void UsbEndpointBase::EnableInterrupt(uint8_t value)
{
    //UsbDevEnableAllInt(value);
}

// take snapshot off intr flags.
void UsbEndpointBase::HandleInterrupt()
{
    
    SelectEndpoint();

    // do not clear interrupt here. see ProcessInterrupt.
    _endpointInterruptStatus.U_UEIENX.Data = UEINTX;
    _endpointInterruptStatus.U_UESTA0X.Data = UESTA0X;
    
}


