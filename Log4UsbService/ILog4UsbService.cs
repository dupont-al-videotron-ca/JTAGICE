using log4net.Core;

namespace Log4UsbService
{
    /// <summary>
    /// Interface for the Log4UsbService, providing methods to configure logging levels for USB communication.
    /// </summary>
    public interface ILog4UsbService
    {
        /// <summary>
        /// Sets the logging level for receiving log messages from the USB device.
        /// </summary>
        /// <param name="level">The logging level to set.</param>
        /// <returns>True if the logging level was successfully set; otherwise, false.</returns>
        bool SetReceivingLog(Level level);
    }
}