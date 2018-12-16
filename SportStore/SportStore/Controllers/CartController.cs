using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportStore.Infrastructure;
using SportStore.Models;

namespace SportStore.Controllers
{
	public class CartController : Controller
	{
		private IProductRepository _repository;
		private Cart _cart;

		public CartController(IProductRepository repository, Cart cart)
		{
			_repository = repository;
			_cart = cart;
		}

		public ActionResult Index(string returnUrl)
		{
			CartIndexViewModel cartIndexViewModel = new CartIndexViewModel
			{
				Cart = _cart,
				ReturnUrl = returnUrl
			};

			return View(cartIndexViewModel);
		}

		public RedirectToActionResult AddToCart(int productID, string returnUrl)
		{
			Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productID);

			if (product != null)
				_cart.AddItem(product, 1);

			return RedirectToAction("Index", new { returnUrl });
		}

		public RedirectToActionResult RemoveFromCart(int productID, string returnUrl)
		{
			Product product = _repository.Products.SingleOrDefault(p => p.ProductID == productID);

			if (product != null)
				_cart.RemoveItem(product);

			return RedirectToAction("Index", new { returnUrl });
		}
	}
}
