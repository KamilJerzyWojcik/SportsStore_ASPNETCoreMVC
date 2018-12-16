using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using Xunit;

namespace SportStore.Tests
{
	public class OrderControllerTests
	{
		[Fact]
		public void Cannot_Checkout_Empty_Cart()
		{
			//Przygotowanie - imitacja repo
			Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

			//przygotowanie - pusty koszyk
			Cart cart = new Cart();

			//przygotowanie - utworzenie pustego koszyka
			Order order = new Order();
			
			//przygotowanie - egzemplarz kontrolera
			OrderController orderController = new OrderController(mock.Object, cart);

			//działanie
			ViewResult result = orderController.Checkout(order) as ViewResult;

			//assercje - czy zamowienie zostalo umieszczone w repozytorium
			mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

			//assercje - czy metoda zwraca domyślny widok
			Assert.True(string.IsNullOrEmpty(result.ViewName));

			//assercje - czy przekazujemy prawidłowy model do widoku
			Assert.False(result.ViewData.ModelState.IsValid);

		}

		[Fact]
		public void Cannot_Checkout_Invalid_ShippingDetails()
		{
			//przygotowanie - tworzenie imitacji
			Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

			//przygotowanie produkt w koszyku
			Cart cart = new Cart();

			cart.AddItem(new Product(), 1);

			//przgotowanie - tworzenie kontrollera
			OrderController orderController = new OrderController(mock.Object, cart);

			//przygotowanie - dodanie błędu
			orderController.ModelState.AddModelError("error", "error");

			//działanie - próba zakonczenia zamownienia
			ViewResult viewResult = orderController.Checkout(new Order()) as ViewResult;

			//asercje - sprawdzenie czy zamówienie nie zostało przekazane do repozytorium
			mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

			//asercje - sprawdzenie czy metoda zwraca domyślny widok
			Assert.True(string.IsNullOrEmpty(viewResult.ViewName));

			//assercje - czy przekazujemy nieprawidlowy model do widoku
			Assert.False(viewResult.ViewData.ModelState.IsValid);

		}

		[Fact]
		public void Can_Checkout_And_Submit_Order()
		{
			//przygotowanie - tworzenie imitacji repozytorium
			Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

			//Przygotowanie - tworzenie koszyka z produktem
			Cart cart = new Cart();
			cart.AddItem(new Product(), 1);

			//przygotowanie - tworzenie egzemplarza kontrolera
			OrderController orderController = new OrderController(mock.Object, cart);

			//działanie - próba zakonczenia zamówienia
			RedirectToActionResult redirectToAction = orderController.Checkout(new Order()) as RedirectToActionResult;

			//Asercje - czy zamówienie nie zostało przekazane do repozytorium
			mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

			//Assercje - czy metoda przekierowuwuje do metody akcji completed()
			Assert.Equal("Completed", redirectToAction.ActionName);

		}
	}
}
