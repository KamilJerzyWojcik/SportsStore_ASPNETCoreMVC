using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Test
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //przygotowanie
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            //przygotowanie - utworzenie pustego koszyka
            Cart cart = new Cart();
            //przygotowanie - utworzenie zamówienia
            Order order = new Order();
            //przygotowanie - utworzenie egzemplarza kontrolera
            OrderController target = new OrderController(mock.Object, cart);

            //działanie
            ViewResult result = target.Checkout(order) as ViewResult;

            //Assercje
            //czy zamówienie zostało umieszczone w repozytorium
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //czy metoda zwraca domyślny widok
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //czy przekazujemy prawidłowy model do widoku
            Assert.False(result.ViewData.ModelState.IsValid);


        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //przygotowanie
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //przygotowanie - utworzenie pustego koszyka
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //przygotowanie - utworzenie egzemplarza kontrolera
            OrderController target = new OrderController(mock.Object, cart);
            //dodanie błędu do modelu
            target.ModelState.AddModelError("error", "error");

            //Działanie - próba zakończenia zamówienia
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            //Assercje
            //sprawdzenie czy zamówienie nie zostało przekazane do repozytoriu
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //czy metoda zwraca domyślny widok
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //czy przekazujemy nieprawidłowy model do widoku
            Assert.False(result.ViewData.ModelState.IsValid);

        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //przygotowanie
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            //przygotowanie - utworzenie pustego koszyka
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //przygotowanie - utworzenie zamówienia
            Order order = new Order();
            //przygotowanie - utworzenie egzemplarza kontrolera
            OrderController target = new OrderController(mock.Object, cart);

            //Działanie - próba zakończenia zamówienia
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            //Asercje
            //czy zamówienie nie zostało przekazane do repozytorium
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            //czy metoda przekierowywuje do metody akcji Completed()
            Assert.Equal("Completed", result.ActionName);
        }

        }
}
