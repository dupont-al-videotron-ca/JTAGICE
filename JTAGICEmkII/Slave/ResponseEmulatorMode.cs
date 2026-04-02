using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEmulatorMode : Response
    {

        #region Constructors 
        internal ResponseEmulatorMode(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public EmulatorModeEnum EmulatorMode { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < 2)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            EmulatorMode = (EmulatorModeEnum)data[1];
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
