using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
        protected byte NumberOfParameters { get; set; }

        internal List<ParameterDetail> Parameters { get; } = new List<ParameterDetail>();

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
            return buffer;
        }

        public void AddParameter(ParameterEnum parameterId, UInt32 parameterData)
        {
            Parameters.Add(new ParameterDetail(parameterId, parameterData));
        }

        public void AddParameter(IEnumerable<ParameterDetail> parameterDetails)
        {
            Parameters.AddRange(parameterDetails);
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
