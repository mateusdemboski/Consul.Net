namespace Consul.Net.Models
{
    using System.Collections.Generic;

    /// <summary>
    ///     Represents a service check structure to be registerd on Consul.
    ///     If you don't provide a name or id for the check then they will be generated.
    ///     To provide a custom id and/or name set the CheckID and/or Name field.
    /// </summary>
    public sealed class AgentServiceCheck
    {
        /// <summary>
        ///     Gets or Sets the ID of the node for an alias check.
        ///     If no service is specified, the check will alias the health of the node.
        ///     If a service is specified, the check will alias the specified service on this particular node.
        /// </summary>
        public string? AliasNode { get; set; }

        /// <summary>
        ///     Gets or Sets the ID of a service for an alias check.
        ///     If the service is not registered with the same agent, AliasNode must also be specified.
        ///     Note this is the service ID and not the service name (though they are very often the same).
        /// </summary>
        public string? AliasService { get; set; }

        /// <summary>
        ///     Gets or Sets the command arguments to run to update the status of the check.
        ///     Prior to Consul 1.0, checks used a single Script field to define the command to run, and would always run in a shell.
        ///     In Consul 1.0, the Args array was added so that checks can be run without a shell.
        ///     The Script field is deprecated, and you should include the shell in the Args to run under a shell.
        /// </summary>
        /// <example>"args": ["sh", "-c", "..."].</example>
        /// <remarks>
        ///     Consul 1.0 shipped with an issue where Args was erroneously named ScriptArgs in this API.
        ///     Please use ScriptArgs with Consul 1.0 (that will continue to be accepted in future versions of Consul),
        ///     and Args in Consul 1.0.1 and later.
        /// </remarks>
        public IReadOnlyCollection<string>? Args { get; set; }

        /// <summary>
        ///     Gets or Sets the body that should be sent with HTTP checks.
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        ///     Gets or Sets the number of consecutive unsuccessful results required before check status transitions to critical.
        ///     Available for HTTP, TCP, gRPC, Docker <![CDATA[&]]> Monitor checks.
        /// </summary>
        public int FailuresBeforeCritical { get; set; }

        /// <summary>
        ///     Gets or Sets the time that checks associated with a service should deregister after this time.
        ///     This is specified as a time duration with suffix like "10m".
        ///     If a check is in the critical state for more than this configured value, then its associated
        ///     service (and all of its associated checks) will automatically be deregistered.
        ///     The minimum timeout is 1 minute, and the process that reaps critical services runs every 30 seconds,
        ///     so it may take slightly longer than the configured timeout to trigger the deregistration.
        ///     This should generally be configured with a timeout that's much, much longer than any expected
        ///     recoverable outage for the given service.
        /// </summary>
        public string? DeregisterCriticalServiceAfter { get; set; }

        /// <summary>
        ///     Gets or Sets the value that specifies that the check is a Docker check, and Consul will evaluate the script every
        ///     <see cref="Interval"/> in the given container using the specified Shell.
        ///     Note that Shell is currently only supported for Docker checks.
        /// </summary>
        public string? DockerContainerID { get; set; }

        /// <summary>
        ///     Gets or Sets the address that uses http2 with TLS to run a ping check on.
        ///     At the specified <see cref="Interval"/>, a connection is made to the address, and a ping is sent.
        ///     If the ping is successful, the check will be classified as `passing`, otherwise it will be marked as `critical`.
        ///     A valid SSL certificate is required by default, but verification can be removed with <see cref="TlsSkipVerify"/>.
        /// </summary>
        public string? H2Ping { get; set; }

        /// <summary>
        ///     Gets or Sets the set of headers that should be set for `HTTP` checks.
        ///     Each header can have multiple values.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>>? Header { get; set; }

        /// <summary>
        ///     Gets or Sets the `HTTP` check to perform a `GET` request against the value of `HTTP` (expected to be a URL) every <see cref="Interval"/>.
        ///     If the response is any `2xx` code, the check is `passing`. If the response is `429 Too Many Requests`, the check is `warning`.
        ///     Otherwise, the check is critical. HTTP checks also support SSL. By default, a valid SSL certificate is expected.
        ///     Certificate verification can be controlled using the <see cref="TlsSkipVerify"/>.
        /// </summary>
        public string? Http { get; set; }

        /// <summary>
        ///     Gets or Sets the `gRPC` check's endpoint that supports the standard gRPC health checking protocol.
        ///     The state of the check will be updated at the given <see cref="Interval"/> by probing the configured endpoint.
        ///     Add the service identifier after the gRPC check's endpoint in the following format to check for
        ///     a specific service instead of the whole gRPC server `/:service_identifier`.
        /// </summary>
        public string? Grpc { get; set; }

        /// <summary>
        ///     Gets or Sets a value indicating whether to use TLS for this `gRPC` health check.
        ///     If TLS is enabled, then by default, a valid TLS certificate is expected.
        ///     Certificate verification can be turned off by setting <see cref="TlsSkipVerify"/> to `true`.
        /// </summary>
        public bool GrpcUseTls { get; set; }

        /// <summary>
        ///     Gets or Sets the unique ID for this check on the node.
        ///     This defaults to the <see cref="Name"/> parameter, but it may be necessary to provide an ID for uniqueness.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Gets or Sets the frequency at which to run this check.
        ///     This is required for HTTP and TCP checks.
        /// </summary>
        public string? Interval { get; set; }

        /// <summary>
        ///     Gets or Sets the different HTTP method to be used for an `HTTP` check.
        ///     When no value is specified, `GET` is used.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        ///     Gets or Sets the name of the check.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or Sets the arbitrary information for humans.
        ///     This is not used by Consul internally.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        ///     Gets or Sets the maximum size of text for the given check.
        ///     This value must be greater than 0, by default, the value is 4k.
        ///     The value can be further limited for all checks of a given agent using
        ///     the `check_output_max_size` flag in the agent.
        /// </summary>
        public uint OutputMaxSize { get; set; }

        /// <summary>
        ///     Gets or Sets the ID of a service to associate the registered check with an existing service provided by the agent.
        /// </summary>
        public string? ServiceId { get; set; }

        /// <summary>
        ///     Gets or Sets the initial status of the health check.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        ///     Gets or Sets the number of consecutive successful results required before check status transitions to passing.
        ///     Available for HTTP, TCP, gRPC, Docker <![CDATA[&]]> Monitor checks.
        /// </summary>
        public int SuccessBeforePassing { get; set; }

        /// <summary>
        ///     Gets or Sets the TCP to connect against the value of `TCP` (expected to be an IP or hostname
        ///     plus port combination) every <see cref="Interval"/>.
        ///     If the connection attempt is successful, the check is `passing`.
        ///     If the connection attempt is unsuccessful, the check is `critical`.
        ///     In the case of a hostname that resolves to both IPv4 and IPv6 addresses, an attempt will be made
        ///     to both addresses, and the first successful connection attempt will result in a successful check.
        /// </summary>
        public string? Tcp { get; set; }

        /// <summary>
        ///     Gets or Sets the string used to set the SNI host when connecting via TLS.
        ///     For an HTTP check, this value is set automatically if the URL uses a hostname (not an IP address).
        /// </summary>
        public string? TlsServerName { get; set; }

        /// <summary>
        ///     Gets or Sets a value indicating whether if the certificate for an HTTPS check should not be verified.
        /// </summary>
        public bool TlsSkipVerify { get; set; }

        /// <summary>
        ///     Gets or Sets the value when a this is a TTL check, and the TTL endpoint must be used
        ///     periodically to update the state of the check.
        /// </summary>
        public string? Ttl { get; set; }
    }
}
