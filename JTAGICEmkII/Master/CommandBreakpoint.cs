using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandBreakpoint : CommandBreakAddress
    {
        #region Constructors 
        internal CommandBreakpoint(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public BreakpointTypeEnum Type { get; set; }
        public BreakpointModeEnum Mode { get; set; }

        public override byte[] WriteToBytes()
        {
            var buffer = new byte[3];
            buffer[0] = (byte)MessageId;
            buffer[1] = (byte)Type;
            buffer[2] = BreakNumber;

            // Add Address bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();

            buffer = buffer.Concat(new byte[] { (byte)Mode }).ToArray();
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
