using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseProgramCounter : Response
    {


        #region Constructors 

        internal ResponseProgramCounter(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public UInt32 ProgramCounter { get; set; }

        public override int Size => base.Size + 4; 

        #endregion

        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(BitConverter.GetBytes(ProgramCounter)).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data) 
        { 
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ProgramCounter = BitConverter.ToUInt32(data, base.Size);
        }

        #endregion
    }
}
