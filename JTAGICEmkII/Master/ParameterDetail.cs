using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class ParameterDetail
    {


        #region Constructors 

        internal ParameterDetail(ParameterEnum parameterId, uint parameterData)
        {
            this.ParameterId = parameterId;
            this.ParameterData = parameterData;
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public ParameterEnum ParameterId { get; set; }
        public UInt32 ParameterData { get; set; }


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public byte[] WriteToBytes()
        {
            var buffer = new byte[5];

            // Add ParameterId and ParameterData bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)ParameterId }).ToArray();
            buffer = buffer.Concat(BitConverter.GetBytes(ParameterData)).ToArray();
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
