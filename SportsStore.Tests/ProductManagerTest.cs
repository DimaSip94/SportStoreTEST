using Microsoft.Extensions.Configuration;
using SportsStore.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductManagerTest
    {
        [Fact]
        public void Can_GetProduct()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("client-secrets.json")
                .Build();

            IEFProductManager eFProductManager = new EFProductManager(config["DbConnection"], "");
            var productSucsses = eFProductManager.GetProduct(3);
            Assert.Equal(3, productSucsses.ProductID);

            productSucsses = eFProductManager.GetProduct(15);
            if (productSucsses == null)
            {
                productSucsses = new Models.Product { ProductID=0 };
            }
            Assert.Equal(0, productSucsses.ProductID);
        }
    }
}
