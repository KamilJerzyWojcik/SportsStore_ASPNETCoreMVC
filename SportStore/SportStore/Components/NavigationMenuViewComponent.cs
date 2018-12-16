using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Components
{
	public class NavigationMenuViewComponent : ViewComponent
	{
		private IProductRepository _repository;

		public NavigationMenuViewComponent(IProductRepository repository)
		{
			_repository = repository;
		}

		public IViewComponentResult Invoke()
		{
			ViewBag.currentCategory = RouteData.Values["category"];

			IEnumerable<string> category = _repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);

			return View(category);
		}
	}
}
