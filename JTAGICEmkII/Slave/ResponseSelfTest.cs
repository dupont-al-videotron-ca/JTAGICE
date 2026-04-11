using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseSelfTest : ResponseMultipleByte
    {


        #region Constructors 
        internal ResponseSelfTest(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion

        #region Properties 
        private const int NbSelfTestResult = 8;

        public override int Size => base.Size + NbSelfTestResult;

        #endregion

        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            return base.WriteToBytes();
        }

        public IEnumerable<SelfTestReponseEnum> SelfTestResults => Data.Select(b => (SelfTestReponseEnum)b);

        public SelfTestReponseEnum GetSelfTestResult(int index)
        {
            if (index < 0 || index >= Data.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the data array.");

            return (SelfTestReponseEnum)Data[index];
        }

        public void SetSelfTestResult(int index, SelfTestReponseEnum result)
        {
            if (index < 0 || index >= Data.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the data array.");

            Data[index] = (byte)result;
        }

        public override void ReadFromBytes(byte[] data)
        {
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            base.ReadFromBytes(data);
        }


        #endregion
    }
}
