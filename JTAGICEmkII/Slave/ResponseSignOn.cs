using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.AppBroadcasting;

namespace JTAGICEmkII.Slave
{
    internal class ResponseSignOn : Response
    {

        #region Constructors 

        internal ResponseSignOn(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        private const int SerialNumberSize = 6;
        private const int SerialNumberOffset = 10;
        private const int DeviceIdOffset = SerialNumberOffset + SerialNumberSize;

        #endregion


        #region Properties 

        public byte CommunicationProtocolVersion { get; set; }
        public byte MasterMcuBootLoaderVersion { get; set; }

        public byte MasterMcuHwVersion { get; set; }

        public byte MasterMcuFirmwareVersionMajor { get; set; }

        public byte MasterMcuFirmwareVersionMinor { get; set; }

        public byte SlaveMcuBootLoaderVersion { get; set; }

        public byte SlaveMcuFirmwareVersionMajor { get; set; }

        public byte SlaveMcuFirmwareVersionMinor { get; set; }

        public byte SlaveMcuHwVersion { get; set; }

        public byte[] SerialNumber { get; set; } = new byte[SerialNumberSize];

        // The Serial Number is stored in reverse order, so we need to reverse it back to get the correct string representation.
        public string SerialNumberString
        {
            get
            {
                var retval = string.Empty;
                for (int i = SerialNumber.Length-1; i >= 0; i--)
                {
                    retval += String.Join(string.Empty, SerialNumber[i].ToString());
                }
                return retval;
            }
        }

        public string DeviceIdString
        {
            get
            {
                Char[] characters = new Char[DeviceId.Length - 1];
                for (int i = 0; i < DeviceId.Length; i++)
                {

                    if (DeviceId[i] == 0x00)
                        break;

                    characters[i] = (Char)DeviceId[i];
                }
                return new string(characters);
            }
        }

        public byte[] DeviceId { get; set; } = new byte[1];

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < DeviceIdOffset)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            CommunicationProtocolVersion = data[1];
            MasterMcuBootLoaderVersion = data[2];
            MasterMcuFirmwareVersionMinor = data[3];
            MasterMcuFirmwareVersionMajor = data[4];
            MasterMcuHwVersion = data[5];
            SlaveMcuBootLoaderVersion = data[6];
            SlaveMcuFirmwareVersionMinor = data[7];
            SlaveMcuFirmwareVersionMajor = data[8];
            SlaveMcuHwVersion = data[9];

            Array.Copy(data, SerialNumberOffset, SerialNumber, 0, SerialNumberSize);

            DeviceId = new byte[data.Length - DeviceIdOffset];
            Array.Copy(data, DeviceIdOffset, DeviceId, 0, data.Length - DeviceIdOffset);
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
