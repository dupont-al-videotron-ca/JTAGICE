using EmbeddedConsoleApp.Commands;
using Log4UsbService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyFramework.CommandArgs;
using MyFramework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmbeddedConsoleApp.DependencyInjection
{
    internal class ConsoleHostedService : IHostedService
    {
        private readonly IHost hostService;
        private readonly ICommands commandService;
        private readonly ILogger<ConsoleHostedService> _logger;

        private readonly Log4UsbService.ILog4UsbService _log4UsbService;

        public ConsoleHostedService(
            IHost host,
            ICommands commandService,
            Log4UsbService.ILog4UsbService log4UsbService,
            ILogger<ConsoleHostedService> logger)
        {
            ArgumentNullException.ThrowIfNull(commandService);
            this.hostService = host;
            this.commandService = commandService;
            this._log4UsbService = log4UsbService ?? throw new ArgumentNullException(nameof(log4UsbService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var exitCommand = this.commandService.FindCommand("exit") as ICommandArgs<CommandExit>;
            if (exitCommand != null)
            {
                exitCommand.CommandExec = ExecuteExit;
            }

            // Register commands
            new Log4UsbCommand(this.ExecuteLog4Usb);
        }

        private bool ExecuteExit(CommandExit cmd)
        {
            //Console.WriteLine("Exiting application...");
            // stop all services and exit the application
            hostService.StopAsync().Wait();
            //Environment.Exit(0);
            return true;
        }

        /// <summary>
        /// Called by depencey framework to start the hosted service. This method runs the console application and blocks until the application is stopped.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ConsoleHostedService is starting.");

            // Run application console
            RunConsole(Environment.GetCommandLineArgs(), cancellationToken);

            _logger.LogInformation("ConsoleHostedService has completed.");

            // Implement background task logic here
            return Task.CompletedTask;

        }

        internal void RunConsole(string[] appArgs, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine();
                Console.Write("=>");
                string? line = Console.ReadLine();
                string[] lineArg = line!.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (lineArg.Length >= 1)
                {
                    if (commandService.CommandList.ContainsKey(lineArg[0].ToLower()))
                    {
                        try
                        {
                            if (commandService.CommandList[lineArg[0].ToLower()].Execute(lineArg))
                            {
                                Console.WriteLine($"Command '{lineArg[0]}' executed successfully.");
                            }
                        }
                        catch (TargetInvocationException ex)
                        {
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine(ex.InnerException.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown command: {lineArg[0]}");
                        foreach (var command in commandService.CommandList.Values.OrderBy(c => c.Name))
                        {
                            Console.WriteLine($" {command.Name}");
                        }
                    }
                }
            }
        }

        private bool ExecuteLog4Usb(Log4UsbCommand cmd)
        {
            if (this._log4UsbService.SetReceivingLog(cmd.Level))
            {
                Console.WriteLine($"Log4UsbCommand: Level is {cmd.Level}, logs will be received.");
                return true;
            }
            else
            {
                Console.WriteLine($"Log4UsbCommand: Failed to start receiving logs at level {cmd.Level}.");
                return false;
            }

        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken) => this.StartAsync(cancellationToken);

        Task IHostedService.StopAsync(CancellationToken cancellationToken) => this.StopAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Dispose our service;
            var l = this.hostService.Services.GetService<Log4UsbService.ILog4UsbService>();
            l?.Dispose();
            this.hostService.Services.GetService<UsbDeviceBase.IUsbDevice>()?.Dispose();

            _logger.LogInformation("ConsoleHostedService is stopping.");
            return Task.CompletedTask;
        }
    }
}
