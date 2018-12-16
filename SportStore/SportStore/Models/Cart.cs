using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
	public class Cart
	{
		private List<CartLine> cartLineCollection = new List<CartLine>();


		public virtual IEnumerable<CartLine> LinesCollection
		{
			get
			{
				return cartLineCollection;
			}
		}

		public virtual void AddItem(Product product, int quantity)
		{
			CartLine cartLine = cartLineCollection.Where(p => p.Product.ProductID == product.ProductID).FirstOrDefault();

			if (cartLine == null)
			{
				cartLine = new CartLine
				{
					Product = product,
					Quantity = quantity
				};

				cartLineCollection.Add(cartLine);
			}
			else
			{
				cartLine.Quantity += quantity;
			}

		}

		public virtual void RemoveItem(Product product)
		{
			cartLineCollection.RemoveAll(p => p.Product.ProductID == product.ProductID);
		}

		public virtual decimal ComputeTotalValue()
		{
			decimal totalValue = cartLineCollection.Sum(p => p.Quantity * p.Product.Price);

			return totalValue;
		}

		public virtual void ClearCart()
		{
			cartLineCollection.Clear();
		}

	}
}
