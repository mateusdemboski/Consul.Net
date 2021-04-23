namespace Consul.Net
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public sealed class AgentResourcesTests
    {
        [TestMethod]
        public void ConstructingWithoutServiceResourcesShouldThrowArgumentNullException()
        {
            // Act
            Action constructingWithoutServiceResources =
                () => _ = new AgentResources(default!);

            // Assert
            _ = constructingWithoutServiceResources.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("serviceResources");
        }
    }
}
