namespace Consul.Net
{
    using System;

    /// <inheritdoc/>
    public sealed class ConsulClient : IConsulClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsulClient"/> class.
        /// </summary>
        /// <param name="agentResources">The <see cref="IAgentResources"/> implementation.</param>
        public ConsulClient(IAgentResources agentResources)
        {
            this.Agent = agentResources ?? throw new ArgumentNullException(nameof(agentResources));
        }

        /// <inheritdoc/>
        public IAgentResources Agent { get; }
    }
}
