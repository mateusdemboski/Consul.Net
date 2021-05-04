namespace Consul.Net
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul.Net.Models;

    /// <summary>
    /// Interface resposible to comunicate with `/agent` consul endpoint.
    /// </summary>
    public interface IAgentResources
    {
        /// <summary>
        /// Gets the entrypoint communication with `/agent/service/*` Consul endpoint.
        /// </summary>
        IAgentServiceResources Service { get; }

        /// <summary>
        /// Gets the queryable entrypoint to filter services using the `/agent/services` Consul endpoint.
        /// </summary>
        IQueryable<AgentService> Services { get; }

        /// <summary>
        /// Gets a list of serviceusing the `/agent/services` Consul endpoint.
        /// </summary>
        /// <param name="filter">The consul filter expression.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A list of <see cref="AgentService"/>.</returns>
        /// <exception cref="HttpRequestException">Throwed when an unsuccessful status code was returnd by the service.</exception>
        Task<IEnumerable<AgentService>> GetServicesAsync(string? filter = default, CancellationToken cancellationToken = default);
    }
}
