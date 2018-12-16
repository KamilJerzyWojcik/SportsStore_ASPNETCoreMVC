using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using SportStore.Models.ViewModels;

namespace SportStore.Controllers
{
	public class ProductController : Controller
	{
		private IProductRepository repository;
		public int PageSize = 4;

		public ProductController(IProductRepository repo)
		{
			repository = repo;
		}

		public ViewResult List(string category,  int productPage = 1)
		{
			IEnumerable<Product> products = repository.Products.Where(p => category == null || p.Category == category).OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize);

			PaginationInfo paginationInfo = new PaginationInfo
			{
				CurrentPage = productPage,
				ItemsPerPage = PageSize,
				TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(p => p.Category == category).Count()
			};

			ProductsListViewModel productsListViewModel = new ProductsListViewModel
			{
				Products = products,
				PaginationInfo = paginationInfo,
				CurrentCategory = category
			};

			return View(productsListViewModel);
		}
	}
}
