
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    internal class CommandSingleStep : CommandPCMode
    {


        #region Constructors 
        internal CommandSingleStep(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Properties 

        public StepModeEnum StepMode { get; set; }

        public override int Size => base.Size+1; // Size of base command + 1 byte for StepMode

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add StepMode bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)StepMode }).ToArray();
            MessageLength += 1; // Increment message length by 1 byte for the StepMode
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            StepMode = (StepModeEnum)data[base.Size];
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
