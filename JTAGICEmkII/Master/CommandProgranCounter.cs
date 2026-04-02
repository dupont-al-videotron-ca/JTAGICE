using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandProgranCounter : Command
    {
        #region Constructors 
        internal CommandProgranCounter(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public UInt32 ProgrammeCounter { get; set; }

        public override byte[] WriteToBytes()
        { 
            var buffer = base.WriteToBytes();

            // Add ProgrammeCounter bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(ProgrammeCounter)).ToArray();
            MessageLength += 4; // Increment message length by 4 bytes for the ProgrammeCounter
            return buffer;
        } 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 



        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
