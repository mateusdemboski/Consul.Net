namespace Consul.Net
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul.Net.Models;
    using Consul.Net.Util.Http;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using static System.Net.Http.HttpMethod;
    using static System.Net.HttpStatusCode;

    [TestClass]
    public sealed class AgentServiceResourcesTests
    {
        [TestMethod]
        public void ConstructingWithoutHttpClientShouldThrowArgumentNullException()
        {
            // Act
            Action constructingWithoutHttpClient =
                () => _ = new AgentServiceResources(default!, Substitute.For<IHttpContentFactory>());

            // Assert
            _ = constructingWithoutHttpClient.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("httpClient");
        }

        [TestMethod]
        public void ConstructingWithoutHttpContentFactoryShouldThrowArgumentNullException()
        {
            // Act
            Action constructingWithoutHttpContentFactory =
                () => _ = new AgentServiceResources(new HttpClient(), default!);

            // Assert
            _ = constructingWithoutHttpContentFactory.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("httpContentFactory");
        }

        [TestMethod]
        public void RegisteringWithoutAgentServiceRegisterShouldThrowArgumentNullException()
        {
            // Arrange
            var resource = CreateResource();

            // Act
            Func<Task> registeringWithoutAgentServiceRegister =
                async () => await resource.RegisterAsync(default!).ConfigureAwait(false);

            // Assert
            _ = registeringWithoutAgentServiceRegister.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("agentServiceRegister");
        }

        [TestMethod]
        public async Task RegisteringShouldPutRequestOnConsulAsync()
        {
            // Arrange
            var agentServiceRegister = new AgentServiceRegister();

            using var emptyContent = new StringContent(string.Empty);

            var httpContentFactory = Substitute.For<IHttpContentFactory>();
            _ = httpContentFactory
                .CreateContent(agentServiceRegister)
                .ReturnsForAnyArgs(emptyContent);

            using var messageHandler = Substitute.For<HttpMessageHandler>();
            using var okMessage = new HttpResponseMessage
            {
                Content = emptyContent,
                StatusCode = OK,
            };

#pragma warning disable NS1000 // Non-virtual setup specification.
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
            _ = messageHandler
                .Protected(
                    "SendAsync",
                    Arg.Is<HttpRequestMessage>(
                        request => request.Method == Put
                        && request.RequestUri == new Uri("http://test/v1/api/agent/service/register")
                        && request.Content == emptyContent),
                    Arg.Any<CancellationToken>())
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1000 // Non-virtual setup specification.
                .Returns(Task.FromResult(okMessage));

            using var httpClient = new HttpClient(messageHandler);
            httpClient.BaseAddress = new Uri("http://test/v1/api/agent/service/");

            var resource = CreateResource(httpClient, httpContentFactory);

            // Act
            Func<Task> register =
                async () =>
                    await resource
                        .RegisterAsync(agentServiceRegister)
                        .ConfigureAwait(false);

            // Assert
            await register.Should().NotThrowAsync().ConfigureAwait(false);
        }

        [DataTestMethod]
        [DataRow(BadRequest)]
        [DataRow(NotFound)]
        [DataRow(InternalServerError)]
        [DataRow(Forbidden)]
        public async Task RegisteringGivenUnsuccefullHttpResponseCodeShouldThrowHttpRequestExceptionWithExpectedErrorStatusCode(
            HttpStatusCode expectedErrorStatusCode)
        {
            // Arrange
            var agentServiceRegister = new AgentServiceRegister();

            using var emptyContent = new StringContent(string.Empty);
            using var messageHandler = Substitute.For<HttpMessageHandler>();
            using var errorMessage = new HttpResponseMessage
            {
                Content = emptyContent,
                StatusCode = expectedErrorStatusCode,
            };

            using var httpClient = new HttpClient(messageHandler);
            httpClient.BaseAddress = new Uri("http://test/v1/api/agent/service/");

            var httpContentFactory = Substitute.For<IHttpContentFactory>();
            _ = httpContentFactory
                .CreateContent(agentServiceRegister)
                .ReturnsForAnyArgs(emptyContent);

#pragma warning disable NS1000 // Non-virtual setup specification.
            _ = messageHandler
                .Protected("SendAsync", default!, default!)
#pragma warning restore NS1000 // Non-virtual setup specification.
                .ReturnsForAnyArgs(Task.FromResult(errorMessage));

            var resource = CreateResource(
                httpClient,
                httpContentFactory);

            // Act
            Func<Task> registeringGivenUnsuccefullHttpResponseCode =
                async () => await resource.RegisterAsync(agentServiceRegister).ConfigureAwait(false);

            // Assert
            _ = (await registeringGivenUnsuccefullHttpResponseCode
                    .Should()
                        .ThrowExactlyAsync<HttpRequestException>().ConfigureAwait(false))
                    .Which
                        .StatusCode.Should().Be(expectedErrorStatusCode);
        }
        /////////////////////////////////////////

        [TestMethod]
        public void DeregisteringWithoutServiceIdShouldThrowArgumentNullException()
        {
            // Arrange
            var resource = CreateResource();

            // Act
            Func<Task> registeringWithoutAgentServiceRegister =
                async () => await resource.DeregisterAsync(default!).ConfigureAwait(false);

            // Assert
            _ = registeringWithoutAgentServiceRegister.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("serviceId");
        }

        [TestMethod]
        public async Task DeregisteringShouldPutRequestOnConsulAsync()
        {
            // Arrange
            var agentServiceRegister = new AgentServiceRegister();

            using var emptyContent = new StringContent(string.Empty);

            using var messageHandler = Substitute.For<HttpMessageHandler>();
            using var okMessage = new HttpResponseMessage
            {
                Content = emptyContent,
                StatusCode = OK,
            };

#pragma warning disable NS1000 // Non-virtual setup specification.
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
            _ = messageHandler
                .Protected(
                    "SendAsync",
                    Arg.Is<HttpRequestMessage>(
                        request => request.Method == Put
                        && request.RequestUri == new Uri("http://test/v1/api/agent/service/deregister/service-id")
                        && request.Content == emptyContent),
                    Arg.Any<CancellationToken>())
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1000 // Non-virtual setup specification.
                .Returns(Task.FromResult(okMessage));

            using var httpClient = new HttpClient(messageHandler);
            httpClient.BaseAddress = new Uri("http://test/v1/api/agent/service/");

            var resource = CreateResource(httpClient);

            // Act
            Func<Task> deregister =
                async () =>
                    await resource
                        .DeregisterAsync("service-id")
                        .ConfigureAwait(false);

            // Assert
            await deregister.Should().NotThrowAsync().ConfigureAwait(false);
        }

        [DataTestMethod]
        [DataRow(BadRequest)]
        [DataRow(NotFound)]
        [DataRow(InternalServerError)]
        [DataRow(Forbidden)]
        public async Task DeregisteringGivenUnsuccefullHttpResponseCodeShouldThrowHttpRequestExceptionWithExpectedErrorStatusCode(
            HttpStatusCode expectedErrorStatusCode)
        {
            // Arrange
            var agentServiceRegister = new AgentServiceRegister();

            using var emptyContent = new StringContent(string.Empty);
            using var messageHandler = Substitute.For<HttpMessageHandler>();
            using var errorMessage = new HttpResponseMessage
            {
                Content = emptyContent,
                StatusCode = expectedErrorStatusCode,
            };

            using var httpClient = new HttpClient(messageHandler);
            httpClient.BaseAddress = new Uri("http://test/v1/api/agent/service/");

#pragma warning disable NS1000 // Non-virtual setup specification.
            _ = messageHandler
                .Protected("SendAsync", default!, default!)
#pragma warning restore NS1000 // Non-virtual setup specification.
                .ReturnsForAnyArgs(Task.FromResult(errorMessage));

            var resource = CreateResource(
                httpClient);

            // Act
            Func<Task> deregisteringGivenUnsuccefullHttpResponseCode =
                async () => await resource.RegisterAsync(agentServiceRegister).ConfigureAwait(false);

            // Assert
            _ = (await deregisteringGivenUnsuccefullHttpResponseCode
                    .Should()
                        .ThrowExactlyAsync<HttpRequestException>().ConfigureAwait(false))
                    .Which
                        .StatusCode.Should().Be(expectedErrorStatusCode);
        }

        private static IAgentServiceResources CreateResource(
            HttpClient? httpClient = default,
            IHttpContentFactory? httpContentFactory = default)
            => new AgentServiceResources(
                httpClient ?? Substitute.For<HttpClient>(),
                httpContentFactory ?? Substitute.For<IHttpContentFactory>());
    }
}
