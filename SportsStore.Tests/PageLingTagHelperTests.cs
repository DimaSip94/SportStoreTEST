using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
    public class PageLingTagHelperTests
    {
        [Fact]
        public void Can_Generate_PageLinks()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f =>
            f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);

            PageLingTagHelper helper =
                new PageLingTagHelper(urlHelperFactory.Object) { 
                    PageModel = new PagingInfo
                    {
                        CurrentPage = 2,
                        TotalItems = 28,
                        ItemsPerPage = 10
                    },
                    PageAction = "Test"
                };
            TagHelperContext ctx = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), "");

            var content = new Mock<TagHelperContent>();
            TagHelperOutput tagHelperOutput = new TagHelperOutput("div",
                new TagHelperAttributeList(),
                (cache, encoder)=> Task.FromResult(content.Object));
            helper.Process(ctx, tagHelperOutput);
            Assert.Equal(@"<a href =""Test/Pagel"" >1</a>" + @"<а href =""Test/Page2"">2</a>", tagHelperOutput.Content.GetContent());
        }
    }
}
