#pragma warning disable CS8981
using Windows.Win32;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    /// <summary>
    /// SetupDiGetClassDdevsFlags enumeration.
    /// </summary>
    /// 
    [Flags]
    public enum SetupDiGetClassDevsFlags : uint
    {
        DIGCF_DEFAULT = winmdroot.Devices.DeviceAndDriverInstallation.SETUP_DI_GET_CLASS_DEVS_FLAGS.DIGCF_DEFAULT,
        DIGCF_PRESENT = winmdroot.Devices.DeviceAndDriverInstallation.SETUP_DI_GET_CLASS_DEVS_FLAGS.DIGCF_PRESENT,
        DIGCF_ALLCLASSES = winmdroot.Devices.DeviceAndDriverInstallation.SETUP_DI_GET_CLASS_DEVS_FLAGS.DIGCF_ALLCLASSES,
        DIGCF_PROFILE = winmdroot.Devices.DeviceAndDriverInstallation.SETUP_DI_GET_CLASS_DEVS_FLAGS.DIGCF_PROFILE,
        DIGCF_DEVICEINTERFACE = winmdroot.Devices.DeviceAndDriverInstallation.SETUP_DI_GET_CLASS_DEVS_FLAGS.DIGCF_DEVICEINTERFACE,
        DIGCF_INTERFACEDEVICE = winmdroot.Devices.DeviceAndDriverInstallation.SETUP_DI_GET_CLASS_DEVS_FLAGS.DIGCF_INTERFACEDEVICE,
    }
}