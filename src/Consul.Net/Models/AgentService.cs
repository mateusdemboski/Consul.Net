namespace Consul.Net.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// aaaaaaaaaaaaa.
    /// </summary>
    public sealed class AgentService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentService"/> class.
        /// </summary>
        public AgentService()
        {
            this.Address = string.Empty;
            this.Datacenter = string.Empty;
            this.ID = string.Empty;
            this.Meta = new Dictionary<string, string>();
            this.Service = string.Empty;
            this.Tags = new HashSet<string>();
        }

        /// <summary>
        /// Gets or Sets aaa.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or Sets aaaa.
        /// </summary>
        public string Datacenter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether aaaaa.
        /// </summary>
        public bool EnableTagOverride { get; set; }

        /// <summary>
        /// Gets or Sets aaaaaa.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets aaaaaaa.
        /// </summary>
        public IDictionary<string, string> Meta { get; }

        /// <summary>
        /// Gets or Sets aaaaaaaa.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or Sets aaaaaa.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or Sets aaaaaaa.
        /// </summary>
        public IEnumerable<string> Tags { get; set; }
    }
}
