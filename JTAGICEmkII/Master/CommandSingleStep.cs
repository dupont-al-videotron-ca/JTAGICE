using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandSingleStep : CommandPCMode
    {


        #region Constructors 
        internal CommandSingleStep(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public StepModeEnum StepMode { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add StepMode bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)StepMode }).ToArray();
            MessageLength += 1; // Increment message length by 1 byte for the StepMode
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
