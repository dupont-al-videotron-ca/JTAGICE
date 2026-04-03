using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace JTAGICEmkII.Slave
{
    internal class ResponseBreakpoint : Response
    {


        #region Constructors 
        internal ResponseBreakpoint(SlaveResponseEnum messageId) : base(messageId)
        {

        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public BreakpontTypeEnum BreakpontType { get; set; }

        public UInt32 Address { get; set; }

        public BreakpointModeEnum BreakpointMode { get; set; }

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < 6)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            BreakpontType = (BreakpontTypeEnum)data[1];
            Address = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(2, 4));
            BreakpointMode = (BreakpointModeEnum)data[6];
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
