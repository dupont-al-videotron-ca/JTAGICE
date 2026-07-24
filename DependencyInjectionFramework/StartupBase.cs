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
        /// Creates and configures an <see cref="IHostBuilder"/> instance for the application.
        /// </summary>
        /// <returns></returns>
        public virtual IHostBuilder HostCreateBuilder()
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureHostOptions(x => x.ShutdownTimeout = TimeSpan.FromSeconds(5)); // Set the shutdown timeout to 5 seconds
            builder.ConfigureHostConfiguration(hostBuilder => CreateConfigurationBuilder(hostBuilder));
            builder.UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
            });

            builder.ConfigureContainer<Microsoft.Extensions.DependencyInjection.IServiceCollection>((context, services) =>
            {
                services.AddOptions();
            });

#if DEBUG
            builder.UseEnvironment(Environments.Development); // Set the environment name if needed
#else
            builder.UseEnvironment(Environments.Production); // Set the environment name if needed
#endif  
            builder.ConfigureLogging(logging => logging.AddLog4Net());

            // Configure the application configuration
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
