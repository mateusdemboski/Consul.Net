namespace Consul.Net.Util.Http
{
    using System.Net.Http;
    using System.Net.Http.Json;

    /// <summary>
    /// A <see cref="IHttpContentFactory"/> implementation that creates <see cref="JsonContent"/> instances.
    /// </summary>
    public sealed class JsonContentFactory : IHttpContentFactory
    {
        /// <inheritdoc/>
        HttpContent IHttpContentFactory.CreateContent<T>(T value)
        {
            return JsonContent.Create(value);
        }
    }
}
