using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandAddress : Command
    {
        internal CommandAddress(MasterCommandEnum messageId) : base(messageId)
        {
        }

        public UInt32 Address { get; set; }

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            // Add Address bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();
            return buffer;
        }
    }
}
