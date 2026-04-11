using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandParameter : CommandMultipleByte
    {
        #region Constructors 
        internal CommandParameter(MasterCommandEnum messageId) : base(messageId, false)
        {
        }


        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public ParameterEnum ParameterId { get; set; }

        public override int Size => base.Size+1;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add ParameterId bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)ParameterId }).ToArray();
            MessageLength += 1;
            buffer = buffer.Concat(WriteDataToBytes()).ToArray();
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ParameterId = (ParameterEnum)data[base.Size];
            this.ReadDataToBytes(data, base.Size + 1);
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
