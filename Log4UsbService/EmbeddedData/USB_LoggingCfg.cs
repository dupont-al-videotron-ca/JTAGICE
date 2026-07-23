using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Usb;
using log4net.Core;

namespace Log4UsbService.EmbeddedData;

/// <summary>
/// Represents the configuration settings for USB logging, including the logging level and whether logging is enabled or disabled. 
/// This class encapsulates a USB setup packet that is used to communicate these settings to a USB device.
/// </summary>
internal class USB_LoggingCfg
{

    #region Constructors 
    /// <summary>
    /// Initializes a new instance of the <see cref="USB_LoggingCfg"/> class with default values.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="USB_LoggingCfg"/> class with specified logging level and enable/disable flag.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="enable"></param>
    internal USB_LoggingCfg(Level level, bool enable) : this()
    {
        IsLoggingEnabled = enable;
        LoggingLevel = level;
    }

    #endregion


    #region Fields 
    private UsbSetupPacket setupPacket;

    #endregion


    #region Properties 


    /// <summary>
    /// Gets the USB setup packet used for configuring logging settings.
    /// </summary>
    public UsbSetupPacket SetupPacket
    {
        get { return this.setupPacket; }
    }

    /// <summary>
    /// Gets or sets the logging level for the USB device. The logging level is determined by the lower 8 bits of the Index field in the setup packet.
    /// </summary>
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
    
    /// <summary>
    /// Gets or sets a value indicating whether logging is enabled for the USB device. The logging enable flag is determined by the 1st bit of the Value field in the setup packet.
    /// </summary>  
    public bool IsLoggingEnabled
    {
        get
        {
            return (this.setupPacket.Value & 0x0001) != 0; // Check if the 1st bit is set
        }
        set
        {
            if (value)
            {
                this.setupPacket.Value |= 0x0001; // Set the 1st bit to enable logging
            }
            else
            {
                this.setupPacket.Value &= ~(uint)0x0001; // Clear the 1st bit to disable logging
            }
        }

    }

    #endregion

}
