using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseMultipleByte : Response
    {

        #region Constructors 
        internal ResponseMultipleByte(SlaveResponseEnum messageId) : base(messageId)
        {
            Data = new List<byte>();
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        protected List<byte> Data { get; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < 2)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            Data.Clear();
            Data.AddRange(data.Skip(1));
        }

        public byte[] GetDataBytes()
        {
            return Data.ToArray();
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
