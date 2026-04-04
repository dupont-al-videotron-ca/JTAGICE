using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    internal interface ITxComAdaptor
    {
        int SendBytes(byte[] values);

        Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken);

    }
}
