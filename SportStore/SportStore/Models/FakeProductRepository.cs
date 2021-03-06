﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
	public class FakeProductRepository /*: IProductRepository*/
	{
		public IQueryable<Product> Products
		{
			get
			{
				var list = new List<Product>
				{
					new Product {Name = "Piłka nożna", Price = 25},
					new Product {Name= "Deska surfingowa", Price = 179},
					new Product {Name = "Buty do biegania", Price = 95}
				};

				return list.AsQueryable();
			}
		}
	}
}
