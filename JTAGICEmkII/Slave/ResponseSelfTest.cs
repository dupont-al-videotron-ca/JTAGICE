using System;
using System.Collections.Generic;
using System.Linq;
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


        #region Fields 

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public IEnumerable<SelfTestReponseEnum> SelfTestResults => Data.Select(b => (SelfTestReponseEnum)b);

        public SelfTestReponseEnum GetSelfTestResult(int index)
        {
            if (index < 0 || index >= Data.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be within the bounds of the data array.");

            return (SelfTestReponseEnum)Data[index];
        }

        internal override void ReadFromBytes(byte[] data)
        {
            if (data.Length < 9)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            Data.Clear();
            Data.AddRange(data.Skip(1));

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
