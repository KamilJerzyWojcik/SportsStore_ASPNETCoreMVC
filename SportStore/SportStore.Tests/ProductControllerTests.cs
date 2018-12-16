using Moq;
using SportStore.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SportStore.Models;
using SportStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SportStore.Tests
{
	public class ProductControllerTests
	{
		Func<ViewResult, ProductsListViewModel> GetModel = gm => gm?.ViewData.Model as ProductsListViewModel;
		Mock<IProductRepository> mock = new Mock<IProductRepository>();

		[Fact]
		public void Can_Send_Pagination_View_Model()
		{
			//przygotwanie			
			mock.Setup(m => m.Products).Returns((new Product[] {

				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
				new Product {ProductID = 4, Name = "P4"},
				new Product {ProductID = 5, Name = "P5"}

			}).AsQueryable<Product>());

			//Przygotowanie 2
			ProductController productController = new ProductController(mock.Object) { PageSize = 3 };

			//działanie
			ProductsListViewModel productsListViewModel = GetModel(productController.List(null, 2));

			//assercje
			PaginationInfo paginationInfo = productsListViewModel.PaginationInfo;
			Assert.Equal(2, paginationInfo.CurrentPage);
			Assert.Equal(3, paginationInfo.ItemsPerPage);
			Assert.Equal(5, paginationInfo.TotalItems);
			Assert.Equal(2, paginationInfo.Pages);
		}

		[Fact]
		public void Can_Paginate()
		{
			//przygotowanie
			mock.Setup(r => r.Products).Returns((new Product[] {

				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID = 3, Name = "P3"},
				new Product {ProductID = 4, Name = "P4"},
				new Product {ProductID = 5, Name = "P5"}

			}).AsQueryable<Product>());

			ProductController productController = new ProductController(mock.Object) { PageSize = 3};

			//działanie
			Product[] productArray = GetModel(productController.List(null, 2))?.Products.ToArray();

			//Asercje
			Assert.True(productArray.Length == 2);
			Assert.Equal("P4", productArray[0].Name);
			Assert.Equal("P5", productArray[1].Name);
		}

		[Fact]
		public void Can_Filter_Product()
		{
			//przygotowanie
			mock.Setup(r => r.Products).Returns((new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
				new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
				new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
				new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
				new Product {ProductID = 5, Name = "P5", Category = "Cat1"}
			}).AsQueryable<Product>());

			//przygotowanie
			ProductController controller = new ProductController(mock.Object) { PageSize = 3};

			//działanie
			Product[] result = GetModel(controller.List("Cat2", 1))?.Products.ToArray();

			//Assercje
			Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
			Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
		}

		[Fact]
		public void Generate_Category_Specific_Product_Count()
		{
			//przygotowanie
			mock.Setup(r => r.Products).Returns((new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
				new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
				new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
				new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
				new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
			}).AsQueryable<Product>());

			ProductController productController = new ProductController(mock.Object) { PageSize = 3};

			//Działanie
			int? res1 = GetModel(productController.List("Cat1"))?.PaginationInfo.TotalItems;
			int? res2 = GetModel(productController.List("Cat2"))?.PaginationInfo.TotalItems;
			int? res3 = GetModel(productController.List("Cat3"))?.PaginationInfo.TotalItems;
			int? resAll = GetModel(productController.List(null))?.PaginationInfo.TotalItems;

			//Assercje
			Assert.Equal(2, res1);
			Assert.Equal(2, res2);
			Assert.Equal(1, res3);
			Assert.Equal(5, resAll);
		}
	}
}
