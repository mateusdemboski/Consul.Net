namespace Consul.Net
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul.Net.Models;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    /// <inheritdoc/>
    [SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Instantiated through reflection")]
    internal class AgentServiceRegisterHostedService : IHostedService
    {
        private readonly IConsulClient consulClient;
        private AgentServiceRegister serviceRegister;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentServiceRegisterHostedService"/> class.
        /// </summary>
        /// <param name="consulClient">The <see cref="IConsulClient"/> instance to register the running service.</param>
        /// <param name="serviceRegisterOptions"> The configured <see cref="IOptions{AgentServiceRegister}"/>.</param>
        public AgentServiceRegisterHostedService(
            IConsulClient consulClient,
            IOptionsMonitor<AgentServiceRegister> serviceRegisterOptions)
        {
            this.consulClient = consulClient;
            this.serviceRegister = serviceRegisterOptions.CurrentValue;
            _ = serviceRegisterOptions.OnChange(async options =>
            {
                await this.StopAsync().ConfigureAwait(false);

                this.serviceRegister = options;

                await this.StopAsync().ConfigureAwait(false);
            });
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return this.consulClient.Agent.Service
                .RegisterAsync(this.serviceRegister, cancellationToken);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
