
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Log4UsbService.EmbeddedData;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using UsbDeviceBase;
using Windows.Devices.Usb;

namespace Log4UsbService
{
    public class Log4UsbService : IDisposable, ILog4UsbService
    {


        #region Constructors 
        /// <summary>
        /// Initializes a new instance of the Log4UsbService class with the specified UsbDeviceBase.
        /// </summary>
        /// <param name="usbDeviceBase"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public Log4UsbService(int interfaceId, int pipeId, IUsbDevice usbDevice, ILog logger)
        {
            loggerDebug = logger ?? throw new ArgumentNullException(nameof(logger));
            this.interfaceId = interfaceId;
            this.pipeId = pipeId;
            this.usbDevice = usbDevice ?? throw new ArgumentNullException(nameof(usbDevice));
            pipeIn = null!;
            cancellationSource = new CancellationTokenSource();
            receiveTask = null!;

            usbDevice.DeviceOpened += OnDeviceOpened;
            usbDevice.DeviceClosed += this.OnDeviceClosed;
        }


        /// <summary>
        /// Initializes a new instance of the Log4UsbService class with the specified UsbDeviceBase for testing purpose.
        /// </summary>
        /// <param name="usbDeviceBase"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        internal Log4UsbService(int interfaceId, int pipeId, IUsbDevice controlDevice, IUsbPipeIn pipeIn, ILog logger) : this(interfaceId, pipeId, controlDevice, logger)
        {
            this.pipeIn = pipeIn ?? throw new ArgumentNullException(nameof(pipeIn));
        }
        #endregion


        #region Fields 

        private readonly TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);
        private readonly ILog loggerDebug;
        private readonly int interfaceId;
        private readonly int pipeId;
        private IUsbDevice usbDevice;
        private IUsbPipeIn pipeIn;
        private CancellationTokenSource cancellationSource;
        private Task receiveTask;
        private bool disposedValue;
        private readonly TimeSpan openTimeout = TimeSpan.FromSeconds(5);
        private Level startLogRequestLevel = Level.Off;
        private UInt32 previousTickCount = 0;
        private readonly System.Threading.Lock receiveLock = new ();

        #endregion


        #region Properties 

        internal bool IsReceivingLog => receiveTask != null;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        /// <summary>
        /// Sets the receiving log level. 
        /// If the level is Level.Off, it stops receiving log messages; 
        /// otherwise, it starts receiving log messages with the specified level.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool SetReceivingLog(Level level)
        {
            loggerDebug.Debug($"SetReceivingLog called with level: {level}.");

            if (level == Level.Off)
            {
                return StopReceivingLog();
            }
            else
            {
                return StartReceivingLog(level);
            }
        }

        /// <summary>
        /// Starts receiving log messages from the USB device by sending a logging configuration with the specified levelUsb.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        internal bool StartReceivingLog(Level level)
        {
            startLogRequestLevel = level;

            if (!usbDevice.WaitOpenned(openTimeout))
            {
                loggerDebug.Error("USB device is not opened.");
                return false;
            }

            uint error;

            // start logging on the USB device by sending a logging configuration with the specified levelUsb.
            if ((error = SendLoggingConfigurationToUsbDevice(level, true)) != 0)
            {
                loggerDebug.Error($"Failed to send logging configuration to USB device. Error code: {(int)error:X8}");
                return false;
            }

            if (!IsReceivingLog)
                StartReceivingTask();

            loggerDebug.Debug($"ReceivingLog started with level: {level}.");
            return error == 0;
        }

        /// <summary>
        /// Stops receiving log messages from the USB device by sending a logging configuration with Level.Off.
        /// </summary>
        /// <returns></returns>
        internal bool StopReceivingLog()
        {
            startLogRequestLevel = Level.Off;
            uint error = 1;
            if (!usbDevice.IsOpened)
            {
                loggerDebug.Error("USB device is not opened.");
            }
            else
            {
                // stop logging on the USB device by sending a logging configuration with Level.Off.
                if ((error = SendLoggingConfigurationToUsbDevice(Level.Off, false)) != 0)
                {
                    loggerDebug.Error($"Failed to send logging configuration to USB device. Error code: {(int)error:X8}");
                }
            }

            StopReceivingTask();

            loggerDebug.Debug("ReceivingLog stopped.");
            return error == 0;
        }
        
        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        private bool StopReceivingTask()
        {
            if (this.receiveTask != null && !this.receiveTask.IsCompleted)
            {
                loggerDebug.Debug("Receive task cancel requested.");
                cancellationSource.Cancel(true);
                try
                {
                    if (!this.receiveTask.Wait(1000))
                    {
                        loggerDebug.Error("Timeout while waiting for receive task to complete.");
                        return false;
                    }
                    else
                    {
                        this.receiveTask = null!;
                        loggerDebug.Debug("Receive task completed successfully.");
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
                            this.receiveTask = null!;
                            retval = true;
                        }
                        else
                        {
                            loggerDebug.Fatal("Error while waiting for receive task to complete.", inner);
                        }
                    }
                    return retval;
                }
            }
            else
            {
                loggerDebug.Debug("Receive task already completed.");
            }
            return this.receiveTask == null!;
        }

        private void OnDeviceClosed(object? sender, EventArgs e)
        {
            loggerDebug.Debug($"Device closed: {usbDevice.DeviceName}");

            lock (receiveLock)
            {
                if (this.pipeIn != null && !this.pipeIn.IsDisposed)
                    usbDevice.ReleasePipe(this.pipeIn);

                this.pipeIn = null!;
            }
        }

        private void OnDeviceOpened(object? sender, DeviceInfoEventArgs e)
        {
            loggerDebug.Debug($"Device opened: {e.DeviceInfo.Name}");

            previousTickCount = 0;
            lock (receiveLock)
            {
                this.pipeIn = usbDevice.AcquireInPipe<BulkInPipeImpl>(this.interfaceId, this.pipeId);

                if (this.pipeIn == null)
                {
                    loggerDebug.Error($"Failed to get bulk in pipe for interface {this.interfaceId} and pipe {this.pipeId}.");
                    return;
                }
            }

            SetReceivingLog(this.startLogRequestLevel);

        }


        private void StartReceivingTask()
        {
            cancellationSource = new CancellationTokenSource();

            this.receiveTask = Task.Run(() =>
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
                        this.ReceiveLogFrameAsync();
                    }
                    catch (OperationCanceledException ex)
                    {
                        // Handle cancellation if necessary
                        loggerDebug.Debug("Receiving task OperationCanceledException.", ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        loggerDebug.Fatal("Receiving Exception:", ex);
                    }
                }

                loggerDebug.Debug("Receiving task exit.");
                this.receiveTask = null!;
                return;

            }, this.cancellationSource.Token);

        }

        private async void ReceiveLogFrameAsync()
        {
            try
            {
                receiveLock.Enter();
                if (this.pipeIn != null && this.usbDevice.IsConnected && this.usbDevice.IsOpened && !this.cancellationSource.Token.IsCancellationRequested)
                {
                    //loggerDebug.Debug($"Reading structure: timeout : {receiveTimeout}"); 
                    var logEventDataResult = await this.pipeIn.ReadStructureAsync<USB_LoggingEventData_t>(this.cancellationSource.Token, (int)receiveTimeout.TotalMilliseconds);
                    if (!logEventDataResult.HasValue)
                    {
                        //loggerDebug.Debug("timeout occured.");
                        return;
                    }
                    else if (this.cancellationSource.Token.IsCancellationRequested)
                    {
                        //loggerDebug.Debug("CancellationRequested in ReadStructureAsync.");
                        return;
                    }


                    USB_LoggingEventData_t logEventData = logEventDataResult.Value;

                    if (!logEventData.IsValid)
                    {
                        loggerDebug.Error($"Received invalid log frame header, length:{logEventData.Header.Length}, reserve:{logEventData.Header.Reseved}, Version:{logEventData.Header.Version}, Level:{logEventData.Level}");
                        return;
                    }

                    //loggerDebug.Debug($"Received log frame header: {logEventData.ToString()}");

                    int strLength = logEventData.Header.Length - (ushort)Marshal.SizeOf<USB_LoggingEventData_t>();
                    //loggerDebug.Debug($"Reading strings length {strLength}, timeout: {receiveTimeout}");
                    if (!await this.pipeIn.ReadBytesAsync(out byte[] values, strLength, this.cancellationSource.Token, (int)receiveTimeout.TotalMilliseconds))
                    {
                        //loggerDebug.Debug("ReadBytes timeout.");
                        return;
                    }
                    else if (this.cancellationSource.Token.IsCancellationRequested)
                    {
                        //loggerDebug.Debug("CancellationRequested in ReadBytesAsync.");
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

                    if (previousTickCount == 0)
                    {
                        previousTickCount = logEventData.TickCount;
                    }
                    else
                    {
                        Int32 diff = (Int32)(logEventData.TickCount - previousTickCount);
                        //loggerDebug.Debug($"Received log frame: Level={logEventData.Level}, ElapsedTick={diff}ms, LogName={logName}, FileName={fileName}, Message={message}");
                    }

                    if (String.IsNullOrWhiteSpace(logName))
                    {
                        logName = loggerDebug.Logger.Name;
                    }

                    ILog loggerUsb = LogManager.GetLogger(logName);

                    string logMessage = $"[{logEventData.TickCount,-10}ms]: {fileName}, {message}";
                    loggerUsb.Logger.Log(typeof(Log4UsbService), MapLevel(logEventData.Level), logMessage, null);

                    previousTickCount = logEventData.TickCount;
                }
                else
                {
                    // wait for device (release CPU time)
                    Task.Delay(10).Wait();
                    return;
                }
            }
            catch (OperationCanceledException ex)
            {
                loggerDebug.Debug($"OperationCanceledException {ex.Message}.");
                return;
            }
            catch (COMException ex)
            {
                loggerDebug.Debug($"COMException errorCode: 0x{ex.ErrorCode:X8}:");
                return;
            }
            catch (Exception ex)
            {
                loggerDebug.Fatal("Error while reading log frame header from USB device.", ex);
                return;
            }
            finally
            {
                receiveLock.Exit();
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

            return usbDevice.SendControlOutTransfer(uSB_LoggingCfg.SetupPacket);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.loggerDebug.Info("Disposing Log4UsbService.");
                    if (this.receiveTask != null)
                    {
                        StopReceivingLog();
                        usbDevice.DeviceOpened -= OnDeviceOpened;
                        usbDevice.DeviceClosed -= this.OnDeviceClosed;
                        usbDevice = null!;
                        pipeIn = null!;
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
