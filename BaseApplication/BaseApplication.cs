using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ApplicationFW
{
    /// <summary>
    /// Base class for applications implementing the IApplication interface.    
    /// </summary>
    /// <seealso cref="ApplicationFW.IHostApplication" />
    public abstract class BaseApplication : IApplication
    {
        protected BaseApplication()
        {
        }

        /// <summary>
        /// The logger.
        /// </summary>
        [AllowNull]
        private Serilog.ILogger _logger;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILogger Logger => this._logger;


        /// <summary>
        /// Initializes - build common application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Apllication builder.</returns>
        private HostApplicationBuilder Initialize(string[] args)
        {

            HostApplicationBuilderSettings settings = new()
            {
                Args = null,
                Configuration = new ConfigurationManager(),
                ContentRootPath = Directory.GetCurrentDirectory(),
                DisableDefaults = true,
            };

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(settings);
            builder.Configuration.AddJsonFile("AppSettings.json", optional: true, true);
            //            builder.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
            //            builder.Configuration.AddCommandLine(args, switchMappings);
            //           builder.Configuration.AddConfiguration(builder.Configuration);

            builder.Services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
#if DEBUG1
                .MinimumLevel.Debug()
#endif
                .CreateLogger();

            this._logger = Log.Logger;

            Log.Information("Hello, world!");
            this.Logger.Information("Hello World! Logging is {Description}.", "fun");
            return this.BuildApplication(builder);
        }

        /// <summary>
        /// Builds the application.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        protected virtual HostApplicationBuilder BuildApplication(HostApplicationBuilder builder)
        {
            return builder;
        }

        /// <summary>
        /// Executes the application.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void ExecuteApp(HostApplicationBuilder builder)
        {
            var host = builder.Build();
            host.Run();
        }

        public void Run(string[] args)
        {
            try
            {
                var builder = this.Initialize(args);
                this.ExecuteApp(builder);
                Log.Information("Application is shutting down.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred during application execution.");
            }
        }
    }
}
