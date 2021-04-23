namespace Consul.Net
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class ConsulClientTests
    {
        [TestMethod]
        public void ConstructingWithoutAgentResourcesShouldThrowArgumentNullException()
        {
            // Act
            Action constructingWithoutAgentResources =
                () => _ = new ConsulClient(default!);

            // Assert
            _ = constructingWithoutAgentResources.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("agentResources");
        }
    }
}
