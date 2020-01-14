using SportsStore.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductManagerTest
    {
        private readonly string connection = "Server=LAPTOP-I0KLMA05\\SQLEXPRESS;Database=sportsStore;User Id=dima;Password=123456;MultipleActiveResultSets=true";
        [Fact]
        public void Can_GetProduct()
        {
            IEFProductManager eFProductManager = new EFProductManager(connection,"");
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
