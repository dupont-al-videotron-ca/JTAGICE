using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseMultipleByte : Response
    {

        #region Constructors 
        internal ResponseMultipleByte(SlaveResponseEnum messageId) : base(messageId)
        {
            Data = new List<byte>();
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        internal List<byte> Data { get; }
        public override int Size => base.Size + Data.Count; 

        #endregion

        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(Data).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            Data.Clear();
            Data.AddRange(data.Skip(base.Size));
        }

        public byte[] GetDataBytes()
        {
            return Data.ToArray();
        }

        #endregion
    }
}
