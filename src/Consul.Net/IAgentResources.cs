namespace Consul.Net
{
    /// <summary>
    /// Interface resposible to comunicate with `/agent` consul endpoint.
    /// </summary>
    public interface IAgentResources
    {
        /// <summary>
        /// Gets the entrypoint communication with `/agent/service/*` Consul endpoint.
        /// </summary>
        IAgentServiceResources Service { get;  }
    }
}
