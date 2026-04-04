using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    public interface IRxComAdaptor
    {
        bool ReadByte(out byte value, int timeout = -1);

        bool ReadBytes(out byte[]? values, uint length, int timeout = -1);

        Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1);

        Task<bool> ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout = -1);

    }
}
