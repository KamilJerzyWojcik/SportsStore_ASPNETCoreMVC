using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportStore.Components;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
	public class NavigationMenuViewComponentTests
	{
		Mock<IProductRepository> mock = new Mock<IProductRepository>();

		[Fact]
		public void Can_Select_Categories()
		{
			//przygotwanie
			mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = "Jablka"},
				new Product {ProductID = 2, Name = "P2", Category = "Jablka"},
				new Product {ProductID = 3, Name = "P3", Category = "Sliwki"},
				new Product {ProductID = 4, Name = "P4", Category = "Pomarancze"}
			}).AsQueryable<Product>());

			NavigationMenuViewComponent navigationMenuViewComponent = new NavigationMenuViewComponent(mock.Object);
			navigationMenuViewComponent.ViewComponentContext = new ViewComponentContext
			{
				ViewContext = new ViewContext
				{
					RouteData = new RouteData()
				}
			};
			navigationMenuViewComponent.RouteData.Values["category"] = "Pomarancze";

			//działanie
			string[] OrderedCategory = ((IEnumerable<string>)(navigationMenuViewComponent.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

			//Assercje
			Assert.True(Enumerable.SequenceEqual(new string[] { "Jablka", "Pomarancze", "Sliwki" }, OrderedCategory));
		}

		[Fact]
		public void Indicates_Selected_Category()
		{
			//Przygotowanie
			string selectedCategory = "Jablka";
			mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = "Jablka"},
				new Product {ProductID = 1, Name = "P2", Category = "Pomarancze"}

			}).AsQueryable<Product>);

			NavigationMenuViewComponent navigationMenuViewComponent = new NavigationMenuViewComponent(mock.Object);
			navigationMenuViewComponent.ViewComponentContext = new ViewComponentContext
			{
				ViewContext = new ViewContext
				{
					RouteData = new RouteData()
				}

			};
			navigationMenuViewComponent.RouteData.Values["category"] = selectedCategory;

			//Działanie
			string currentCategory = (string)(navigationMenuViewComponent.Invoke() as ViewViewComponentResult).ViewData["currentCategory"];

			//Asercje
			Assert.Equal(selectedCategory, currentCategory);
		}
	}
}
