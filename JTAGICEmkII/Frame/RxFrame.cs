#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
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

namespace JTAGICEmkII
{
    public sealed class RxFrame : IRxFrame, IDisposable
    {
        #region Constructors 

        internal RxFrame(IRxFrameAdaptor rxComAdaptor)
        {
            Logger = LogManager.GetLogger(this.GetType());
            state = RxStateEnum.WaitStart;

            source = new CancellationTokenSource();
            token = source.Token;

            PreviousSequenceNumber = -1;
            Timeout = 1000; // Default _timeout of 1000 milliseconds
            this.rxFrameAdaptor = rxComAdaptor ?? throw new ArgumentNullException(nameof(rxComAdaptor));
        }


        #endregion


        #region Fields 
        private const byte ESCAPE_BYTE = 27;
        private const byte TOKEN_BYTE = 14;
        private const UInt16 SequenceNumberWrap = 0xFFFF;
        private const UInt16 SequenceNumberEvent = 0xFFFF;
        private readonly IRxFrameAdaptor rxFrameAdaptor;
        private RxStateEnum state;
        private bool disposedValue;
        private Task receiveTask;
        private UInt32 messageLength;

        private CancellationTokenSource source;
        private CancellationToken token;
        private List<byte> frameBuffer = new List<byte>();
        private List<byte> messageBuffer = new List<byte>();
        private UInt16 rxSequenceNumber;

        #endregion


        #region Properties 

        internal int PreviousSequenceNumber { get; set; }

        internal bool TimeoutOccured { get; set; }

        internal ILog Logger { get; private set; }

        public bool IsReceiving => receiveTask != null && !receiveTask.IsCompleted;

        public IRxFrameAdaptor Adaptor => rxFrameAdaptor;

        /// <summary>
        /// Timeout in milliseconds for receiving each part of the frame (e.g., waiting for start byte, sequence number, token, message bytes, CRC).
        /// </summary>
        internal int Timeout { get; set; }

        internal CancellationToken CancellationToken { get => this.token; }

        #endregion


        #region Delegates / Events 

        public event EventHandler<ResponseReceivedEventArgs>? ResponceReceived;
        public event EventHandler<CommandReceivedEventArgs>? CommandReceived;
        public event EventHandler? RxTimerExpired;

        #endregion


        #region Public Methods 

        public void StartReceiving()
        {
            Logger.Debug("receiving start.");
            this.receiveTask = Task.Run(() =>
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
            }, this.source.Token);
        }

        private void ResetReceiver()
        {
            Logger.Debug($"ResetReceiver.");
            PreviousSequenceNumber = -1;
            GoWaitStart();
        }

        public void StopReceiving()
        {
            try
            {
                Logger.Debug("Receiving Task stop request.");
                GoStop();
                source.Cancel(true);
                receiveTask?.Wait(100);
                receiveTask = null;
                Logger.Debug("Stop terminated.");
            }
            catch (OperationCanceledException ex)
            {
                Logger.Debug($"Stop OperationCanceledException: {ex.Message}");
            }
        }
        #endregion


        #region Protected Methods 

        internal bool ReadByte(out byte value, int timeout = -1)
            => rxFrameAdaptor.ReadByte(out value, timeout);

        internal bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
            => rxFrameAdaptor.ReadBytes(out values, length, timeout);

        internal Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
            => rxFrameAdaptor.ReadBytesAsync(out values, length, cancellationToken, timeout);

        internal Task<bool> ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout = -1) =>
            rxFrameAdaptor.ReadByteAsync(out value, cancellationToken, timeout);

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
            ResponceReceived?.Invoke(this, new ResponseReceivedEventArgs(response));
        }

        internal void OnCommandReceived(Master.IMasterCommand command)
        {
            CommandReceived?.Invoke(this, new CommandReceivedEventArgs(command));
        }

        internal void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    source.Cancel();
                    source.Dispose();
                    receiveTask?.Wait();
                    receiveTask?.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
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
            if (state != RxStateEnum.WaitStart)
            {
                Logger.Debug($"GoWaitStart.");
                frameBuffer.Clear();
                messageBuffer.Clear();
                state = RxStateEnum.WaitStart;
            }
        }

        private void GoWaitSequenceNumber()
        {

            if (state != RxStateEnum.WaitSequenceNumber)
            {
                Logger.Debug($"GoWaitSequenceNumber.");
                state = RxStateEnum.WaitSequenceNumber;
            }
        }

        private void GoWaitToken()
        {
            if (state != RxStateEnum.WaitToken)
            {
                Logger.Debug($"GoWaitToken.");
                state = RxStateEnum.WaitToken;
            }
        }

        private void GoWaitMessageSize()
        {
            if (state != RxStateEnum.WaitMessageSize)
            {
                Logger.Debug($"GoWaitMessageSize.");
                state = RxStateEnum.WaitMessageSize;
            }
        }

        private void GoWaitMessage()
        {
            if (state != RxStateEnum.WaitMessage)
            {
                Logger.Debug($"GoWaitMessage.");
                state = RxStateEnum.WaitMessage;
            }
        }

        private void GoWaitCRC()
        {
            if (state != RxStateEnum.WaitCRC)
            {
                Logger.Debug($"GoWaitCRC.");
                state = RxStateEnum.WaitCRC;
            }
        }

        private void GoStop()
        {
            if (state != RxStateEnum.Stop)
            {
                Logger.Debug($"GoStop.");
                state = RxStateEnum.Stop;
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
            // read bytes according to the current state, and update the state machine accordingly.
            // The read bytes will be stored in frameBuffer, and the message body bytes will be stored in messageBuffer.
            byte rxbyte;
            byte[]? rxBytes = null;
            switch (state)
            {
                case RxStateEnum.Stop:
                    // Do nothing, just wait for stop state to be reset
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
                        Logger.Debug("Start byte received.");
                        frameBuffer.Add(rxbyte); // Add the escape byte to the frame buffer
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
                        frameBuffer.AddRange(rxBytes);

                        // LSB is first byte, MSB is second byte
                        rxSequenceNumber = BinaryPrimitives.ReadUInt16LittleEndian(rxBytes);
                        Logger.Debug($"Sequence number bytes {rxSequenceNumber}.");
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
                        frameBuffer.AddRange(rxBytes); // Add the message size bytes to the frame buffer

                        // LSB is first byte, MSB is last byte
                        messageLength = BinaryPrimitives.ReadUInt32LittleEndian(rxBytes);

                        Logger.Debug($"message Length:{messageLength:x4}.");
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
                        Logger.Debug($"Received TOKEN.");
                        frameBuffer.Add(rxbyte); // Add the token byte to the frame buffer
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
                        Logger.Debug($"Received message bytes, length: {messageBytes.Length}.");
                        frameBuffer.AddRange(messageBytes); // Add the message bytes to the frame buffer
                        messageBuffer.AddRange(messageBytes); // Store the message bytes separately   
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
                        frameBuffer.AddRange(crcBytes); // Add the CRC bytes to the frame buffer
                        if (Crc16.ValidateCrc(frameBuffer.ToArray()))
                        {
                            Logger.Debug($"CRC valid: 0x{crcBytes[1]:x2}{crcBytes[0]:x2}.");

                            if (ManageSequenceNumber())
                            {
                                // Accept the message and dispatch it for processing
                                Logger.Debug($"Message length: {messageLength} & messageBuffer.count: {messageBuffer.Count}.");
                                DispathMessageBody(messageBuffer, messageLength);
                            }
                            // else, allredy received, just ignore this message and wait for next message with correct sequence number.

                            GoWaitStart();
                        }
                        else
                        {
                            Logger.Error($"Handle CRC validation failure: 0x{crcBytes[1]:x2}{crcBytes[0]:x2}.");
                            GoWaitStart();
                        }
                    }
                    break;
                default:
                    // Handle unexpected state
                    Logger.Error($"Invalid State: {state}.");
                    GoWaitStart();
                    break;
            }
        }

        private bool ManageSequenceNumber()
        {
            // Is Event ?
            if (rxSequenceNumber != SequenceNumberEvent)
            {
                bool retval = false;
                UInt16 expectedSequenceNumber = (UInt16)((PreviousSequenceNumber + 1) % SequenceNumberWrap);
                Int16 diffSequenceNumber = (Int16)(expectedSequenceNumber - rxSequenceNumber);

                // Is first message received?
                if (PreviousSequenceNumber == -1)
                {
                    PreviousSequenceNumber = rxSequenceNumber;
                    retval = true;
                }
                else if (diffSequenceNumber == 0)
                {
                    PreviousSequenceNumber = rxSequenceNumber;
                    retval = true;
                }
                else if (diffSequenceNumber < 0)
                {
                    // We mist a frame, log a warning but still accept this message and update the sequence number to avoid blocking the receiving of next messages.
                    Logger.Warn($"Missed frame(s), expected sequence number {expectedSequenceNumber}, received {rxSequenceNumber}.");
                    PreviousSequenceNumber = rxSequenceNumber;
                    retval = true;
                }
                else// if (diffSequenceNumber > 0)
                {
                    // This is a retransmission of the previous message, log a warning. 
                    Logger.Warn($"Received a retransmission of the previous message, expected sequence number {expectedSequenceNumber}, received {rxSequenceNumber}.");
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
