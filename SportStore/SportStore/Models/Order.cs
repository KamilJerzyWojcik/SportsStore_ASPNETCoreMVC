﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
	public class Order
	{
		[BindNever]//brak możliwości zmiany odstrony użytkownika
		public int OrderId { get; set; }

		[BindNever]
		public ICollection<CartLine> Lines { get; set; }

		[BindNever]
		public bool Shipped { get; set; }

		[Required(ErrorMessage = "Proszę podać imię i nazwisko.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Poroszę podać pierwszy wiersz adresu.")]
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }

		[Required(ErrorMessage = "Proszę podać nazwę miasta.")]
		public string City { get; set; }

		[Required(ErrorMessage = "Proszę podać nazwę województwa.")]
		public string State { get; set; }

		public string Zip { get; set; }

		[Required(ErrorMessage = "Proszę podać nazwę kraju.")]
		public string Country { get; set; }

		public bool GiftWrap { get; set; }
	}
}