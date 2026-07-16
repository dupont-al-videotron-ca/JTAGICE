

using Windows.Devices.Usb;

namespace UsbDeviceBase
{
    public interface IUsbDevice
    {
        UInt16 VendorId { get; }
        UInt16 ProductId { get; }

        string DeviceId { get; }

        string DeviceName { get; }

        //
        // Summary:
        //     Gets the **bConfigurationValue** field of a USB configuration descriptor. The
        //     value is the number that identifies the configuration.
        //
        // Returns:
        //     The number that identifies the configuration.
        byte ConfigurationValue { get; }

        //
        // Summary:
        //     Gets the **bMaxPower** field of a USB configuration descriptor. The value indicates
        //     the maximum power (in milliamp units) that the device can draw from the bus,
        //     when the device is bus-powered.
        //
        // Returns:
        //     The maximum power (in milliamp units) that the device can draw from the bus.
        uint MaxPowerMilliamps { get; }

        //
        // Summary:
        //     Gets the D5 bit value of the **bmAttributes** field in the USB configuration
        //     descriptor. The value indicates whether the device can send a resume signal to
        //     wake up itself or the host system from a low power state.
        //
        // Returns:
        //     True, if the device supports remote wakeup; otherwise false.
        bool RemoteWakeup { get; }

        //
        // Summary:
        //     Gets the D6 bit of the **bmAttributes** field in the USB configuration. This
        //     value indicates whether the device is drawing power from a local source or the
        //     bus.
        //
        // Returns:
        //     True, if the device is drawing power from a local source; false indicates that
        //     the device is only drawing power from the bus.
        bool SelfPowered { get; }

        bool Initialize();

        bool IsConnected { get; }
    }
}
