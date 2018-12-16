using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportStore.Models;

namespace SportStore
{
	public class Startup
	{
		protected IConfiguration Configuration;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
				Configuration["Data:SportStoreProducts:ConnectionString"]
				));

			services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(
				Configuration["Data:SportstoreIdentity:ConnectionString"]
				));

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<AppIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.AddTransient<IProductRepository, EFProductRepository>();
			services.AddTransient<IOrderRepository, EFOrderRepository>();
			services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddMvc();
			services.AddMemoryCache();
			services.AddSession();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseStatusCodePages();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();
			app.UseSession();
			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute(name: "Error", template: "Error",
					defaults: new { controller = "Error", action = "Error" });

				routes.MapRoute(
					name: null,
					template: "{category}/Strona{productPage:int}",
					defaults: new { Controller = "Product", action = "List" });

				routes.MapRoute(
					name: null,
					template: "Strona{productPage:int}",
					defaults: new { Controller = "Product", action = "List", productPage = 1 });

				routes.MapRoute(
					name: null,
					template: "{category}",
					defaults: new { Controller = "Product", action = "List", productPage = 1 });

				routes.MapRoute(
					name: null,
					template: "",
					defaults: new { Controller = "Product", action = "List", productPage = 1 });

				routes.MapRoute(
					name: null,
					template: "{controller}/{action}/{id?}");
			});
			//SeedData.FillDatabaseByData(app);
			//IdentitySeedData.EnsurePopulated(app);
		}
	}
}
