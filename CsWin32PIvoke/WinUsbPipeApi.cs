#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.SafeHandles;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.Properties;
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using static CsWin32Api.WinUsbApi;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    /// <summary>
    /// Wrapper for WinUsbPipeApi, which is used to manage pipes of a USB device. It is used by WinUsbDeviceApi to manage the pipes of a USB device.
    /// 
    /// </summary>
    public class WinUsbPipeApi : IDisposable
    {
        private readonly SafeHandle _associatedInterfaceHandle;
        private readonly byte _pipeId;
        private readonly ILog _logger;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the WinUsbPipeApi class for a specific USB pipe using the provided interface
        /// handle and pipe identifier. 
        /// </summary>
        /// <param name="associatedInterfaceHandle">The handle to the associated USB interface. Cannot be null.</param>
        /// <param name="pipeId">The identifier of the USB pipe to associate with this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="associatedInterfaceHandle"/> is null.</exception>
        internal WinUsbPipeApi(SafeHandle associatedInterfaceHandle, uint pipeId)
        {
            _associatedInterfaceHandle = associatedInterfaceHandle ?? throw new ArgumentNullException(nameof(associatedInterfaceHandle));
            this._pipeId = (byte)pipeId;
            _logger = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Sets the pipe policy.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="pipeID">The pipe identifier.</param>
        /// <param name="policyType">Type of the policy.</param>
        /// <param name="valueLength">Length of the value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool SetPipePolicy(USBPipePolicyEnum policyType, UInt64 data)
        {
            var refData = new Span<byte>(new byte[sizeof(UInt64)]);
            MemoryMarshal.Write(refData, data);

            bool retval = winmdroot.PInvoke.WinUsb_SetPipePolicy(this._associatedInterfaceHandle, this._pipeId,
                (winmdroot.Devices.Usb.WINUSB_PIPE_POLICY)policyType,
                refData);

            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            return retval;
        }

        /// <summary>
        /// Gets the pipe policy.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="pipeID">The pipe identifier.</param>
        /// <param name="policyType">Type of the policy.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public bool GetPipePolicy(USBPipePolicyEnum policyType, ref UInt64 data)
        {
            byte[] buffer = new byte[100];
            Span<byte> refData = new Span<byte>(buffer);
            uint len = (uint)refData.Length;

            var retval = winmdroot.PInvoke.WinUsb_GetPipePolicy(this._associatedInterfaceHandle, _pipeId, (winmdroot.Devices.Usb.WINUSB_PIPE_POLICY)policyType, ref len, refData);

            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            if (len == sizeof(byte))
            {
                data = buffer[0];
            }
            else if (len == sizeof(UInt16))
            {
                data = (UInt16)BitConverter.ToUInt16(refData);
            }
            else if (len == sizeof(UInt32))
            {
                data = (UInt32)BitConverter.ToUInt32(refData);
            }
            else if (len == sizeof(UInt64))
            {
                data = BitConverter.ToUInt64(refData);
            }

            return retval;
        }

        /// <summary>
        /// Attempts to abort the USB pipe associated with the current device handle.   
        /// </summary>
        /// <remarks>This method is typically used to terminate ongoing transfers on a USB pipe. If the
        /// operation fails, an error is logged. The pipe must be valid and open for the abort operation to
        /// succeed.</remarks>
        /// <returns>true if the pipe was successfully aborted; otherwise, false.</returns>
        public bool AbortPipe()
        {
            bool retval = winmdroot.PInvoke.WinUsb_AbortPipe(this._associatedInterfaceHandle, this._pipeId);

            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            return retval;
        }

        /// <summary>
        /// Flushes the USB pipe, ensuring that any buffered data is cleared and the pipe is reset for subsequent
        /// operations.
        /// </summary>
        /// <remarks>This method is typically used to clear any residual data from the USB pipe before
        /// starting new transfers. If the operation fails, an error is logged. Flushing the pipe may be necessary after
        /// encountering errors or before reusing the pipe for new communication.</remarks>
        /// <returns>true if the pipe was successfully flushed; otherwise, false.</returns>
        public bool FlushPipe()
        {
            bool retval = winmdroot.PInvoke.WinUsb_FlushPipe(this._associatedInterfaceHandle, this._pipeId);
            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }
            return retval;
        }

        /// <summary>
        /// Resets the USB pipe associated with the current device handle.  
        /// </summary>
        /// <remarks>This method attempts to reset the USB pipe, which can be useful for recovering from
        /// communication errors. If the reset operation fails, an error message is logged. The pipe must be valid and
        /// open for the reset to succeed.</remarks>
        /// <returns>true if the pipe was successfully reset; otherwise, false.</returns>
        public bool ResetPipe()
        {
            bool retval = winmdroot.PInvoke.WinUsb_ResetPipe(this._associatedInterfaceHandle, this._pipeId);
            if (!retval)
            {
                _logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }
            return retval;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WinUsbPipeApi()
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
#if false

BOOL WinUsb_QueryPipe(
  [in]  WINUSB_INTERFACE_HANDLE  InterfaceHandle,
  [in]  UCHAR                    AlternateInterfaceNumber,
  [in]  UCHAR                    PipeIndex,
  [out] PWINUSB_PIPE_INFORMATION PipeInformation
);
BOOL WinUsb_QueryPipeEx(
  [in]  WINUSB_INTERFACE_HANDLE     InterfaceHandle,
  [in]  UCHAR                       AlternateSettingNumber,
  [in]  UCHAR                       PipeIndex,
  [out] PWINUSB_PIPE_INFORMATION_EX PipeInformationEx
);
BOOL WinUsb_ReadPipe(
  [in]            WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [in]            UCHAR                   PipeID,
  [out]           PUCHAR                  Buffer,
  [in]            ULONG                   BufferLength,
  [out, optional] PULONG                  LengthTransferred,
  [in, optional]  LPOVERLAPPED            Overlapped
);
BOOL WinUsb_WritePipe(
  [in]            WINUSB_INTERFACE_HANDLE InterfaceHandle,
  [in]            UCHAR                   PipeID,
  [in]            PUCHAR                  Buffer,
  [in]            ULONG                   BufferLength,
  [out, optional] PULONG                  LengthTransferred,
  [in, optional]  LPOVERLAPPED            Overlapped
);

#endif
    }
}
