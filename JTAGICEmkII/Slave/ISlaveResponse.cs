using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    public interface ISlaveResponse
    {
        void ReadFromBytes(byte[] data);
        byte[] WriteToBytes();

        SlaveResponseEnum ResponseId { get; }

        uint MessageLength { get; set; }

        int Size { get; }

        bool IsEvent { get; }

    }
}
