namespace Consul.Net
{
    using System.Diagnostics.CodeAnalysis;
    using Consul.Net.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    /// <inheritdoc/>
    [SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Instantiated through reflection")]
    internal sealed class AgentServiceRegisterSetup
        : ConfigureFromConfigurationOptions<AgentServiceRegister>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentServiceRegisterSetup"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        public AgentServiceRegisterSetup(IConfiguration configuration)
            : base(configuration.GetSection(AgentServiceRegisterOptions.DefaultName))
        {
        }
    }
}
