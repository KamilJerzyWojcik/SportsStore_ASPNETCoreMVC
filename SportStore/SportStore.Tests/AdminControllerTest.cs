using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
	public class AdminControllerTest
	{
		[Fact]
		public void Index_Contains_All_Products()
		{
			//przygotowanie - tworzenie repozytorium
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID  = 3, Name = "P3"}
			}.AsQueryable<Product>);

			//Przygotowanie - utworzenie kontrolera
			AdminController adminController = new AdminController(mock.Object);

			//Dzialanie
			Product[] products = GetViewModel<IEnumerable<Product>>(adminController.Index())?.ToArray();

			//Assercje
			Assert.Equal(3, products.Length);
			Assert.Equal("P1", products[0].Name);
			Assert.Equal("P2", products[1].Name);
			Assert.Equal("P3", products[2].Name);
		}

		[Fact]
		public void Can_Edit_Product()
		{
			//Przygotowanie - imitacja repozutorium
			Mock<IProductRepository> mock = new Mock<IProductRepository>();


			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID  = 3, Name = "P3"}
			}.AsQueryable<Product>);

			//przygotowanie - utworzenie kontrolera
			AdminController adminController = new AdminController(mock.Object);

			//Działanie
			Product p1 = GetViewModel<Product>(adminController.Edit(1));
			Product p2 = GetViewModel<Product>(adminController.Edit(2));
			Product p3 = GetViewModel<Product>(adminController.Edit(3));

			//Asercje
			Assert.Equal(1, p1.ProductID);
			Assert.Equal(2, p2.ProductID);
			Assert.Equal(3, p3.ProductID);
		}

		[Fact]
		public void Cannot_Edit_Nonexistent_Product()
		{
			//Przygotowanie - imitacja repozytorium
			Mock<IProductRepository> mock = new Mock<IProductRepository>();

			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1"},
				new Product {ProductID = 2, Name = "P2"},
				new Product {ProductID  = 3, Name = "P3"}

			}.AsQueryable<Product>);

			//przygotowanie - utworzenie kontrollera
			AdminController adminController = new AdminController(mock.Object);

			//działanie
			Product product = GetViewModel<Product>(adminController.Edit(4));

			//assercje
			Assert.Null(product);
		}

		[Fact]
		public void Can_Save_Valid_Chnges()
		{
			//Przygotowanie - imitacja repozytorium
			Mock<IProductRepository> mock = new Mock<IProductRepository>();

			//przygotowanie - imitacja TempData
			Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

			//Przygotowanie - tworzenie kontrollera
			AdminController adminController = new AdminController(mock.Object)
			{
				TempData = tempData.Object
			};

			//przygotowanie - tworzenieproduktu
			Product product = new Product() { Name = "test" };

			//Działanie - próba zapisania produktu
			IActionResult actionResult = adminController.Edit(product);

			//Assercje - czy zostalo wywołane repozytorium
			mock.Verify(m => m.SaveProduct(product));

			//Asercje - sprawdzenie typu zwracanego z metody
			Assert.IsType<RedirectToActionResult>(actionResult);
			Assert.Equal("Index", (actionResult as RedirectToActionResult).ActionName);
		}

		[Fact]
		public void Cannot_Save_invalid_Changes()
		{
			//tworzenie imitacji repozytorium
			Mock<IProductRepository> mock = new Mock<IProductRepository>();

			//przygotowanie - utworzenie kontrollera
			AdminController adminController = new AdminController(mock.Object);

			//przygotowanie - utworzenie produktu
			Product product = new Product() { Name = "Test" };

			//przygotowanie - dodanie będu do stanu modelu
			adminController.ModelState.AddModelError("error", "error");

			//działanie - próba zapisu produktu
			IActionResult actionResult = adminController.Edit(product);

			//assercje - sprawdzenie czy zostało wywołane repozytorium
			mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
			//assercje - sprawdzenie typu zwranego z metody
			Assert.IsType<ViewResult>(actionResult);
		}

		[Fact]
		public void Can_Delete_Valid_Products()
		{
			//przygotowanie - tworzenie produktu
			Product product = new Product { ProductID = 2, Name = "Test" };

			//przygotowanie - imitacja repozytorium
			Mock<IProductRepository> mock = new Mock<IProductRepository>();

			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1"},
				product,
				new Product {ProductID = 3, Name = "P2"}

			}.AsQueryable<Product>());

			//przygotowanie - tworzenie kontrollera
			AdminController adminController = new AdminController(mock.Object);

			//działanie - usunięcie produktu
			adminController.Delete(product.ProductID);

			//Asercje - upewnienie się ze metoda repozytorium wywołana z właściwym produktem
			mock.Verify(m => m.DeleteProduct(product.ProductID));

		}

		//typ może być tylko klasą (where T : class), inne to np. struct
		private T GetViewModel<T>(IActionResult result) where T : class
		{
			return (result as ViewResult)?.ViewData.Model as T;
		}
	}
}
