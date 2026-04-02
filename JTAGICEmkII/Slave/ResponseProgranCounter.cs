using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseProgranCounter : Response
    {


        #region Constructors 

        internal ResponseProgranCounter(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public UInt32 ProgramCounter { get; set; }


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        internal override void ReadFromBytes(byte[] data) 
        { 
            base.ReadFromBytes(data);
            if (data.Length < 5)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ProgramCounter = BitConverter.ToUInt32(data, 1);
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
