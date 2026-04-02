using System;
using System.Collections.Generic;
using System.Linq;
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


        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
