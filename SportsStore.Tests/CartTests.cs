using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_LineItem()
        {
            Product p1 = new Product { Name = "1", Price = 2, ProductID = 1 };
            Product p2 = new Product { Name = "2", Price = 3, ProductID = 2 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            CartLine[] results = target.Lines.ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Remove_LineItem()
        {
            Product p1 = new Product { Name = "1", Price = 2, ProductID = 1 };
            Product p2 = new Product { Name = "2", Price = 3, ProductID = 2 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.RemoveItem(p1);
            CartLine[] results = target.Lines.ToArray();

            Assert.Equal(1, results.Length);
            Assert.Equal(p2, results[0].Product);
        }

        [Fact]
        public void Check_TotalValue_Cart()
        {
            Product p1 = new Product { Name = "1", Price = 2, ProductID = 1 };
            Product p2 = new Product { Name = "2", Price = 3, ProductID = 2 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            decimal total = target.ComputeTotalValue();

            Assert.Equal(5, total);
        }

        [Fact]
        public void Can_Clear_Cart()
        {
            Product p1 = new Product { Name = "1", Price = 2, ProductID = 1 };
            Product p2 = new Product { Name = "2", Price = 3, ProductID = 2 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.Clear();
            Assert.Equal(0, target.Lines.Count());
        }
    }
}
