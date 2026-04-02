using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandMultipleByte : Command
    {
        #region Constructors 

        internal CommandMultipleByte(MasterCommandEnum messageId) : this(messageId, true)
        {
        }

        protected CommandMultipleByte(MasterCommandEnum messageId, bool writeDataFlg) : base(messageId)
        {
            Data = new List<byte>();
            this.writeDataFlg = writeDataFlg;
        }

        #endregion


        #region Fields 

        private readonly bool writeDataFlg;

        #endregion


        #region Properties 

        public List<byte> Data { get; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            if(writeDataFlg)
            {
                buffer = buffer.Concat(Data).ToArray();
                MessageLength += (byte)Data.Count;
            }

            return buffer;
        }

        protected byte[] WriteDataToBytes()
        {
            var buffer = Data.ToArray();
            MessageLength += (byte)Data.Count;
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
