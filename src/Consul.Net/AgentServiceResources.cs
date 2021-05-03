namespace Consul.Net
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul.Net.Models;
    using Consul.Net.Util.Http;
    using Flurl;

    /// <inheritdoc/>
    public sealed class AgentServiceResources : IAgentServiceResources
    {
        private readonly HttpClient httpClient;
        private readonly IHttpContentFactory httpContentFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentServiceResources"/> class.
        /// </summary>
        /// <param name="httpClient">The configured <see cref="HttpClient"/> to comunicate with Consul `/agent/service/*` endpoints.</param>
        /// <param name="httpContentFactory">The <see cref="IHttpContentFactory"/> implementation to convert objects into <see cref="HttpContent"/>.</param>
        public AgentServiceResources(
            HttpClient httpClient,
            IHttpContentFactory httpContentFactory)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.httpContentFactory = httpContentFactory ?? throw new ArgumentNullException(nameof(httpContentFactory));
        }

        /// <inheritdoc/>
        Task IAgentServiceResources.DeregisterAsync(string serviceId, CancellationToken cancellationToken)
        {
            _ = serviceId ?? throw new ArgumentNullException(nameof(serviceId));

            return CreateTask();

            async Task CreateTask()
            {
                var url = new Url("deregister")
                    .AppendPathSegment(serviceId);

                using var content = new StringContent(string.Empty);

                var request = await this.httpClient
                    .PutAsync(
                        new Uri(url, UriKind.Relative),
                        content,
                        cancellationToken)
                    .ConfigureAwait(false);

                _ = request
                    .EnsureSuccessStatusCode();
            }
        }

        /// <inheritdoc/>
        Task IAgentServiceResources.RegisterAsync(AgentServiceRegister agentServiceRegister, CancellationToken cancellationToken)
        {
            _ = agentServiceRegister ?? throw new ArgumentNullException(nameof(agentServiceRegister));

            return CreateTask();

            async Task CreateTask()
            {
                var request = await this.httpClient
                    .PutAsync(
                        new Uri("register", UriKind.Relative),
                        this.httpContentFactory.CreateContent(agentServiceRegister),
                        cancellationToken)
                    .ConfigureAwait(false);

                _ = request
                    .EnsureSuccessStatusCode();
            }
        }
    }
}
