namespace Consul.Net
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Options container to configure Consul client.
    /// </summary>
    public sealed class ConsulClientOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsulClientOptions"/> class.
        /// </summary>
        public ConsulClientOptions()
        {
            this.Address = new Uri("http://127.0.0.1:8500");
        }

        /// <summary>
        /// Gets the default name used to find client configuration on <see cref="IConfiguration"/> container.
        /// </summary>
        public static string DefaultName { get; } = "Consul";

        /// <summary>
        /// Gets or Sets the address to the Consul API.
        /// </summary>
        public Uri Address { get; set; }
    }
}
