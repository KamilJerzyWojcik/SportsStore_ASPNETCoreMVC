using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure
{
    
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")] //okreslenie prefiksu dla nazw atrybutow elementu
        //wartosc dowolnego atrybutu ktorego nazwa zaczyna się od prefiksu zostanie dodana do słownika PageUrlValues
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; }//wartość bool do określenia stylu linku (brawny - aktywny, bezbarwny - nieaktywny)
        public string PageClassNormal { get; set; } //styl dla nieaktywnego linku
        public string PageClassSelected { get; set; }//styl dla aktywnego linku

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                PageUrlValues["productPage"] = i;
                //przekazanie słownika PageUrlValues w metodzie Action() w celu wygenerowania adresów URL atrybutów href elementów a
                tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = i });
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);

                if(PageClassesEnabled)
                {
                    tag.AddCssClass(i == PageModel.CurrentPage ? 
                        PageClassSelected : PageClassNormal); //określenie czy w pagiancji link jest aktywny czy nie 
                }
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}

