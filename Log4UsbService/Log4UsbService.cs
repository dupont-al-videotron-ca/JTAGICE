
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Log4UsbService.EmbeddedData;
using UsbDeviceBase;

namespace Log4UsbService
{
    public class Log4UsbService: IDisposable
    {
        private readonly ILog loggerDebug;
        private readonly IUSBControlDevice usbControl;
        private readonly IUsbPipeIn pipeIn;
        private readonly CancellationTokenSource cancellationSource;
        private Task _receiveTask;
        private bool disposedValue;

        public CancellationToken CancellationToken { get; private set; }

        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the Log4UsbService class with the specified UsbDeviceBase.
        /// </summary>
        /// <param name="usbDeviceBase"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public Log4UsbService(IUSBControlDevice controlDevice, IUsbPipeIn pipe)
        {
            loggerDebug = LogManager.GetLogger(GetType());
            this.usbControl = controlDevice ?? throw new ArgumentNullException(nameof(controlDevice));
            pipeIn = pipe ?? throw new ArgumentNullException(nameof(pipe));
            cancellationSource = new CancellationTokenSource();
            _receiveTask = null!;
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        /// <summary>
        /// Starts receiving log messages from the USB device by sending a logging configuration with the specified levelUsb.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool StartReceivingLog(Level level)
        {
            uint error;

            if ((error = SendLoggingConfigurationToUsbDevice(level, true)) != 0)
            {
                loggerDebug.Error($"Failed to send logging configuration to USB device. Error code: {error}");
                return false;
            }

            StartReceivingTask();
            loggerDebug.Debug("Log4UsbService started.");
            return true;
        }

        /// <summary>
        /// Stops receiving log messages from the USB device by sending a logging configuration with Level.Off.
        /// </summary>
        /// <returns></returns>
        public bool StopReceivingLog()
        {
            uint error;

            if ((error = SendLoggingConfigurationToUsbDevice(Level.Off, false)) != 0)
            {
                loggerDebug.Error($"Failed to send logging configuration to USB device. Error code: {error}");
                return false;
            }

            if(this._receiveTask != null && !this._receiveTask.IsCompleted)
            {
                cancellationSource.Cancel(true);
                try
                {
                    if (!this._receiveTask.Wait(1000))
                    {
                        loggerDebug.Error("Timeout while waiting for receive task to complete.");
                        return false;
                    }
                }
                catch (AggregateException ex)
                {
                    bool retval = false;
                    foreach (var inner in ex.InnerExceptions)
                    {
                        if (inner is OperationCanceledException)
                        {
                            loggerDebug.Debug("Receive task canceled successfully.");
                            retval = true;
                        }
                        else
                        {
                            loggerDebug.Error("Error while waiting for receive task to complete.", inner);
                        }
                    }
                    this._receiveTask = null!;
                    return retval;
                }
            }

            this._receiveTask = null!;
            loggerDebug.Debug("Log4UsbService stopped.");
            return true;
        }
        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        private void StartReceivingTask()
        {
            this._receiveTask = Task.Run(() =>
            {
                loggerDebug.Debug("receiving Task running.");
                while (true)
                {
                    if (this.cancellationSource.Token.IsCancellationRequested)
                    {
                        loggerDebug.Debug("Receiving task cancellation requested.");
                        break;
                    }

                    try
                    {
                        this.ReceiveLogFrame();
                    }
                    catch (OperationCanceledException ex)
                    {
                        // Handle cancellation if necessary
                        loggerDebug.Debug("Receiving task OperationCanceledException.", ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        loggerDebug.Error("Receiving Exception:", ex);
                    }
                }

                loggerDebug.Debug("Receiving task exit.");
                return;

            }, this.cancellationSource.Token);
        }

        private void ReceiveLogFrame()
        {
            USB_LoggingEventData_t logEventData = new USB_LoggingEventData_t();

            try
            {
                if (this.usbControl.IsConnected)
                {
                    if (!this.pipeIn.ReadStructure(ref logEventData, 1000))
                    {
                        return;
                    }

                    int strLength = logEventData.Header.Length - (ushort)Marshal.SizeOf<USB_LoggingEventData_t>();
                    if (!this.pipeIn.ReadBytes(out byte[] values, (uint)strLength, 1000))
                    {
                        return;
                    }

                    string logName = string.Empty;
                    string fileName = string.Empty;
                    string message = string.Empty;

                    var span = new Span<byte>(values);
                    string logStrings = System.Text.Encoding.UTF8.GetString(span);
                    var count = logStrings.Count(c => c == '\0');
                    string[] strings = logStrings.Split("\0", count+1, StringSplitOptions.None);

                    if (strings.Length >= 1)
                    {
                        logName = strings[0];
                    }

                    if (strings.Length >= 2)
                    {
                        fileName = strings[1];
                    }

                    if (strings.Length >= 3)
                    {
                        message = strings[2];
                    }

                    ILog loggerUsb = LogManager.GetLogger(logName);
                    string logMessage = $"[{logEventData.TickCount,-10}ms]: {fileName}, {message}";
                    loggerUsb.Logger.Log(typeof(Log4UsbService), MapLevel(logEventData.Level), logMessage, null);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                loggerDebug.Error("Error while reading log frame header from USB device.", ex);
            }
        }

        private Level MapLevel(byte levelUsb)
        {
            return levelUsb switch
            {
                0 => Level.Off,
                1 => Level.Fatal,
                2 => Level.Error,
                3 => Level.Warn,
                4 => Level.Info,
                5 => Level.Debug,
                _ => Level.All,
            };
        }

        /// <summary>
        /// Sends the logging configuration to the USB device.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>0 success.</returns>
        private uint SendLoggingConfigurationToUsbDevice(Level level, bool enable)
        {
            USB_LoggingCfg uSB_LoggingCfg = new USB_LoggingCfg();
            uSB_LoggingCfg.LoggingLevel = level;
            uSB_LoggingCfg.IsLoggingEnabled = enable;

            return usbControl.SendControlOutTransfer(uSB_LoggingCfg.SetupPacket);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this._receiveTask != null)
                    {
                        StopReceivingLog();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
