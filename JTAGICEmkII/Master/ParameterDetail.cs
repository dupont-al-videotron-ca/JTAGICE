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
    internal class ParameterDetail
    {


        #region Constructors 

        internal ParameterDetail(ParameterEnum parameterId, uint parameterData)
        {
            this.ParameterId = parameterId;
            this.ParameterData = parameterData;
        }

        internal ParameterDetail()
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public ParameterEnum ParameterId { get; set; }
        public UInt32 ParameterData { get; set; }

        public static int Size => 1 + 4; // 1 byte for ParameterId and 4 bytes for ParameterData
        
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public byte[] WriteToBytes()
        {
            var buffer = new byte[Size];

            // Add ParameterId and Value bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)ParameterId }).ToArray();
            buffer = buffer.Concat(BitConverter.GetBytes(ParameterData)).ToArray();
            return buffer;
        }

        public void ReadFromBytes(ReadOnlySpan<byte> data)
        {
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ParameterId = (ParameterEnum)data[0];
            ParameterData = BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(1, 4));
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
