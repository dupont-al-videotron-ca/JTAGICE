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

        SlaveResponseEnum ResponseId { get; }

    }
}
