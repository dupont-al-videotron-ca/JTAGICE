
//#include <avr/interrupt.h>
#include <stdint.h>
#include "..\src\usb_drv.h"
#include "usart_debug.h"
#include "usb_api.h"
#include "usb_requests.h"

#ifndef USBENDPOINTBASE
#define USBENDPOINTBASE


struct EndpointInterruptStatus
{
	union
	{
		struct
		{
            //
			uint8_t TXIN_flg: 1;
			uint8_t STALLED_flg: 1;
			uint8_t RXOUT_flg: 1;
			uint8_t RXSTP_flg: 1;
			uint8_t NAKOUT_flg: 1;
			uint8_t RWAL_flg: 1;
			uint8_t NAKIN_flg: 1;
			uint8_t FIFOCON_flg: 1;
            
		} Flags;
		uint8_t Data;
	} U_UEIENX;

	union
	{
    	struct
    	{
        	uint8_t NBUSYBK_flg: 2;
        	uint8_t DTSEQ_flg: 2;
        	uint8_t reserved: 1;
			uint8_t UNDERF_flg: 1;
			uint8_t OVERF_flg: 1;
        	uint8_t CFGOK_flg: 1;     	
    	} Flags;
    	uint8_t Data;
	} U_UESTA0X;    
};

class UsbEndpointBase
{
protected:
    uint8_t _endpointNumber;
	EndpointInterruptStatus _endpointInterruptStatus;
	

public:
    UsbEndpointBase(uint8_t endpointNumber);

    void SelectEndpoint();


    bool IsUsbDevHasReceivedOUT();

    bool IsUsbDevHasReceivedSETUP();

    void ToggleResetBit();
    void SetResetBit();
    void ClearResetBit();

    void StallRequest();
    void ClearStallRequest();

    void EnableEndpoint();

    uint8_t GetDataToggle();
	
	void HandleInterrupt();
	
	virtual void ProcessInterrupt();

	void DisableAllInterrupt()
	{
		UsbDevDisableAllInt();
	}
	
protected:
    bool InitializeUECFG(uint8_t type, uint16_t size, uint8_t banks, uint8_t dir);

    bool UsbDevReadBytesN(void* c, uint8_t n);
    
    uint8_t DisableInterrupt();
    void EnableInterrupt(uint8_t intrValue);
    

};

#endif // !USBENDPOINTBASE
