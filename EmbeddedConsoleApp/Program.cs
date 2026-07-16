
namespace EmdeddedConsoleApp
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using EmbeddedConsoleApp.Commands;
    using FluentArgs;
    using FluentArgs.Help;
    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using Log4UsbService.EmbeddedData;
    using MyFramework.CommandArgs;


    public static class Program
    {
        public static void Main(string[] args)
        {
            // initialize log4net
            //Hierarchy logRepository = (Hierarchy)LogManager.GetRepository(Assembly.GetCallingAssembly());
            //XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo("log4net.config"));

            // initialize commands
            new Log4UsbCommand();


            // Run application console
            Commands.RunConsole(args);

            Console.WriteLine("Test complete. Hit enter");
        }

    }
}