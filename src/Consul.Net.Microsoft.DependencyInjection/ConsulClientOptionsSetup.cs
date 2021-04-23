namespace Consul.Net
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    /// <inheritdoc/>
    [SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Instantiated through reflection")]
    internal sealed class ConsulClientOptionsSetup
        : ConfigureFromConfigurationOptions<ConsulClientOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsulClientOptionsSetup"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        public ConsulClientOptionsSetup(IConfiguration configuration)
            : base(configuration.GetSection(ConsulClientOptions.DefaultName))
        {
        }
    }
}
