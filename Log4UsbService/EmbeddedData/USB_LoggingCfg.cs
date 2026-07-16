using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Usb;
using log4net.Core;

namespace Log4UsbService.EmbeddedData;

internal class USB_LoggingCfg 
{
    private UsbSetupPacket setupPacket;

    internal USB_LoggingCfg()
    {
        this.setupPacket = new UsbSetupPacket();
        // 0x40; Vendor request
        this.setupPacket.RequestType.Direction = UsbTransferDirection.Out;
        this.setupPacket.RequestType.ControlTransferType = UsbControlTransferType.Vendor;
        this.setupPacket.RequestType.Recipient = UsbControlRecipient.Device;

        this.setupPacket.Request = 0x01; // Custom request code for logging configuration
        this.setupPacket.Value = 0x0000; // 0 = LogActivation

    }

    internal USB_LoggingCfg(Level level, bool enable) : this()
    {
        IsLoggingEnabled = enable;
        LoggingLevel = level;
    }

    public UsbSetupPacket SetupPacket
    {
        get { return this.setupPacket; }
    }

    public Level LoggingLevel
    {
        get
        {
            USB_LevelType uSB_LevelType = (USB_LevelType)(this.setupPacket.Index & 0x00FF); // Get the lower 8 bits
            return uSB_LevelType switch
            {
                USB_LevelType.All => Level.All,
                USB_LevelType.Fatal => Level.Fatal,
                USB_LevelType.Error => Level.Error,
                USB_LevelType.Warning => Level.Warn,
                USB_LevelType.Info => Level.Info,
                USB_LevelType.Debug => Level.Debug,
                _ => Level.Off,
            };
        }

        set
        {
            USB_LevelType uSB_LevelType = USB_LevelType.None;

            if (value == Level.All)
            {
                uSB_LevelType = USB_LevelType.All;
            }
            else if (value == Level.Off)
            {
            }
            else if (value == Level.Fatal)
            {
                uSB_LevelType = USB_LevelType.Fatal;
            }
            else if (value == Level.Error)
            {
                uSB_LevelType = USB_LevelType.Error;
            }
            else if (value == Level.Warn)
            {
                uSB_LevelType = USB_LevelType.Warning;
            }
            else if (value == Level.Info)
            {
                uSB_LevelType = USB_LevelType.Info;
            }
            else if (value == Level.Debug)
            {
                uSB_LevelType = USB_LevelType.Debug;
            }

            this.setupPacket.Index &= ~(uint)0x00FF; // Clear the lower 8 bits
            this.setupPacket.Index |= (uint)uSB_LevelType; // Clear the lower 8 bits
        }
    }

    public bool IsLoggingEnabled
    {
       
        get
        {
            return (this.setupPacket.Value & 0x0100) != 0; // Check if the 9th bit is set
        }
        set
        {
            if (value)
            {
                this.setupPacket.Value |= 0x0100; // Set the 9th bit to enable logging
            }
            else
            {
                this.setupPacket.Value &= ~(uint)0x0100; // Clear the 9th bit to disable logging
            }
        }

    }
}
