using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    public sealed class ResponseSignOn : Response
    {

        #region Constructors 

        internal ResponseSignOn(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        public const int SerialNumberSize = 6;
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
                for (int i = SerialNumber.Length - 1; i >= 0; i--)
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

        public override int Size => base.Size + SerialNumberSize + SerialNumberOffset;


        #endregion

        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(new byte[] { (byte)CommunicationProtocolVersion }).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) MasterMcuBootLoaderVersion}).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) MasterMcuFirmwareVersionMinor}).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) MasterMcuFirmwareVersionMajor}).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) MasterMcuHwVersion}).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) SlaveMcuBootLoaderVersion}).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) SlaveMcuFirmwareVersionMinor}).ToArray();
            buffer = buffer.Concat(new byte[] { (byte) SlaveMcuFirmwareVersionMajor}).ToArray();
            buffer = buffer.Concat(new byte[] {(byte) SlaveMcuHwVersion}).ToArray();
            
            buffer = buffer.Concat(SerialNumber).ToArray();
            buffer = buffer.Concat(DeviceId).ToArray();

            MessageLength = (uint)buffer.Length; 
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            CommunicationProtocolVersion = data[base.Size];
            MasterMcuBootLoaderVersion = data[base.Size + 1];
            MasterMcuFirmwareVersionMinor = data[base.Size + 2];
            MasterMcuFirmwareVersionMajor = data[base.Size + 3];
            MasterMcuHwVersion = data[base.Size + 4];
            SlaveMcuBootLoaderVersion = data[base.Size + 5];
            SlaveMcuFirmwareVersionMinor = data[base.Size + 6];
            SlaveMcuFirmwareVersionMajor = data[base.Size + 7];
            SlaveMcuHwVersion = data[base.Size + 8];

            Array.Copy(data, base.Size + 9, SerialNumber, 0, SerialNumberSize);

            DeviceId = new byte[data.Length - DeviceIdOffset];
            Array.Copy(data, DeviceIdOffset, DeviceId, 0, data.Length - DeviceIdOffset);
        }

        #endregion
    }
}
