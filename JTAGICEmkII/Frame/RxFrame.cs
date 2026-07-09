#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
//#define LOGGER

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;
using MyFramework;
using MyFramework.Threading;
using log4net;
using JTAGICEmkII.Frame;

namespace JTAGICEmkII
{
    public sealed class RxFrame : IRxFrame, IDisposable
    {
        #region Constructors 

        internal RxFrame(IRxFrameAdaptor rxComAdaptor)
        {
            Logger = LogManager.GetLogger(this.GetType());
            _state = RxStateEnum.WaitStart;

            _source = new CancellationTokenSource();

            PreviousSequenceNumber1 = -1;
            Timeout = 1000; // Default _timeout of 1000 milliseconds
            this._rxFrameAdaptor = rxComAdaptor ?? throw new ArgumentNullException(nameof(rxComAdaptor));
            PreviousSequenceNumber = new SequenceNumber();
        }


        #endregion


        #region Fields 
        private const byte ESCAPE_BYTE = 27;
        private const byte TOKEN_BYTE = 14;
        private const UInt16 SequenceNumberWrap = 0xFFFF;
        private const UInt16 SequenceNumberEvent = 0xFFFF;
        private readonly IRxFrameAdaptor _rxFrameAdaptor;
        private RxStateEnum _state;
        private bool _disposedValue;
        private Task _receiveTask;
        private UInt32 messageLength;

        private CancellationTokenSource _source;
        private List<byte> _frameBuffer = new List<byte>();
        private List<byte> _messageBuffer = new List<byte>();
        private UInt16 _rxSequenceNumber;

        #endregion


        #region Properties 

        internal int PreviousSequenceNumber1 { get; set; }
        internal SequenceNumber PreviousSequenceNumber { get; set; }

        internal bool TimeoutOccured { get; set; }

        internal ILog Logger { get; private set; }

        public bool IsReceiving => _receiveTask != null && !_receiveTask.IsCompleted;

        public IRxFrameAdaptor Adaptor => _rxFrameAdaptor;

        /// <summary>
        /// Timeout in milliseconds for receiving each part of the frame (e.g., waiting for start byte, sequence number, _token, message bytes, CRC).
        /// </summary>
        internal int Timeout { get; set; }

        internal CancellationToken CancellationToken { get => this._source.Token; }

        #endregion


        #region Delegates / Events 

        public event EventHandler<ResponseReceivedEventArgs>? ResponseReceived;
        public event EventHandler<CommandReceivedEventArgs>? CommandReceived;
        public event EventHandler? RxTimerExpired;

        #endregion


        #region Public Methods 

        public void StartReceiving()
        {
            Logger.Debug("receiving start.");
            this._receiveTask = Task.Run(() =>
            {
                Logger.Debug("receiving Task running.");
                while (true)
                {
                    if (this.CancellationToken.IsCancellationRequested)
                    {
                        Logger.Debug("Receiving task cancellation requested.");
                        break;
                    }

                    try
                    {
                        this.ReceiveRxFrame();
                    }
                    catch (OperationCanceledException ex)
                    {
                        // Handle cancellation if necessary
                        Logger.Debug("Receiving task OperationCanceledException.", ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        this.ResetReceiver();
                        Logger.Fatal("Receiving Exception:", ex);
                    }
                }

                Logger.Debug("Receiving task exit.");
                return;
            }, this._source.Token);
        }

        private void ResetReceiver()
        {
#if LOGGER
            Logger.Debug($"ResetReceiver.");
#endif
            PreviousSequenceNumber1 = -1;
            GoWaitStart();
        }

        public void StopReceiving()
        {
            try
            {
#if !LOGGER
                Logger.Debug("Receiving Task stop request.");
#endif
                GoStop();
                _source.Cancel(true);
                _receiveTask?.Wait(100);
                _receiveTask = null;
#if !LOGGER
                Logger.Debug("Stop terminated.");
#endif
            }
            catch (OperationCanceledException ex)
            {
                Logger.Debug($"Stop OperationCanceledException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Debug($"Stop Exception: {ex.Message}");
            }
        }
        #endregion


        #region Protected Methods 

        internal bool ReadByte(out byte value, int timeout = -1)
        {
            var retval = this.ReadBytes(out byte[]? values, 1, timeout);
            if(retval && values != null && values.Length == 1)
                value = values[0];
            else
                value = 0;

            return retval;
        }

        internal bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
            => _rxFrameAdaptor.ReadBytes(out values, length, timeout);

        internal Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
            => _rxFrameAdaptor.ReadBytesAsync(out values, length, cancellationToken, timeout);

        internal void OnRxTimeoutOccured()
        {
            if (TimeoutOccured)
            {
                TimeoutOccured = false;
                RxTimerExpired?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void OnReponseReceived(ISlaveResponse response)
        {
            ResponseReceived?.Invoke(this, new ResponseReceivedEventArgs(response));
        }

        internal void OnCommandReceived(Master.IMasterCommand command)
        {
            CommandReceived?.Invoke(this, new CommandReceivedEventArgs(command));
        }

        internal void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        StopReceiving();
                        _receiveTask?.Dispose();
                        _source.Dispose();
                    }
                    catch (AggregateException ex)
                    {
                        Logger.Debug($"Dispose AggregateException: {ex.GetBaseException().Message}");
                    }


                    // free unmanaged resources (unmanaged objects) and override finalizer
                    // set large fields to null
                    _disposedValue = true;
                }
            }
        }

        //  override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RxFrame()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods 

        private void GoWaitStart()
        {
            if (_state != RxStateEnum.WaitStart)
            {
#if LOGGER
                Logger.Debug($"GoWaitStart.");
#endif
                _frameBuffer.Clear();
                _messageBuffer.Clear();
                _state = RxStateEnum.WaitStart;
            }
        }

        private void GoWaitSequenceNumber()
        {

            if (_state != RxStateEnum.WaitSequenceNumber)
            {
#if LOGGER
                Logger.Debug($"GoWaitSequenceNumber.");
#endif
                _state = RxStateEnum.WaitSequenceNumber;
            }
        }

        private void GoWaitToken()
        {
            if (_state != RxStateEnum.WaitToken)
            {
#if LOGGER
                Logger.Debug($"GoWaitToken.");
#endif
                _state = RxStateEnum.WaitToken;
            }
        }

        private void GoWaitMessageSize()
        {
            if (_state != RxStateEnum.WaitMessageSize)
            {
#if LOGGER
                Logger.Debug($"GoWaitMessageSize.");
#endif
                _state = RxStateEnum.WaitMessageSize;
            }
        }

        private void GoWaitMessage()
        {
            if (_state != RxStateEnum.WaitMessage)
            {
#if LOGGER
                Logger.Debug($"GoWaitMessage.");
#endif
                _state = RxStateEnum.WaitMessage;
            }
        }

        private void GoWaitCRC()
        {
            if (_state != RxStateEnum.WaitCRC)
            {
#if LOGGER
                Logger.Debug($"GoWaitCRC.");
#endif
                _state = RxStateEnum.WaitCRC;
            }
        }

        private void GoStop()
        {
            if (_state != RxStateEnum.Stop)
            {
#if LOGGER
                Logger.Debug($"GoStop.");
#endif
                _state = RxStateEnum.Stop;
            }
        }

        private void DispathMessageBody(List<byte> messageBuffer, uint messageLength)
        {
            try
            {
                ISlaveResponse? response = null;
                Master.IMasterCommand? command = null;

                if ((response = Slave.ResponseFactory.CreateResponse((SlaveResponseEnum)messageBuffer[0])) != null)
                {
                    response.MessageLength = messageLength;
                    response.ReadFromBytes(messageBuffer.ToArray());
                    // Now you can use the 'response' object as needed

                    OnReponseReceived(response);
                }
                else if ((command = Master.CommandFactory.CreateCommand((Master.MasterCommandEnum)messageBuffer[0])) != null)
                {
                    command.MessageLength = messageLength;
                    command.ReadFromBytes(messageBuffer.ToArray());
                    OnCommandReceived(command);
                }
                else
                {
                    Logger.Fatal($"Handle unknown message id: {messageBuffer[0]}.");
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal($"DispathMessageBody: {ex.Message}");
                throw;
            }
        }

        private void ReceiveRxFrame()
        {
            // read bytes according to the current _state, and update the _state machine accordingly.
            // The read bytes will be stored in _frameBuffer, and the message body bytes will be stored in _messageBuffer.
            byte rxbyte;
            byte[]? rxBytes = null;
            switch (_state)
            {
                case RxStateEnum.Stop:
                    // Do nothing, just wait for stop _state to be reset
                    break;

                case RxStateEnum.WaitStart:
                    if (!ReadByte(out rxbyte, Timeout))
                    {
                        OnRxTimeoutOccured();
                        GoWaitStart();
                        break;
                    }

                    if (rxbyte == ESCAPE_BYTE)
                    {
#if LOGGER
                        Logger.Debug("Start byte received.");
#endif
                        _frameBuffer.Add(rxbyte); // Add the escape byte to the frame buffer
                        GoWaitSequenceNumber();
                    }
                    else
                    {
                        GoWaitStart();
                    }
                    break;
                case RxStateEnum.WaitSequenceNumber:
                    if (!ReadBytes(out rxBytes, 2, Timeout))
                    {
                        OnRxTimeoutOccured();
                        GoWaitStart();
                        break;
                    }

                    if (rxBytes?.Length == 2)
                    {
                        _frameBuffer.AddRange(rxBytes);

                        // LSB is first byte, MSB is second byte
                        _rxSequenceNumber = BinaryPrimitives.ReadUInt16LittleEndian(rxBytes);
#if LOGGER
                        Logger.Debug($"Sequence number : {_rxSequenceNumber}.");
#endif
                        GoWaitMessageSize();
                    }
                    else
                    {
                        Logger.Error($"Handle error in reading sequence number bytes.");
                        // Handle error in reading sequence number bytes
                        GoWaitStart();
                    }
                    break;
                case RxStateEnum.WaitMessageSize:
                    if (!ReadBytes(out rxBytes, 4, Timeout))
                    {
                        OnRxTimeoutOccured();
                        GoWaitStart();
                        break;
                    }

                    if (rxBytes?.Length == 4) // message size 
                    {
                        _frameBuffer.AddRange(rxBytes); // Add the message size bytes to the frame buffer

                        // LSB is first byte, MSB is last byte
                        messageLength = BinaryPrimitives.ReadUInt32LittleEndian(rxBytes);

#if LOGGER
                        Logger.Debug($"message Length:{messageLength:x4}.");
#endif
                        GoWaitToken();
                    }
                    else
                    {
                        Logger.Error($"Handle error in reading message size bytes.");
                        GoWaitStart();
                    }

                    break;
                case RxStateEnum.WaitToken:
                    if (!ReadByte(out rxbyte, Timeout))
                    {
                        OnRxTimeoutOccured();
                        GoWaitStart();
                        break;
                    }

                    if (rxbyte == TOKEN_BYTE)
                    {
#if LOGGER
                        Logger.Debug($"Received TOKEN.");
#endif
                        _frameBuffer.Add(rxbyte); // Add the _token byte to the frame buffer
                        GoWaitMessage();
                    }
                    else
                    {
                        Logger.Error($"Handle error in reading token byte.");
                        GoWaitStart();
                    }
                    break;
                case RxStateEnum.WaitMessage:
                    if (!ReadBytes(out byte[]? messageBytes, messageLength, Timeout))
                    {
                        OnRxTimeoutOccured();
                        GoWaitStart();
                        break;
                    }

                    if (messageBytes?.Length == messageLength)
                    {
#if LOGGER
                        Logger.Debug($"Received message bytes, length: {messageBytes.Length}.");
#endif

                        _frameBuffer.AddRange(messageBytes); // Add the message bytes to the frame buffer
                        _messageBuffer.AddRange(messageBytes); // Store the message bytes separately   
                        GoWaitCRC();
                    }
                    else
                    {
                        Logger.Error($"Handle error in reading message bytes. length: {messageBytes?.Length}, expected: {messageLength}.");
                        GoWaitStart();
                    }
                    break;
                case RxStateEnum.WaitCRC:
                    if (!ReadBytes(out byte[]? crcBytes, 2, Timeout))
                    {
                        OnRxTimeoutOccured();
                        GoWaitStart();
                        break;
                    }
                    if (crcBytes?.Length == 2)
                    {
                        _frameBuffer.AddRange(crcBytes); // Add the CRC bytes to the frame buffer
                        if (Crc16.ValidateCrc(_frameBuffer.ToArray()))
                        {
                            Logger.Info($"CRC valid: 0x{crcBytes[1]:x2}{crcBytes[0]:x2}.");

                            if (ManageSequenceNumber())
                            {
                                // Accept the message and dispatch it for processing
                                Logger.Info($"Message length: {messageLength} & messageBuffer.count: {_messageBuffer.Count}.");
                                DispathMessageBody(_messageBuffer, messageLength);
                            }
                            // else, allredy received, just ignore this message and wait for next message with correct sequence number.

                            GoWaitStart();
                        }
                        else
                        {
                            Logger.Error($"Handle CRC validation failure: 0x{crcBytes[1]:X2}{crcBytes[0]:X2}.");
                            GoWaitStart();
                        }
                    }
                    break;
                default:
                    // Handle unexpected _state
                    Logger.Error($"Invalid State: {_state}.");
                    GoWaitStart();
                    break;
            }
        }

        public bool ManageSequenceNumber()
        {
#if LOGGER
            Logger.Debug($"ManageSequenceNumber2, received sequence number: {_rxSequenceNumber}.");
#endif

            // Is Event ?
            if (_rxSequenceNumber != SequenceNumberEvent)
            {
                // no
                bool retval = false;

                // Is first message received?
#if LOGGER
                Logger.Debug($"Initial {PreviousSequenceNumber.IsInitial}, Last sequence number: {PreviousSequenceNumber.NumberValue}.");
#endif
                if (PreviousSequenceNumber.IsInitial)
                {
                    PreviousSequenceNumber = new SequenceNumber(_rxSequenceNumber);
                    retval = true;
                }
                else
                {
                    UInt16 expectedSequenceNumber1 = (UInt16)((PreviousSequenceNumber1 + 1) % SequenceNumberWrap);
                    Int16 diffSequenceNumber1 = (Int16)(expectedSequenceNumber1 - _rxSequenceNumber);

                    SequenceNumber expectedSequenceNumber = new SequenceNumber(PreviousSequenceNumber);
                    expectedSequenceNumber++;
                    var diffSequenceNumber = expectedSequenceNumber.Difference(_rxSequenceNumber);
#if LOGGER
                    Logger.Debug($"Diff: {diffSequenceNumber}, expected sequence number: {expectedSequenceNumber}, received: {_rxSequenceNumber}.");
#endif
                    if (diffSequenceNumber == 0)
                    {
                        PreviousSequenceNumber++;
                        retval = true;
                    }
                    else if (diffSequenceNumber < 0)
                    {
                        // We mist a frame, log a warning but still accept this message and update the sequence number to avoid blocking the receiving of next messages.
                        Logger.Warn($"Missed frame(s), expected sequence number {expectedSequenceNumber}, received {_rxSequenceNumber}.");
                        PreviousSequenceNumber = new SequenceNumber(_rxSequenceNumber);
                        retval = true;
                    }
                    else// if (diffSequenceNumber > 0)
                    {
                        // This is a retransmission of the previous message, log a warning. 
                        Logger.Warn($"Received a retransmission of the previous message, expected sequence number {expectedSequenceNumber}, received {_rxSequenceNumber}.");
                        retval = false; // Do not update the sequence number, just ignore this message and wait for next message with correct sequence number.
                    }
                }

#if LOGGER
                Logger.Debug($"Return {retval} with sequence number: {PreviousSequenceNumber.NumberValue}.");
#endif
                return retval;
            }
            else
            {
                // Yes
                return true;
            }
        }

        private bool ManageSequenceNumber1()
        {

            //return ManageSequenceNumber2();

            // Is Event ?
            if (_rxSequenceNumber != SequenceNumberEvent)
            {
                bool retval = false;

                UInt16 expectedSequenceNumber = (UInt16)((PreviousSequenceNumber1 + 1) % SequenceNumberWrap);
                Int16 diffSequenceNumber = (Int16)(expectedSequenceNumber - _rxSequenceNumber);

                // Is first message received?
                if (PreviousSequenceNumber1 == -1)
                {
                    PreviousSequenceNumber1 = _rxSequenceNumber;
                    retval = true;
                }
                else if (diffSequenceNumber == 0)
                {
                    PreviousSequenceNumber1 = _rxSequenceNumber;
                    retval = true;
                }
                else if (diffSequenceNumber < 0)
                {
                    // We mist a frame, log a warning but still accept this message and update the sequence number to avoid blocking the receiving of next messages.
                    Logger.Warn($"Missed frame(s), expected sequence number {expectedSequenceNumber}, received {_rxSequenceNumber}.");
                    PreviousSequenceNumber1 = _rxSequenceNumber;
                    retval = true;
                }
                else// if (diffSequenceNumber > 0)
                {
                    // This is a retransmission of the previous message, log a warning. 
                    Logger.Warn($"Received a retransmission of the previous message, expected sequence number {expectedSequenceNumber}, received {_rxSequenceNumber}.");
                    retval = false; // Do not update the sequence number, just ignore this message and wait for next message with correct sequence number.
                }

                return retval;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
