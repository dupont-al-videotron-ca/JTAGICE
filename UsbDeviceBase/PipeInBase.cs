using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDeviceBase
{
    public abstract class PipeInBase : UsbPipeBase, IUsbPipeIn
    {
        public PipeInBase(Windows.Devices.Usb.UsbEndpointDescriptor descriptor):base(descriptor)
        {
        }

        public abstract bool IsByteToRead { get; }

        public abstract bool ReadBytes(out byte[] values, uint length, int timeout = -1);
        public abstract Task<bool> ReadBytesAsync(out byte[] values, uint length, CancellationToken cancellationToken, int timeout = -1);

        public abstract bool ReadStructure<T>(ref T obj, int timeout) where T : struct;
    }
}
