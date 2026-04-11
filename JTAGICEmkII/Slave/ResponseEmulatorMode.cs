using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEmulatorMode : Response
    {

        #region Constructors 
        internal ResponseEmulatorMode(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion

        #region Properties 

        public EmulatorModeEnum EmulatorMode { get; set; }
        public override int Size => base.Size+1;

        #endregion

        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(new byte[] { (byte)EmulatorMode }).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            EmulatorMode = (EmulatorModeEnum)data[base.Size];
        }

        #endregion
    }
}
