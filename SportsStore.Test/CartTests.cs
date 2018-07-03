using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Test
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //przygotowanir - utworzenie produktow testowych
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            //Przygotowanie - utworzenie nowego koszyka
            Cart target = new Cart();

            //Działanie
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            //Asercje
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //przygotowanie - tworzenie produktow testowych
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            //przygotowanie - utworzenie nowego koszyka
            Cart target = new Cart();

            //Dzialanie
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            //Asercje
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //przygotowanie - tworzenie produktow testowych
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            //przygotowanie - utworzenie nowego koszyka
            Cart target = new Cart();

            //przygotowanie - dodanie kilku produktów do koszyka
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            //działanie
            target.RemoveLine(p2);

            //Asercje
            Assert.Equal(0, target.Lines.Where(c => c.Product == p2).Count());
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            //przygotowanie - tworzenie produktow testowych
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            //przygotowanie - utworznie koszyka
            Cart target = new Cart();

            //działanie
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            //Asercje
            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            //przygotowanie - tworzenie produktow testowych
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            //przygotowanie - utworzenie koszyka
            Cart target = new Cart();

            //przygotowanie - dodanie produktów
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            //działanie
            target.Clear();

            //Assercje
            Assert.Equal(0, target.Lines.Count());
        }
    }
}
