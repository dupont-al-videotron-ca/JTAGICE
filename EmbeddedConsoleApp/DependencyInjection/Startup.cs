using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFramework.DependencyInjection;
using MyFramework.CommandArgs;
using MyUsbDevice;
using UsbDeviceBase;

namespace EmbeddedConsoleApp.DependencyInjection
{
    internal class Startup : StartupBase
    {

        internal Startup() { }

        override public HostApplicationBuilder HostCreateApplicationBuilder()
        {
            var builder = base.HostCreateApplicationBuilder();
            // Add additional services or configurations here if needed
            // For example, you can add custom services, logging providers, etc.
            builder.Services.AddSingleton<IUsbDevice, At90UsbKeyDevice>();

            builder.Services.AddLogUsbService();

            builder.Services.AddSingleton<ICommands, MyFramework.CommandArgs.Commands>();
            builder.Services.AddHostedService<ConsoleHostedService>();
            return builder;
        }

        override public IConfigurationBuilder CreateConfigurationBuilder(IConfigurationBuilder configBuilder)
        {
            base.CreateConfigurationBuilder(configBuilder);
            // Add additional configuration sources here if needed
            // For example, you can add JSON files, environment variables, etc.
            // configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            // configBuilder.AddEnvironmentVariables();

            return configBuilder;
        }
    }
}
