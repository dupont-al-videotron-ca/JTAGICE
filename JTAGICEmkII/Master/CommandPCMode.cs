using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandPCMode : Command
    {

        #region Constructors 
        internal CommandPCMode(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public ExceutionModeEnum ExecutionMode { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add ExecutionMode bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)ExecutionMode }).ToArray();
            MessageLength += 1; // Increment message length by 1 byte for the ExecutionMode
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
