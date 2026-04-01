#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using MemoryPack;
using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public class BulkInPipeImpl : PipeInBase
    {

        #region Constructors 
        public BulkInPipeImpl(Windows.Devices.Usb.UsbEndpointDescriptor descriptor, Windows.Devices.Usb.UsbBulkInPipe pipe) : base(descriptor)
        {
            this.InPipe = pipe;
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public Windows.Devices.Usb.UsbBulkInPipe InPipe { get; private set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public async Task<T> Read<T>() where T : struct
        {
            uint readLen = (uint)Unsafe.SizeOf<T>();

            using (DataReader dataReader = new DataReader(InPipe.InputStream))
            {
                dataReader.ByteOrder = this.ByteOrder;
                dataReader.UnicodeEncoding = this.UnicodeEncoding;

                await dataReader.LoadAsync(readLen);

                byte[] buf = new byte[readLen];
                dataReader.ReadBytes(buf);

                dataReader.DetachStream();
                T retval = MemoryPackSerializer.Deserialize<T>(buf, this.SerializerOptions);

                return retval;
            }
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Provate Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

        public static IBuffer StructToBuffer<T>(T data)
        where T : notnull
        {
            byte[] bytes = MemoryPackSerializer.Serialize(data);
            var val = MemoryPackSerializer.Deserialize<T>(bytes);

            IBuffer buffer = WindowsRuntimeBuffer.Create(bytes, 0, bytes.Length, bytes.Length);

            return buffer;
        }

    }
}
