namespace Consul.Net
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Options container to configure Consul agent service auto register.
    /// </summary>
    public sealed class AgentServiceRegisterOptions
    {
        /// <summary>
        /// Gets the default name used to find client configuration on <see cref="IConfiguration"/> container.
        /// </summary>
        public static string DefaultName { get; } = "Consul:ServiceRegister";
    }
}
