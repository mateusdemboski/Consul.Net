namespace Consul.Net.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Model representing the agent service register request.
    /// </summary>
    public sealed class AgentServiceRegister
    {
        /// <summary>
        ///     Gets or Sets the address of the service.
        ///     If not provided, the agent's address is used as the address for the service during DNS queries.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        ///     Gets or Sets the service check configuration.
        /// </summary>
        public AgentServiceCheck? Check { get; set; }

        /// <summary>
        ///     Gets or Sets the list of checks.
        ///     The automatically generated Name and CheckID depend on the position of the check within the array,
        ///     so even though the behavior is deterministic, it is recommended for all checks to either let consul
        ///     set the CheckID by leaving the field empty/omitting it or to provide a unique value.
        /// </summary>
        public IReadOnlyCollection<AgentServiceCheck>? Checks { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to disable the anti-entropy feature for this service's tags.
        ///     If EnableTagOverride is set to true then external agents can update this service in the catalog and modify the tags.
        ///     Subsequent local sync operations by this agent will ignore the updated tags.
        ///     For instance, if an external agent modified both the tags and the port for this service and EnableTagOverride was set
        ///     to true then after the next sync cycle the service's port would revert to the original value but the tags would
        ///     maintain the updated value. As a counter example, if an external agent modified both the tags and port for this
        ///     service and EnableTagOverride was set to false then after the next sync cycle the service's port and the tags
        ///     would revert to the original value and all modifications would be lost.
        /// </summary>
        public bool EnableTagOverride { get; set; }

        /// <summary>
        ///     Gets or Sets the unique ID for this service.
        ///     This must be unique per agent.
        ///     This defaults to the Name parameter if not provided.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Gets or Sets the kind of service.
        ///     Defaults is a typical Consul service.
        /// </summary>
        public AgentServiceKind? Kind { get; set; }

        /// <summary>
        /// Gets or sets the arbitrary KV metadata linked to the service instance.
        /// </summary>
        public IReadOnlyDictionary<string, string>? Meta { get; set; }

        /// <summary>
        ///     Gets or Sets the logical name of the service.
        ///     Many service instances may share the same logical service name.
        ///     Consul team recommend using valid DNS labels for compatibility with external DNS.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or Sets the port of the service.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Gets or Sets a map of explicit LAN and WAN addresses for the service instance
        ///     Both the address and port can be specified within the map values.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>>? TaggedAddresses { get; set; }

        /// <summary>
        ///     Gets or Sets the list of tags to assign to the service.
        ///     These tags can be used for later filtering and are exposed via the APIs.
        ///     Consul team recommend using valid DNS labels for compatibility with external DNS.
        /// </summary>
        public IReadOnlyCollection<string>? Tags { get; set; }
    }
}
