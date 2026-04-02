using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEventBreak : ResponseEvent
    {

        #region Constructors 

        internal ResponseEventBreak(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public UInt32 ProgramCounter { get; set; }
        
        public EventBreakCauseEnum BreakCause { get; set; }

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < 6)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ProgramCounter = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(1, 4));
            BreakCause = (EventBreakCauseEnum)data[5];
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
