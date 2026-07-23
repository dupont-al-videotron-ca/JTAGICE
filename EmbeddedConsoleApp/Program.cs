
namespace EmdeddedConsoleApp
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using EmbeddedConsoleApp.Commands;
    using EmbeddedConsoleApp.DependencyInjection;
    using FluentArgs;
    using FluentArgs.Help;
    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using Log4UsbService.EmbeddedData;
    using Microsoft.Extensions.Hosting;
    using MyFramework.CommandArgs;
    using MyFramework.DependencyInjection;

    public static class Program
    {
        public static void Main(string[] args)
        {

            IHost host = new Startup().HostCreateApplicationBuilder().Build();
            //host.Services.GetService(typeof(ConsoleHostedService));

            host.Start();
        }
    }
}