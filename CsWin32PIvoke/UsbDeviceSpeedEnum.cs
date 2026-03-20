using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsWin32Api
{
    // value from winusbio.h
    public enum UsbDeviceSpeedEnum : byte
    {
        UsbDeviceSpeedInfo = 1,

        UsbUndefinedSpeed = 0,
        UsbLowSpeed = 1,
        UsbFullSpeed = 2,
        UsbHighSpeed = 3,
    }
}
