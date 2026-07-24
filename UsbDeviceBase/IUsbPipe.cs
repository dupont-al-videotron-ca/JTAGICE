using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDeviceBase
{
    public interface IUsbPipe : IDisposable
    {
        bool CanAquired();

        bool Aquired(TimeSpan timeout, CancellationToken cancellationToken);

        void Release();

        bool IsDisposed { get; }
    }
}
