using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyFramework.DependencyInjection

{
    /// <summary>
    /// A base class for configuring and initializing a .NET application with dependency injection, logging, and configuration settings.
    /// </summary>
    public abstract class StartupBase
    {
        public StartupBase()
        {
            // Constructor logic if needed
        }

        /// <summary>
        /// Creates and configures an <see cref="IHostApplicationBuilder"/> instance for the application.
        /// </summary>
        /// <returns></returns>
        public virtual HostApplicationBuilder HostCreateApplicationBuilder()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

#if DEBUG
            builder.Environment.EnvironmentName = "Development"; // Set the environment name if needed
#else
            builder.Environment.EnvironmentName = "Production"; // Set the environment name if needed
#endif  

            // logging and read configuration from log4net.config
            builder.Logging.AddLog4Net();

            //builder.Services.AddTransient<TransientDisposable>(); 
            //builder.Services.AddScoped<ScopedDisposable>(); 
            //builder.Services.AddSingleton<SingletonDisposable>();

            // Configure the application configuration
            CreateConfigurationBuilder(builder.Configuration);
            return builder;
        }

        /// <summary>
        /// Creates and configures an <see cref="IConfigurationBuilder"/> instance for the application.
        /// </summary>
        /// <returns></returns>
        public virtual IConfigurationBuilder CreateConfigurationBuilder(IConfigurationBuilder configBuilder)
        {
            configBuilder.AddCommandLine(Environment.GetCommandLineArgs());

            return configBuilder;

        }

        /// <summary>
        /// Builds the application host and configuration using the provided <see cref="HostApplicationBuilder"/> and <see cref="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="hostApplicationBuilder"></param>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public virtual IHost BuildAll(HostApplicationBuilder hostApplicationBuilder, IConfigurationBuilder configurationBuilder)
        {
            IHost host = hostApplicationBuilder.Build();
            IConfigurationRoot configuration = configurationBuilder.Build();

            return host;
        }
    }
}
