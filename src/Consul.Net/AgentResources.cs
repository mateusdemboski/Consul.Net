namespace Consul.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul.Net.Models;
    using Consul.Net.Util.Threading;
    using Flurl;

    /// <inheritdoc/>
    public sealed class AgentResources : IAgentResources
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentResources"/> class.
        /// </summary>
        /// <param name="httpClient">The configured <see cref="HttpClient"/> to comunicate with Consul `/agent/*` endpoints.</param>
        /// <param name="serviceResources">The <see cref="IAgentServiceResources"/> implementation.</param>
        /// <param name="serviceQueryableFactory">The <see cref="IQueryable{T}"/> of <see cref="AgentService"/> factory.</param>
        public AgentResources(
            HttpClient httpClient,
            IAgentServiceResources serviceResources,
            Func<Func<string, IEnumerable<AgentService>>, IQueryable<AgentService>> serviceQueryableFactory)
        {
            _ = serviceQueryableFactory ?? throw new ArgumentNullException(nameof(serviceQueryableFactory));

            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.Service = serviceResources ?? throw new ArgumentNullException(nameof(serviceResources));
            this.Services = serviceQueryableFactory(x => this.GetServicesAsync(x).ToSync());
        }

        /// <inheritdoc/>
        public IAgentServiceResources Service { get; }

        /// <inheritdoc/>
        public IQueryable<AgentService> Services { get; }

        /// <inheritdoc/>
        public Task<IEnumerable<AgentService>> GetServicesAsync(string? filter, CancellationToken cancellationToken = default)
        {
            return CreateTask();

            async Task<IEnumerable<AgentService>> CreateTask()
            {
                var url = new Url("services");

                if (filter is not null)
                {
                    _ = url.SetQueryParam("filter", filter);
                }

                var request = await this.httpClient
                    .GetAsync(new Uri(url, UriKind.Relative), cancellationToken)
                    .ConfigureAwait(false);

                _ = request
                    .EnsureSuccessStatusCode();

                return (await request.Content
                    .ReadFromJsonAsync<IDictionary<string, AgentService>>(cancellationToken: cancellationToken)
                    .ConfigureAwait(false))
                    .Select(x => x.Value);
            }
        }
    }
}
