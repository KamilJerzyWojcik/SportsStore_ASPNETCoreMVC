using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportStore.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private IProductRepository repository;

		public AdminController(IProductRepository repository)
		{
			this.repository = repository;
		}

		public ViewResult Index()
		{
			return View(repository.Products);
		}

		[HttpGet]
		public ViewResult Edit(int productId)
		{
			Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
			return View(product);
		}

		[HttpPost]
		public IActionResult Edit(Product product)
		{
			if(ModelState.IsValid)
			{
				repository.SaveProduct(product);
				TempData["message"] = $"Zapisano {product.Name}";
				return RedirectToAction("Index");
			}
			else
			{
				return View(product);
			}
		}

		[HttpGet]
		public ViewResult Create()
		{
			return View("Edit", new Product());
		}

		[HttpPost]
		public IActionResult Delete(int productId)
		{
			Product deletedProduct = repository.DeleteProduct(productId);
			if(deletedProduct != null)
			{
				TempData["message"] = $"Usunięto {deletedProduct.Name}";
			}
			return RedirectToAction("Index");
		}

	}
}
