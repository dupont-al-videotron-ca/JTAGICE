#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.SafeHandles;
using Serilog;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using Windows.Win32;
using Windows.Devices.Usb;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.Properties;
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using static CsWin32Api.WinUsbApi;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    public class WinUsbDeviceApi : IDisposable
    {
        private bool disposedValue;
        private winmdroot.WinUsb_FreeSafeHandle _usbDeviceHandle;
        private byte _associatedInterfaceIndex;
        private ILog _logger;

        internal WinUsbDeviceApi(winmdroot.WinUsb_FreeSafeHandle deviceHandle, byte associatedInterfaceIndex = 0)
        {
            _usbDeviceHandle = deviceHandle ?? throw new ArgumentNullException(nameof(deviceHandle));
            _associatedInterfaceIndex = associatedInterfaceIndex;
            _logger = LogManager.GetLogger(GetType());

            var selector = Windows.Devices.Usb.UsbDevice.GetDeviceSelector(0x03EB, 0x0001);
        }

        // properties
        public bool IsClosed => _usbDeviceHandle.IsClosed;

        // methods
        public WinUsbPipeApi WinUsbPipeApiFactory(uint pipeId)
        {
            var handle = _usbDeviceHandle;
            if (this._associatedInterfaceIndex != 0)
            {
                // TODO: finish implementing this factory method to create and return a WinUsbPipeApi instance for the specified pipeId. This will likely involve calling
                GetAssociatedInterface(_associatedInterfaceIndex, out SafeHandle associatedInterfaceHandle);
                handle = associatedInterfaceHandle as winmdroot.WinUsb_FreeSafeHandle;
            }

            return new WinUsbPipeApi(handle, pipeId);
        }

        public void Close()
        {
            ThrowIfClosedOrDisposed();

            _usbDeviceHandle.Close();
        }


        public UsbDeviceSpeedEnum QueryDeviceInformation()
        {
            ThrowIfClosedOrDisposed();

            uint bufferLength = 2;
            byte[] byteBuffer = new byte[bufferLength];
            Span<byte> buffer = new Span<byte>(byteBuffer);
            UsbDeviceSpeedEnum deviceSpeed = UsbDeviceSpeedEnum.UsbUndefinedSpeed;

            bool retval = winmdroot.PInvoke.WinUsb_QueryDeviceInformation(this._usbDeviceHandle, (uint)UsbDeviceSpeedEnum.UsbDeviceSpeedInfo, ref bufferLength, buffer);

            if (bufferLength >= 1)
            {
                deviceSpeed = (UsbDeviceSpeedEnum)buffer[0];
            }

            return deviceSpeed;
        }

        /// <summary>
        /// Queries the interface settings.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="alternateInterfaceNumber">The alternate interface number.</param>
        /// <param name="usbInterfaceDescriptor">The usb interface descriptor.</param>
        /// <returns></returns>
        public bool QueryInterfaceSettings(byte alternateInterfaceNumber, out UsbInterfaceDescriptor usbInterfaceDescriptor)
        {
            ThrowIfClosedOrDisposed();

            USB_INTERFACE_DESCRIPTOR usbAltInterfaceDescriptor;

            bool retval = winmdroot.PInvoke.WinUsb_QueryInterfaceSettings(this._usbDeviceHandle,
                    alternateInterfaceNumber,
                    out usbAltInterfaceDescriptor);

            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            usbInterfaceDescriptor = new UsbInterfaceDescriptor(usbAltInterfaceDescriptor);

            return retval;
        }



        /// <summary>
        /// Queries the pipe.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="alternateInterfaceNumber">The alternate interface number.</param>
        /// <param name="pipeIndex">Index of the pipe.</param>
        /// <param name="usbPipeInformation">The usb pipe information.</param>
        /// <returns></returns>
        public bool QueryPipe(byte alternateInterfaceNumber, byte pipeIndex, out UsbPipeInformation usbPipeInformation)
        {
            ThrowIfClosedOrDisposed();

            WINUSB_PIPE_INFORMATION usbAltInterfaceDescriptor;
            bool retval = winmdroot.PInvoke.WinUsb_QueryPipe(this._usbDeviceHandle,
                    alternateInterfaceNumber,
                    pipeIndex,
                    out usbAltInterfaceDescriptor);

            usbPipeInformation = new UsbPipeInformation(usbAltInterfaceDescriptor);
            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }
            return retval;
        }

        /// <summary>
        /// Retrieves a handle to the associated USB interface specified by the given index.        
        /// </summary>
        /// <remarks>If the method returns false, the associated interface handle is not valid and an
        /// error is logged. The caller is responsible for disposing the returned SafeHandle when it is no longer
        /// needed.</remarks>
        /// <param name="associatedInterfaceIndex">The zero-based index of the associated interface to retrieve. Must correspond to a valid interface supported
        /// by the device.</param>
        /// <param name="associatedInterfaceHandle">When this method returns, contains a SafeHandle representing the associated interface if the operation
        /// succeeds; otherwise, contains an invalid handle.</param>
        /// <returns>true if the associated interface handle was successfully retrieved; otherwise, false.</returns>
        public bool GetAssociatedInterface(byte associatedInterfaceIndex, out SafeHandle associatedInterfaceHandle)
        {
            ThrowIfClosedOrDisposed();

            var retval = winmdroot.PInvoke.WinUsb_GetAssociatedInterface(this._usbDeviceHandle,
              associatedInterfaceIndex,
              out winmdroot.WinUsb_FreeSafeHandle handle);

            associatedInterfaceHandle = handle;

            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            return retval;
        }

        public void GetDescriptorTest()
        {
            ThrowIfClosedOrDisposed();

            int bufferSize = Marshal.SizeOf<USB_DESCRIPTOR_REQUEST>();
            int bufferSize2 = Marshal.SizeOf<USB_CONFIGURATION_DESCRIPTOR>();
            byte[] buffer = new byte[2560];
            Span<byte> spanBuffer = new Span<byte>(buffer);


            for (int i = 1; i < 4; i++)
            {
                bool retval = PInvoke.WinUsb_GetDescriptor(this._usbDeviceHandle,
                        (byte)i,
                        0,
                        0,
                        spanBuffer,

                        out uint lengthTransferred);

                _logger.Info($"result = {retval}, descriptor type {i}, returned lenght {lengthTransferred}.");

                var descriptor = MemoryMarshal.Read<USB_DEVICE_DESCRIPTOR>(spanBuffer);

                if (retval && lengthTransferred >= buffer.Length)
                    _logger.Error($"missing bytes {buffer.Length - lengthTransferred}");

                var lastError = Marshal.GetLastPInvokeError();

                if (!retval && lastError != 0)
                {
                    _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}, {lastError}");
                }
            }
        }

        public bool GetDeviceDescriptor(out UsbDeviceDescriptor descriptor)
        {
            bool retval = GetDescriptor<USB_DEVICE_DESCRIPTOR, UsbDeviceDescriptor>(
                UsbDescriptorTypeEnum.USB_DEVICE_DESCRIPTOR_TYPE, 0, out descriptor);

            return retval;
        }


        public bool GetConfigurationDescriptor(int index, out UsbConfigurationDescriptor descriptor)
        {
            ThrowIfClosedOrDisposed();

            bool retval = GetDescriptor<USB_CONFIGURATION_DESCRIPTOR, UsbConfigurationDescriptor>(
                UsbDescriptorTypeEnum.USB_CONFIGURATION_DESCRIPTOR_TYPE, index, out descriptor);

            return retval;
        }

        public bool GetInterfaceDescriptor(int index, out UsbInterfaceDescriptor descriptor)
        {
            ThrowIfClosedOrDisposed();

            var retval = PInvoke.WinUsb_QueryInterfaceSettings(this._usbDeviceHandle, (byte)index, out USB_INTERFACE_DESCRIPTOR usbAltInterfaceDescriptor);

            var lastError = Marshal.GetLastPInvokeError();

            if (!retval && lastError != 0)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}, {lastError}");
            }

            descriptor = new UsbInterfaceDescriptor(usbAltInterfaceDescriptor);

            return retval;
        }

        public bool GetEndpointDescriptor(int index, out UsbEndpointDescriptor descriptor)
        {
            ThrowIfClosedOrDisposed();

            bool retval = GetDescriptor<USB_ENDPOINT_DESCRIPTOR, UsbEndpointDescriptor>(
                UsbDescriptorTypeEnum.USB_ENDPOINT_DESCRIPTOR_TYPE, index, out descriptor);

            return retval;
        }

        public bool GetCurrentAlternateSetting(out byte settingNumber)
        {
            ThrowIfClosedOrDisposed();

            byte[] byteBuffer = new byte[1];
            var spanByte = new Span<byte>(byteBuffer);
            BOOL retval = PInvoke.WinUsb_GetCurrentAlternateSetting(this._usbDeviceHandle, spanByte);
            settingNumber = byteBuffer[0];

            var lastError = Marshal.GetLastPInvokeError();
            if (!retval && lastError != 0)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}, {lastError}");
            }

            return retval;
        }
        public bool SetCurrentAlternateSetting(byte settingNumber)
        {
            ThrowIfClosedOrDisposed();

            BOOL retval = PInvoke.WinUsb_SetCurrentAlternateSetting(this._usbDeviceHandle, settingNumber);

            var lastError = Marshal.GetLastPInvokeError();
            if (!retval && lastError != 0)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}, {lastError}");
            }

            return retval;
        }

        // TODO: Add other APIs as needed
#if false


BOOL WinUsb_ControlTransfer(
  [in]            WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [in]            WINUSB_SETUP_PACKET     SetupPacket,
  [out]           PUCHAR                  Buffer,
  [in]            ULONG                   BufferLength,
  [out, optional] PULONG                  LengthTransferred,
  [in, optional]  LPOVERLAPPED            Overlapped
);

BOOL WinUsb_GetAdjustedFrameNumber(
  [in, out] PULONG        CurrentFrameNumber,
  [in]      LARGE_INTEGER TimeStamp
);


BOOL WinUsb_GetCurrentFrameNumber(
  [in]  WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [out] PULONG                  CurrentFrameNumber,
  [out] LARGE_INTEGER           *TimeStamp
);

BOOL WinUsb_GetCurrentFrameNumberAndQpc(
  [in] WINUSB_INTERFACE_HANDLE                             InterfaceHandle,
  [in] PUSB_FRAME_NUMBER_AND_QPC_FOR_TIME_SYNC_INFORMATION FrameQpcInfo
);

		internal static unsafe winmdroot.Foundation.BOOL WinUsb_GetDescriptor(SafeHandle InterfaceHandle, byte DescriptorType, byte Index, ushort LanguageID, Span<byte> Buffer, out uint LengthTransferred)

BOOL WinUsb_GetOverlappedResult(
  [in]  WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [in]  LPOVERLAPPED            lpOverlapped,
  [out] LPDWORD                 lpNumberOfBytesTransferred,
  [in]  BOOL                    bWait
);

BOOL WinUsb_GetPowerPolicy(
  [in]      WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [in]      ULONG                   PolicyType,
  [in, out] PULONG                  ValueLength,
  [out]     PVOID                   Value
);


BOOL WinUsb_ReadIsochPipe(
  [in]           WINUSB_ISOCH_BUFFER_HANDLE  BufferHandle,
  [in]           ULONG                       Offset,
  [in]           ULONG                       Length,
  [in, out]      PULONG                      FrameNumber,
  [in]           ULONG                       NumberOfPackets,
  [out]          PUSBD_ISO_PACKET_DESCRIPTOR IsoPacketDescriptors,
  [in, optional] LPOVERLAPPED                Overlapped
);

BOOL WinUsb_ReadIsochPipeAsap(
  [in]           WINUSB_ISOCH_BUFFER_HANDLE  BufferHandle,
  [in]           ULONG                       Offset,
  [in]           ULONG                       Length,
  [in]           BOOL                        ContinueStream,
  [in]           ULONG                       NumberOfPackets,
                 PUSBD_ISO_PACKET_DESCRIPTOR IsoPacketDescriptors,
  [in, optional] LPOVERLAPPED                Overlapped
);

BOOL WinUsb_RegisterIsochBuffer(
  [in]  WINUSB_INTERFACE_HANDLE     InterfaceHandle,
  [in]  UCHAR                       PipeID,
  [in]  PUCHAR                      Buffer,
  [in]  ULONG                       BufferLength,
  [out] PWINUSB_ISOCH_BUFFER_HANDLE IsochBufferHandle
);

BOOL WinUsb_SetPowerPolicy(
  [in] WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [in] ULONG                   PolicyType,
  [in] ULONG                   ValueLength,
  [in] PVOID                   Value
);

typedef struct _WINUSB_SETUP_PACKET {
  UCHAR  RequestType;
  UCHAR  Request;
  USHORT Value;
  USHORT Index;
  USHORT Length;
} WINUSB_SETUP_PACKET, *PWINUSB_SETUP_PACKET;

BOOL WinUsb_StartTrackingForTimeSync(
  [in] WINUSB_INTERFACE_HANDLE                       InterfaceHandle,
  [in] PUSB_START_TRACKING_FOR_TIME_SYNC_INFORMATION StartTrackingInfo
);

BOOL WinUsb_StopTrackingForTimeSync(
  [in] WINUSB_INTERFACE_HANDLE                      InterfaceHandle,
  [in] PUSB_STOP_TRACKING_FOR_TIME_SYNC_INFORMATION StopTrackingInfo
);
BOOL WinUsb_UnregisterIsochBuffer(
  [in] WINUSB_ISOCH_BUFFER_HANDLE IsochBufferHandle
);
BOOL WinUsb_WriteIsochPipeAsap(
  [in]           WINUSB_ISOCH_BUFFER_HANDLE BufferHandle,
  [in]           ULONG                      Offset,
  [in]           ULONG                      Length,
  [in]           BOOL                       ContinueStream,
  [in, optional] LPOVERLAPPED               Overlapped

);

#endif

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_usbDeviceHandle != null && !_usbDeviceHandle.IsInvalid)
                    {
                        Close();
                        _usbDeviceHandle.Dispose();
                        _usbDeviceHandle = null;
                    }
                    // dispose managed state (managed objects)
                }

                //  free unmanaged resources (unmanaged objects) and override finalizer
                //  set large fields to null
                disposedValue = true;
            }
        }

        // // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WinUsbDeviceApi()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool GetDescriptor<T, TC>(UsbDescriptorTypeEnum descriptorType, int index, out TC descriptor)
            where T : struct
            where TC : class, IDataWrapper<T>, new()
        {

            descriptor = new TC();

            int bufferSize = Marshal.SizeOf<T>();
            byte[] buffer = new byte[bufferSize];
            Span<byte> spanBuffer = new Span<byte>(buffer);

            var retval = winmdroot.PInvoke.WinUsb_GetDescriptor(this._usbDeviceHandle,
              (byte)descriptorType,
              (byte)index,
              0,
              spanBuffer,
              out uint lengthTransferred);

            descriptor._src = MemoryMarshal.Read<T>(spanBuffer);

            if (retval && lengthTransferred > buffer.Length)
                _logger.Error($"missing bytes {buffer.Length - lengthTransferred}");

            var lastError = Marshal.GetLastPInvokeError();

            if (!retval && lastError != 0)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}, {lastError}");
            }

            return retval;
        }

        private void ThrowIfClosedOrDisposed()
        {
            if (IsClosed)
            {
                throw new ObjectDisposedException(nameof(WinUsbDeviceApi));
            }
            if (disposedValue)
            {
                throw new ObjectDisposedException(nameof(WinUsbDeviceApi));
            }
        }
    }
}
