using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{

    internal class CommandNParameter : Command
    {

        #region Constructors 
        internal CommandNParameter(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        internal byte NumberOfParameters { get; set; }

        internal List<ParameterDetail> Parameters { get; } = new List<ParameterDetail>();

        public override int Size => base.Size + 1 ; // 1 byte for NumberOfParameters and 5 bytes for each parameter (1 byte for ParameterId and 4 bytes for ParameterData)

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            // Add NumberOfParameters bytes to the buffer
            NumberOfParameters = (byte) Parameters.Count;
            buffer = buffer.Concat(new byte[] { NumberOfParameters }).ToArray();
            // Add each parameter's bytes to the buffer
            foreach (var parameter in Parameters)
            {
                buffer = buffer.Concat(parameter.WriteToBytes()).ToArray();
            }

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            NumberOfParameters = data[base.Size];
            
            if (data.Length < Size +(NumberOfParameters * ParameterDetail.Size))
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");
            
            for (int i = 0; i < NumberOfParameters; i++)
            {
                var p = new ParameterDetail();
                p.ReadFromBytes(data.AsSpan(Size + (i * ParameterDetail.Size), ParameterDetail.Size));
                AddParameter(p);
            }
        }

        public void AddParameter(ParameterEnum parameterId, UInt32 parameterData)
        {
            Parameters.Add(new ParameterDetail(parameterId, parameterData));
        }

        public void AddParameter(IEnumerable<ParameterDetail> parameterDetails)
        {
            Parameters.AddRange(parameterDetails);
        }

        public void AddParameter(ParameterDetail parameterDetail)
        {
            Parameters.Add(parameterDetail);
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
