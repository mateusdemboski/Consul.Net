namespace Consul.Net
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul.Net.Models;

    /// <summary>
    ///     The contract to interact with services on the agent in Consul.
    ///     These should not be confused with services in the catalog.
    /// </summary>
    public interface IAgentServiceResources
    {
        /// <summary>
        /// Adds a new service, with optional health checks, to the Consul agent.
        /// </summary>
        /// <param name="agentServiceRegister">The <see cref="AgentServiceRegister"/> structure that will be registerd on Consul.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A completed Task if the service was successful registered. Otherwise, throws an exception.</returns>
        /// <exception cref="HttpRequestException">Throwed when an unsuccessful status code was returnd by the service.</exception>
        Task RegisterAsync(AgentServiceRegister agentServiceRegister, CancellationToken cancellationToken = default);
    }
}
