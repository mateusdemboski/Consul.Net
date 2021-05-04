namespace Consul.Net
{
    using System;
    using System.Linq;
    using Consul.Net.Models;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public sealed class AgentResourcesTests
    {
        [TestMethod]
        public void ConstructingWithoutServiceResourcesShouldThrowArgumentNullException()
        {
            // Act
            Action constructingWithoutServiceResources =
                () => _ = new AgentResources(default!, Substitute.For<IQueryable<AgentService>>());

            // Assert
            _ = constructingWithoutServiceResources.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("serviceResources");
        }

        [TestMethod]
        public void ConstructingWithoutServicesShouldThrowArgumentNullException()
        {
            // Act
            Action constructingWithoutServices =
                () => _ = new AgentResources(Substitute.For<IAgentServiceResources>(), default!);

            // Assert
            _ = constructingWithoutServices.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("services");
        }
    }
}
