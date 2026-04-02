using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandBreakAddress: CommandBreakNumber
     {
        #region Constructors 
        internal CommandBreakAddress(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public UInt32 Address { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add Address bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();
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
