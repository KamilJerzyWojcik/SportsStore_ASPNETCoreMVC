using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;


namespace SportsStore.Test
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //przygotowanie
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns((new Product[]{
                new Product {ProductID = 1, Name = "P1", Category = "Jablka"},
                new Product {ProductID = 2, Name = "P2", Category = "Jablka"},
                new Product {ProductID = 3, Name = "P3", Category = "Sliwki"},
                new Product {ProductID = 4, Name = "P4", Category = "Pomarancze"}
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            //Działanie, pobranie zbioru kategorii 
            string[] results = ((IEnumerable<string>)(target.Invoke()
                as ViewViewComponentResult).ViewData.Model).ToArray();

            //Asercje, porównanie z prawidłową tablicą
            Assert.True(Enumerable.SequenceEqual(new string[] { "Jablka", "Pomarancze", "Sliwki" }, results));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            //przygotowanie
            string categoryToSelect = "Jablka";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Jablka"},
                new Product {ProductID = 2, Name = "P2", Category = "Pomarancze"}
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            //działanie
            string result = (string)(target.Invoke() as
                ViewViewComponentResult).ViewData["SelectedCategory"];

            //Asercje
            Assert.Equal(categoryToSelect, result);
        }

    }
}
