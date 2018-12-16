using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportStore.Models.ViewModels;
using System.Collections.Generic;

namespace SportStore.Infrastructure
{
	[HtmlTargetElement("div", Attributes = "pagination")]
	public class PageLinkTagHelper : TagHelper
	{
		private IUrlHelperFactory _urlHelperFactory;

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }

		public PaginationInfo Pagination { get; set; }
		public string PageAction { get; set; }

		[HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
		public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

		public bool PaginationClassesEnabled { get; set; } = false;
		public string PaginationClass { get; set; }
		public string PaginationClassNormal { get; set; }
		public string PaginationClassSelected { get; set; }

		public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
		{
			_urlHelperFactory = urlHelperFactory;
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
			TagBuilder paginationDiv = new TagBuilder("div");

			for(int i = 1; i <= Pagination.Pages; i++)
			{
				TagBuilder tagA = new TagBuilder("a");

				PageUrlValues["productPage"] = i;

				tagA.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

				if(PaginationClassesEnabled)
				{
					tagA.AddCssClass(PaginationClass);
					tagA.AddCssClass(i == Pagination.CurrentPage ? PaginationClassSelected : PaginationClassNormal);
				}

				tagA.InnerHtml.Append(i.ToString());
				paginationDiv.InnerHtml.AppendHtml(tagA);
			}
			output.Content.AppendHtml(paginationDiv.InnerHtml);
		}
	}
}
