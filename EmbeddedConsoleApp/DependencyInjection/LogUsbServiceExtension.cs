using Microsoft.Extensions.DependencyInjection;
using MyUsbDevice;
using System;
using System.Collections.Generic;
using System.Text;
using UsbDeviceBase;
using Windows.Devices.Portable;

namespace EmbeddedConsoleApp.DependencyInjection
{
    public static class LogUsbServiceExtension
    {

        public static IServiceCollection AddLogUsbService(this IServiceCollection services)
        {
            services.AddSingleton<Log4UsbService.ILog4UsbService, Log4UsbService.Log4UsbService>((t) =>
            {
                At90UsbKeyDevice? device = t.GetService<IUsbDevice>() as At90UsbKeyDevice;
                return new Log4UsbService.Log4UsbService(At90UsbKeyDevice.InterfaceId, 
                                                    At90UsbKeyDevice.LogPipeInId,
                                                    device!,
                                                    log4net.LogManager.GetLogger(typeof(Log4UsbService.Log4UsbService)));
            });

            return services;
        }
    }
}
