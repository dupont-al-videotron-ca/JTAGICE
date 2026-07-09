using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseBreakpoint : Response
    {


        #region Constructors 
        internal ResponseBreakpoint(SlaveResponseEnum messageId) : base(messageId)
        {

        }

        #endregion

        #region Properties 

        public BreakpointTypeEnumS BreakpontType { get; set; }
        public UInt32 Address { get; set; }
        public BreakpointModeEnumS BreakpointMode { get; set; }
        public override int Size => base.Size + 6;

        #endregion

        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(new byte[] { (byte)BreakpontType }).ToArray();
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();
            buffer = buffer.Concat(new byte[] { (byte)BreakpointMode}).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            BreakpontType = (BreakpointTypeEnumS)data[base.Size];
            Address = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(base.Size+1, 4));
            BreakpointMode = (BreakpointModeEnumS)data[base.Size+5];
        }

        #endregion
    }
}
