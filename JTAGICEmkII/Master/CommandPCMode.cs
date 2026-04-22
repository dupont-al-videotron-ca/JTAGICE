using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    internal class CommandPCMode : Command
    {

        #region Constructors 
        internal CommandPCMode(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public ExceutionModeEnum ExecutionMode { get; set; }

        public override int Size => base.Size + 1; // Base size plus 1 byte for ExecutionMode
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add ExecutionMode bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)ExecutionMode }).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }
        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ExecutionMode = (ExceutionModeEnum)data[base.Size];
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
