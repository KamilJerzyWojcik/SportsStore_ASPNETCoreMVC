using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SportStore.Models
{
	public static class SeedData
	{
		public static void FillDatabaseByData(IApplicationBuilder app)
		{

			ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
			context.Database.Migrate();

			if (!context.Products.Any())
			{
				context.Products.AddRange
					(
						new Product { Name = "Kajak", Description = "łódka do pływania w pojedynkę", Category = "Sporty wodne", Price = 275 },
						new Product { Name = "Kamizelka ratunkowa", Description = "Chroni i dodaje uroku", Category = "Sporty wodne", Price = 58.95M },
						new Product { Name = "Piłka", Description = "Zatwierdzone przez fifa", Category = "Piłka nożna", Price = 19.50m },
						new Product { Name = "Flagi narożne", Description = "Nadadzą twojemu boisku profesjonalizmu", Category = "Piłka nożna", Price = 34.95M },
						new Product { Name = "Satadion", Description = "Składany stadion na 35000 osób", Category = "Piłka nożna", Price = 79500 },
						new Product { Name = "Czapka", Description = "Zwiększ efektywność mózgu", Category = "Szachy", Price = 29.95M },
						new Product { Name = "Ludzka szachownica", Description = "Przyjemna gra dla całej rodziny", Category = "Szachy", Price = 75 },
						new Product { Name = "Błyszczący król", Description = "Figura pokryta złotem i wysadzana diamentami", Category = "Szachy", Price = 1200 },
						new Product { Name = "Szary skoczek", Description = "Figura pokryta węglem i wysadzana granitem", Category = "Szachy", Price = 120 }
						);
				context.SaveChanges();
			}
		}
	}
}
