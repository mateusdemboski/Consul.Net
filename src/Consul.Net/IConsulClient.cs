namespace Consul.Net
{
    /// <summary>
    /// Main entrypont to comunicate with all Consul endpoints.
    /// </summary>
    public interface IConsulClient
    {
        /// <summary>
        /// Gets the entrypoint communication with `/agent/*` Consul endpoint.
        /// </summary>
        IAgentResources Agent { get; }
    }
}
