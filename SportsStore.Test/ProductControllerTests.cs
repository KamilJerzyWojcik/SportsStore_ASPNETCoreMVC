using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SportsStore.Test
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //przygotowanie
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns((
                new Product[]{
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>);

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //działanie
            ProductsListViewModel result = 
                     controller.List(2).ViewData.Model as ProductsListViewModel;

            //Asercje
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //przygotowanie
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((
                new Product[]{
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };

            //działanie
            ProductsListViewModel result =
                controller.List(2).ViewData.Model as ProductsListViewModel;

            //asercje
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}
