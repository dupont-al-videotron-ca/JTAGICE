using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;

namespace JTAGICEmkII
{
    internal class Command : IMasterCommand
    {

        #region Constructors 
        internal Command(MasterCommandEnum messageId)
        {
            MessageId = messageId;
            MessageLength = 1; // Default length is 1 byte for the message ID
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public MasterCommandEnum MessageId { get; private set; }

        public int MessageLength { get; protected set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public virtual byte[] WriteToBytes()
        {
            return new byte[] { (byte)MessageId };
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
