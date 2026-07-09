using System;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using log4net;

namespace JTAGICEmkII.Adaptor
{
    // My cable. USB\VID_067B&PID_23A3

    public class SerialConfig
    {
        public int PortNumner { get; set; } = 1;
        public uint BaudRate { get; set; } = 19200;
        public SerialParity Parity { get; set; } = SerialParity.None;
        public SerialStopBitCount StopBits { get; set; } = SerialStopBitCount.One;
        public byte DataBits { get; set; } = 8;

        public ushort VenderId { get; set; } = 0x067B;
        public ushort ProductId { get; set; } = 0x23A3;

    }

    public sealed class TxRxComAdaptor : ITxFrameAdaptor, IRxFrameAdaptor, IDisposable
    {

        #region Constructors 
        public TxRxComAdaptor(SerialConfig serialConfig, bool open = true)
        {
            this._serialConfig = serialConfig;
            _logger = LogManager.GetLogger(typeof(TxRxComAdaptor));
            _dataReader = null!;
            _dataWriter = null!;
            _serialDevice = null!;

            if (open)
            {
                Open();
            }
        }

        #endregion


        #region Fields 

        private readonly ILog _logger;
        private readonly SerialConfig _serialConfig;
        private SerialDevice _serialDevice;
        private bool disposedValue;
        private DataReader _dataReader;
        private DataWriter _dataWriter;


        #endregion

        #region Properties

        public bool IsByteToRead
        {
            get
            {
                if (!IsOpen)
                {
                    return false;
                }

                return _serialDevice.BytesReceived > 0;
            }
        }

        public int ComNumber { get; }

        public bool IsOpen => _serialDevice != null;

        #endregion


        #region Public Methods 

        public bool Open()
        {
            if (IsOpen)
            {
                _logger.Warn("Device is already open.");
                return true;
            }

            var aqs = SerialDevice.GetDeviceSelectorFromUsbVidPid(_serialConfig.VenderId, _serialConfig.ProductId);
            DeviceInformationCollection serialDeviceInfos = DeviceInformation.FindAllAsync(aqs).GetAwaiter().GetResult();

            foreach (DeviceInformation serialDeviceInfo in serialDeviceInfos)
            {
                try
                {
                    SerialDevice serialDevice = SerialDevice.FromIdAsync(serialDeviceInfo.Id).GetAwaiter().GetResult();

                    if (serialDevice != null && serialDevice.UsbProductId == _serialConfig.ProductId && serialDevice.UsbVendorId == _serialConfig.VenderId)

                    {
                        // Found a valid serial device.
                        _serialDevice = serialDevice;
                        _serialDevice.Parity = _serialConfig.Parity;
                        _serialDevice.BaudRate = _serialConfig.BaudRate;
                        _serialDevice.DataBits = _serialConfig.DataBits;
                        _serialDevice.StopBits = _serialConfig.StopBits;
                        _serialDevice.ErrorReceived += this._serialDevice_ErrorReceived;
                        _serialDevice.IsRequestToSendEnabled = true;
                        _serialDevice.IsDataTerminalReadyEnabled = true;


                        _dataReader = new DataReader(_serialDevice.InputStream);
                        _dataReader.InputStreamOptions = InputStreamOptions.Partial;
                        _dataReader.UnicodeEncoding = UnicodeEncoding.Utf8;
                        _dataReader.ByteOrder = ByteOrder.LittleEndian;

                        _dataWriter = new DataWriter(_serialDevice.OutputStream);
                        _dataWriter.UnicodeEncoding = UnicodeEncoding.Utf8;
                        _dataWriter.ByteOrder = ByteOrder.LittleEndian;

                        _logger.Info($"Device opened successfully {_serialDevice.PortName}.");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Couldn't instantiate the device", ex);
                }
            }

            return false;
        }

        private void _serialDevice_ErrorReceived(SerialDevice sender, ErrorReceivedEventArgs args) => throw new NotImplementedException();

        public bool Close()
        {
            if (!IsOpen)
                return false;

            _serialDevice.Dispose();
            _dataReader?.Dispose();
            _dataWriter?.Dispose();
            _serialDevice.ErrorReceived -= this._serialDevice_ErrorReceived;
            _serialDevice.Dispose();
            _serialDevice = null!;

            _logger.Info("Device closed successfully.");
            return !IsOpen;

        }

        public bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
        {
            ThrowIfNotOpen();
            return ReadBytesAsync(out values, length, CancellationToken.None, timeout).GetAwaiter().GetResult();
        }

        public Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
        {
            ThrowIfNotOpen();
            uint originalLength = length;
            try
            {
                if (timeout == -1)
                {
                    _serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(0);
                }
                else
                {
                    _serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(timeout);
                }

                _logger.Debug($"Read operation timed out {_serialDevice.ReadTimeout}.");

                values = new byte[originalLength];

                // Reading a byte from the serial device.
                do
                {
                    var a = _dataReader.LoadAsync(length);

                    if (!a.AsTask().Wait(_serialDevice.ReadTimeout))
                    {
                        _logger.Warn("Read operation timed out.");
                        values = null;
                        return Task.FromResult(false);
                    }

                    uint bytesToRead = a.AsTask().Result;

                    byte[] buffer = new byte[bytesToRead];
                    _dataReader.ReadBytes(buffer);

                    Array.Copy(buffer, 0, values, (int)(originalLength - length), (int)bytesToRead);
                    length -= bytesToRead;
                } while (length > 0);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.Error("Error while reading bytes", ex);
                values = null;
                return Task.FromResult(false);
            }
        }
        public int SendBytes(byte[] values)
        {
            ThrowIfNotOpen();
            return SendBytesAsync(values, CancellationToken.None).GetAwaiter().GetResult();
        }

        public Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
        {
            ThrowIfNotOpen();

            try
            {
                // Writing a byte to the serial device.
                _dataWriter.WriteBytes(values);
                return Task.FromResult(values.Length);
            }
            catch (Exception ex)
            {
                _logger.Error("Error while sending bytes", ex);
                return Task.FromResult(-1);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Protected Methods 
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_serialDevice != null)
                    {
                        _serialDevice.ErrorReceived -= this._serialDevice_ErrorReceived;
                        _serialDevice.Dispose();
                    }
                    _dataReader?.Dispose();
                    _dataWriter?.Dispose();

                    _serialDevice = null!;
                    _dataReader = null!;
                    _dataWriter = null!;
                }

                disposedValue = true;
            }
        }

        #endregion

        #region Private Methods 

        private void ThrowIfNotOpen()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Device is not open.");
            }
        }

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
