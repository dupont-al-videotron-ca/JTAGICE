using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    internal class CommandMultipleByte : Command
    {
        #region Constructors 

        internal CommandMultipleByte(MasterCommandEnum messageId) : this(messageId, true)
        {
        }

        protected CommandMultipleByte(MasterCommandEnum messageId, bool immediateDataFlg) : base(messageId)
        {
            Data = new List<byte>();
            this.ImmediateDataFlg = immediateDataFlg;
        }

        #endregion


        #region Fields 

        private readonly bool ImmediateDataFlg;

        #endregion


        #region Properties 

        public List<byte> Data { get; }

        public override int Size
        {
            get
            {
                if (Data.Count != 0)
                {
                    return base.Size + Data.Count;
                }
                else
                {
                    return base.Size;
                }
            }
        }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            if (ImmediateDataFlg)
            {
                buffer = buffer.Concat(WriteDataToBytes()).ToArray();
                MessageLength = (uint) buffer.Length;
            }

            return buffer;
        }

        protected byte[] WriteDataToBytes()
        {
            var buffer = Data.ToArray();
            MessageLength += (uint)Data.Count;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            if (ImmediateDataFlg)
            {
                ReadDataToBytes(data, base.Size);
            }
        }

        protected void ReadDataToBytes(byte[] data, int offset)
        {
            Data.AddRange(data.Skip(offset)); 
        }
        #endregion
    }
}
