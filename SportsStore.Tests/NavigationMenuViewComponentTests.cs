using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Indicate_Selected_Category()
        {
            string categorySelect = "cat1";
            Mock<IEFProductManager> mock = new Mock<IEFProductManager>();
            mock.Setup(x => x.GetCategories()).Returns(new List<string> {"cat1", "cat2"});

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext {
                ViewContext = new ViewContext { RouteData = new RouteData() }
            };
            target.RouteData.Values["category"] = categorySelect;

            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            Assert.Equal(categorySelect, result);

        }
    }
}
