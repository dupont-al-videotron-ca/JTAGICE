using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsWin32Api
{
    public enum UsbDescriptorTypeEnum : byte
    {
        // USB 1.1: 9.4 Standard Device Requests, Table 9-5. Descriptor Types
        //
        USB_DEVICE_DESCRIPTOR_TYPE = 0x01,
        USB_CONFIGURATION_DESCRIPTOR_TYPE = 0x02,
        USB_STRING_DESCRIPTOR_TYPE = 0x03,
        USB_INTERFACE_DESCRIPTOR_TYPE = 0x04,
        USB_ENDPOINT_DESCRIPTOR_TYPE = 0x05,
        
        //
        // USB 2.0: 9.4 Standard Device Requests, Table 9-5. Descriptor Types
        //
        USB_DEVICE_QUALIFIER_DESCRIPTOR_TYPE = 0x06,
        USB_OTHER_SPEED_CONFIGURATION_DESCRIPTOR_TYPE = 0x07,
        USB_INTERFACE_POWER_DESCRIPTOR_TYPE = 0x08,
        EUSB2_ISOCH_ENDPOINT_COMPANION_DESCRIPTOR_TYPE = 0x12,

        //
        // USB 3.0: 9.4 Standard Device Requests, Table 9-5. Descriptor Types
        //
        USB_OTG_DESCRIPTOR_TYPE = 0x09,
        USB_DEBUG_DESCRIPTOR_TYPE = 0x0A,
        USB_INTERFACE_ASSOCIATION_DESCRIPTOR_TYPE = 0x0B,
        USB_BOS_DESCRIPTOR_TYPE = 0x0F,
        USB_DEVICE_CAPABILITY_DESCRIPTOR_TYPE = 0x10,
        USB_SUPERSPEED_ENDPOINT_COMPANION_DESCRIPTOR_TYPE = 0x30,

        //
        // USB 3.1: 9.4 Standard Device Requests, Table 9-6. Descriptor = Type
        //
        USB_SUPERSPEEDPLUS_ISOCH_ENDPOINT_COMPANION_DESCRIPTOR_TYPE = 0x31
    }
}
