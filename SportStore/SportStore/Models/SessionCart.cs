﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
	public class SessionCart : Cart
	{
		[JsonIgnore]
		public ISession Session { get; set; }

		public static Cart GetCart(IServiceProvider services)
		{
			ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
			SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
			cart.Session = session;
			return cart;
		}

		public override void AddItem(Product product, int quantity)
		{
			base.AddItem(product, quantity);
			Session.SetJson("Cart", this);
		}

		public override void RemoveItem(Product product)
		{
			base.RemoveItem(product);
			Session.SetJson("Cart", this);
		}

		public override void ClearCart()
		{
			base.ClearCart();
			Session.Remove("Cart");
		}
	}
}
