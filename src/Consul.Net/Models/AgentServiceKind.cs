namespace Consul.Net.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum representing the consul service kinds.
    /// </summary>
    public enum AgentServiceKind
    {
        /// <summary>
        /// Connect proxie representing another service.
        /// </summary>
        [EnumMember(Value = "connect-proxy")]
        ConnectProxy,

        /// <summary>
        /// Instances of a ingress gateway.
        /// </summary>
        [EnumMember(Value = "ingress-gateway")]
        IngressGateway,

        /// <summary>
        /// Instance of a mesh gateway.
        /// </summary>
        [EnumMember(Value = "mesh-gateway")]
        MeshGateway,

        /// <summary>
        /// Instances of a terminating gateway.
        /// </summary>
        [EnumMember(Value = "terminating-gateway")]
        TerminatingGateway,
    }
}
