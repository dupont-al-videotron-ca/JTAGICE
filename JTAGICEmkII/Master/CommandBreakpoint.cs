using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandBreakpoint : CommandBreakAddress
    {
        #region Constructors 
        internal CommandBreakpoint(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public BreakpointTypeEnum Type { get; set; }
        public BreakpointModeEnum Mode { get; set; }

        override public int Size => base.Size + 2;

        public override byte[] WriteToBytes()
        {
            var buffer = new byte[3];
            buffer[0] = (byte)MessageId;
            buffer[1] = (byte)Type;
            buffer[2] = BreakNumber;

            // Add Address bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();

            buffer = buffer.Concat(new byte[] { (byte)Mode }).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            if (data == null || data.Length < Size)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            MessageId = (MasterCommandEnum)data[0];
            Type = (BreakpointTypeEnum)data[1];
            BreakNumber = data[2];
            Address = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(3, 4));
            Mode = (BreakpointModeEnum)data[7];
        }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
