using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportStore.Infrastructure;
using SportStore.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportStore.Tests
{
	public class PageLinkTagHeperTests
	{
		[Fact]
		public void Can_Generate_Page_Links()
		{
			//przygotowanie
			Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
			urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>())).Returns("Test/Page1").Returns("Test/Page2").Returns("Test/Page3");

			Mock<IUrlHelperFactory> urlHelperFactory = new Mock<IUrlHelperFactory>();
			urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

			PageLinkTagHelper pageLinkTagHelper = new PageLinkTagHelper(urlHelperFactory.Object)
			{
				Pagination = new PaginationInfo()
				{
					CurrentPage = 2,
					TotalItems = 28,
					ItemsPerPage = 10
				},
				PageAction = "Test"
			};

			TagHelperContext tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");

			Mock<TagHelperContent> tagHelperContent = new Mock<TagHelperContent>();
			TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(tagHelperContent.Object));

			//działanie
			pageLinkTagHelper.Process(tagHelperContext, output);

			//Assercje
			Assert.Equal(@"<a href=""Test/Page1"">1</a><a href=""Test/Page2"">2</a><a href=""Test/Page3"">3</a>", output.Content.GetContent());
		}
	}
}
