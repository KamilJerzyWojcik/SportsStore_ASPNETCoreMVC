﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.ViewModels
{
	public class ProductsListViewModel
	{
		public IEnumerable<Product> Products { get; set; }
		public PaginationInfo PaginationInfo { get; set; }
		public string CurrentCategory { get; set; }
	}
}
