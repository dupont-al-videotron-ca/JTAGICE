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
    internal abstract class RxFrame : IRxFrame, IDisposable
    {
        #region Constructors 

        internal RxFrame()
        {
            Logger = LogManager.GetLogger(this.GetType());
            state = RxStateEnum.WaitStart;
            rxTimer = new SingleShotTimer();
            rxTimer.TimerExpired += this._rxTimer_TimerExpired;

            source = new CancellationTokenSource();
            token = source.Token;

            PreviousSequenceNumber = -1;
            Timeout = 1000; // Default timeout of 1000 milliseconds
        }


        #endregion


        #region Fields 
        private const byte ESCAPE_BYTE = 27;
        private const byte TOKEN_BYTE = 14;
        private const UInt16 SequenceNumberWrap = 0xFFFF;
        private const UInt16 SequenceNumberEvent = 0xFFFF;

        private RxStateEnum state;
        private SingleShotTimer rxTimer;
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

        protected ILog Logger { get; private set; }

        public bool IsReceiving => receiveTask != null && !receiveTask.IsCompleted;

        /// <summary>
        /// Timeout in milliseconds for receiving each part of the frame (e.g., waiting for start byte, sequence number, token, message bytes, CRC).
        /// </summary>
        internal int Timeout { get; set; }

        protected CancellationToken CancellationToken => Token;

        public CancellationToken Token { get => this.token; }

        #endregion


        #region Delegates / Events 

        public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
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
                    if (this.Token.IsCancellationRequested)
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
        protected internal abstract byte GetByte();

        protected internal abstract byte[] GetBytes(uint length);

        protected internal abstract Task<byte[]> GetBytesAsync(uint length, CancellationToken cancellationToken);

        protected internal abstract Task<byte> GetByteAsync(CancellationToken cancellationToken);

        protected virtual void OnRxTimerExpired()
        {
            RxTimerExpired?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnReceiveReponse(ISlaveResponse response)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(response));
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    rxTimer.TimerExpired -= this._rxTimer_TimerExpired;
                    rxTimer.Dispose();
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
                rxTimer.Start(Timeout);
            }
        }

        private void GoWaitSequenceNumber()
        {
            if (state != RxStateEnum.WaitSequenceNumber)
            {
                Logger.Debug($"GoWaitSequenceNumber.");
                state = RxStateEnum.WaitSequenceNumber;
                rxTimer.Start(Timeout);
            }
        }

        private void GoWaitToken()
        {
            if (state != RxStateEnum.WaitToken)
            {
                Logger.Debug($"GoWaitToken.");
                state = RxStateEnum.WaitToken;
                rxTimer.Start(Timeout);
            }
        }

        private void GoWaitMessageSize()
        {
            if (state != RxStateEnum.WaitMessageSize)
            {
                Logger.Debug($"GoWaitMessageSize.");
                state = RxStateEnum.WaitMessageSize;
                rxTimer.Start(Timeout);
            }
        }

        private void GoWaitMessage()
        {
            if (state != RxStateEnum.WaitMessage)
            {
                Logger.Debug($"GoWaitMessage.");
                state = RxStateEnum.WaitMessage;
                rxTimer.Start(Timeout);
            }
        }

        private void GoWaitCRC()
        {
            if (state != RxStateEnum.WaitCRC)
            {
                Logger.Debug($"GoWaitCRC.");
                state = RxStateEnum.WaitCRC;
                rxTimer.Start(Timeout);
            }
        }

        private void GoStop()
        {
            if (state != RxStateEnum.Stop)
            {
                Logger.Debug($"GoStop.");
                state = RxStateEnum.Stop;
                rxTimer.Stop();
            }
        }

        private void _rxTimer_TimerExpired(object? sender, EventArgs e)
        {
            switch (state)
            {
                case RxStateEnum.WaitStart:
                    GoWaitStart();
                    break;
                case RxStateEnum.WaitSequenceNumber:
                    // Handle timeout for waiting for sequence number
                    GoWaitStart();
                    break;
                case RxStateEnum.WaitToken:
                    // Handle timeout for waiting for token
                    GoWaitStart();
                    break;
                case RxStateEnum.WaitMessage:
                    // Handle timeout for waiting for message bytes
                    GoWaitStart();
                    break;
                case RxStateEnum.WaitCRC:
                    // Handle timeout for waiting for CRC bytes
                    GoWaitStart();
                    break;
                case RxStateEnum.Stop:
                    GoStop();
                    break;
                default:
                    // Handle unexpected state
                    GoWaitStart();
                    break;
            }

            OnRxTimerExpired();
        }

        private void DispathMessageBody(List<byte> messageBuffer)
        {
            try
            {
                SlaveResponseEnum responseId = (SlaveResponseEnum)messageBuffer[0]; // Assuming the response id is at index 0
                ISlaveResponse? response = Slave.ResponseFactory.CreateResponse(responseId);

                if (response != null)
                {
                    response.ReadFromBytes(messageBuffer.ToArray());
                    // Now you can use the 'response' object as needed
                    OnReceiveReponse(response);
                }
                else
                {
                    Logger.Fatal($"Handle unknown response id: {responseId}.");
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

            byte rxbyte;
            switch (state)
            {
                case RxStateEnum.Stop:
                    // Do nothing, just wait for stop state to be reset
                    break;

                case RxStateEnum.WaitStart:
                    rxbyte = GetByte();
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
                    var rxBytes = GetBytes(2); // sequence number is 2 bytes
                    if (rxBytes.Length == 2)
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
                    var rxBytes2 = GetBytes(4);
                    if (rxBytes2.Length == 4) // message size 
                    {
                        frameBuffer.AddRange(rxBytes2); // Add the message size bytes to the frame buffer

                        // LSB is first byte, MSB is last byte
                        messageLength = BinaryPrimitives.ReadUInt32LittleEndian(rxBytes2);

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
                    rxbyte = GetByte();
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
                    byte[] messageBytes = GetBytes(messageLength); // 
                    if (messageBytes.Length == messageLength)
                    {
                        Logger.Debug($"Received message bytes, length: {messageBytes.Length}.");
                        frameBuffer.AddRange(messageBytes); // Add the message bytes to the frame buffer
                        messageBuffer.AddRange(messageBytes); // Store the message bytes separately   
                        GoWaitCRC();
                    }
                    else
                    {
                        Logger.Error($"Handle error in reading message bytes. length: {messageBytes.Length}, expected: {messageLength}.");
                        GoWaitStart();
                    }
                    break;
                case RxStateEnum.WaitCRC:
                    byte[] crcBytes = GetBytes(2); // CRC is 2 bytes
                    if (crcBytes.Length == 2)
                    {
                        frameBuffer.AddRange(crcBytes); // Add the CRC bytes to the frame buffer
                        if (Crc16.ValidateCrc(frameBuffer.ToArray()))
                        {
                            Logger.Debug($"CRC valid: 0x{crcBytes[1]:x2}{crcBytes[0]:x2}.");

                            if (ManageSequenceNumber())
                            {
                                // Accept the message and dispatch it for processing
                                DispathMessageBody(messageBuffer);
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
