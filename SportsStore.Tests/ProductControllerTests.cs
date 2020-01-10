using Moq;
using SportsStore.Managers;
using SportsStore.Models;
using System;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            Mock<IEFProductManager> mock = new Mock<IEFProductManager>();
        }
    }
}
