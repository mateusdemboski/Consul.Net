namespace Consul.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Consul.Net.Filtering;
    using Consul.Net.Models;
    using Consul.Net.Util.Http;
    using Flurl;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    ///     Extension methods for registering the Consul.Net library tools in the Microsoft dependency injection container.
    /// </summary>
    public static class ConsulDotNetRegistrationExtensions
    {
        /// <summary>
        ///     Adds Consul.Net resources and dependencyes on the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add the services.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddConsul(this IServiceCollection serviceCollection)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection
                .ConfigureOptions<ConsulClientOptionsSetup>()
                .AddTransient<IHttpContentFactory, JsonContentFactory>()
                .AddHttpClient<IAgentServiceResources, AgentServiceResources>((serviceProvider, httpClient) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<ConsulClientOptions>>().Value;

                    httpClient.BaseAddress = options.Address
                        .AppendPathSegments("v1", "agent", "service")
                        .ToUri();
                }).Services
                .AddTransient<Func<Func<string, IEnumerable<AgentService>>, IQueryProvider>>(serviceProvider =>
                    filterExecuter => ActivatorUtilities.CreateInstance<FilterQueryProvider<AgentService>>(
                        serviceProvider,
                        filterExecuter))
                .AddTransient<Func<Func<string, IEnumerable<AgentService>>, IQueryable<AgentService>>>(serviceProvider =>
                    x => ActivatorUtilities.CreateInstance<FilterQueryable<AgentService>>(
                        serviceProvider,
                        serviceProvider.GetRequiredService<Func<Func<string, IEnumerable<AgentService>>, IQueryProvider>>().Invoke(x)))
                .AddHttpClient<IAgentResources, AgentResources>((serviceProvider, httpClient) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<ConsulClientOptions>>().Value;

                    httpClient.BaseAddress = options.Address
                        .AppendPathSegments("v1", "agent")
                        .ToUri();
                }).Services
                .AddTransient<IConsulClient, ConsulClient>();
        }

        /// <summary>
        ///     Adds a hosted service that register a service on consul based on <see cref="IOptions{AgentServiceRegister}"/>
        ///     to the provided <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/> to add the services.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddConsulServiceRegister(this IServiceCollection serviceCollection)
        {
            _ = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));

            return serviceCollection
                .ConfigureOptions<AgentServiceRegisterSetup>()
                .AddHostedService<AgentServiceRegisterHostedService>();
        }
    }
}
