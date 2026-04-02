using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandBreakNumber  : Command
     {
        #region Constructors 
        internal CommandBreakNumber(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion

        #region Fields 

        #endregion


        #region Properties 

        public byte BreakNumber { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add BreakNumber bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)BreakNumber }).ToArray();
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
