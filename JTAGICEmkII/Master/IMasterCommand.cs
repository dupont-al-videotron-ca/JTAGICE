using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    public interface IMasterCommand 
    {

        void ReadFromBytes(byte[] data);
        byte[] WriteToBytes();

        MasterCommandEnum MessageId { get; }

        uint MessageLength { get; set; }
    }
}
