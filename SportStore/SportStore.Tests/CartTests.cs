using SportStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportStore.Tests
{
	public class CartTests
	{
		[Fact]
		public void Can_Add_New_Lines()
		{
			//przygotowanie
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			Cart cart = new Cart();

			//działanie
			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			CartLine[] cartLine = ((List<CartLine>)cart.LinesCollection).ToArray();

			//Assercje
			Assert.Equal(2, cartLine.Length);
			Assert.Equal(p1, cartLine[0].Product);
			Assert.Equal(p2, cartLine[1].Product);
		}

		[Fact]
		public void Can_Add_Quantity_For_Existing_Lines()
		{
			//przygotowanie
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			Cart cart = new Cart();

			//Działanie
			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			CartLine[] cartLine = cart.LinesCollection.OrderBy(c => c.Product.ProductID).ToArray();

			//Assercje
			Assert.Equal(2, cartLine.Length);
			Assert.Equal(11, cartLine[0].Quantity);
			Assert.Equal(1, cartLine[1].Quantity);
		}

		[Fact]
		public void Can_Remove_Line()
		{
			//przygotowanie
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			Cart cart = new Cart();

			//Działanie
			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);
			cart.RemoveItem(p1);

			CartLine[] cartLine = cart.LinesCollection.OrderBy(c => c.Product.ProductID).ToArray();

			//Assercje
			Assert.Single(cartLine);
			Assert.Equal(1, cartLine[0].Quantity);
			Assert.Empty(cartLine.Where(p => p.Product.ProductID == 1));
		}

		[Fact]
		public void Calculate_Cart_Total()
		{
			//przygotowanie
			Product p1 = new Product { ProductID = 1, Name = "P1", Price = 12.5M };
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 20M };

			Cart cart = new Cart();

			//Działanie
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			decimal? total = cart.ComputeTotalValue();

			//Assercja
			Assert.Equal(145M, total);
		}

		[Fact]
		public void Can_Clear_Contents()
		{
			//przygotowanie
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			Cart cart = new Cart();

			//Działanie
			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);
			cart.ClearCart();

			//Assercja
			Assert.Empty(cart.LinesCollection);
		}
	}
}
