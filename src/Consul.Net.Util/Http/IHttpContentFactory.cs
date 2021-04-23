namespace Consul.Net.Util.Http
{
    using System.Net.Http;

    /// <summary>
    ///     Contract for <see cref="HttpContent"/> Factories.
    /// </summary>
    public interface IHttpContentFactory
    {
        /// <summary>
        ///     Creates a <see cref="HttpContent"/> based on the provided type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value of the object taht will be converted to an <see cref="HttpContent"/>.</param>
        /// <returns>Returns a <see cref="HttpContent"/> for the provided object.</returns>
        HttpContent CreateContent<T>(T value)
            where T : notnull;
    }
}
