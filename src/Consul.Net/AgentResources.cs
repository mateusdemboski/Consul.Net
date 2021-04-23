namespace Consul.Net
{
    using System;

    /// <inheritdoc/>
    public sealed class AgentResources : IAgentResources
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentResources"/> class.
        /// </summary>
        /// <param name="serviceResources">The <see cref="IAgentServiceResources"/> implementation.</param>
        public AgentResources(IAgentServiceResources serviceResources)
        {
            this.Service = serviceResources ?? throw new ArgumentNullException(nameof(serviceResources));
        }

        /// <inheritdoc/>
        public IAgentServiceResources Service { get; }
    }
}
