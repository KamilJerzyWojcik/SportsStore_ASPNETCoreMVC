using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SportsStore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                Configuration["Data:SportStoreProducts:ConnectionString"]));

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseStaticFiles();
                app.UseSession();
                app.UseMvc(routes =>
                {
                    //trasy są stosowane w kolejności definiowania

                    //przyjazne linki dla kategorii
                    // odpowiada np. /Szachy/Strona2
                    routes.MapRoute(
                        name: null,
                        template: "{category}/Strona{productPage:int}",
                        defaults: new//przyjazny link dla kolejnych stron produktów danej kategorii
                        {
                            controller = "Product",
                            action = "List"
                        }
                    );
                    
                    // dodanie "przyjaznych" linków do kolejnych stron produktów
                    // odpowiada np. /Strona2
                    routes.MapRoute(      
                        name: null,
                        template: "Strona{productPage:int}",//przyjazny link dla wszystkich produktów
                        defaults: new
                        {
                            controller = "Product",
                            action = "List",
                            productPage = 1
                        }
                    );

                    //dodanie przyjaznych linków do strony startowej
                    // odpowiada np. /Szachy
                    routes.MapRoute(
                        name: null,
                        template: "{category}", //przyjazny link dla produktów danej kategorii kategorii
                        defaults: new
                        {
                            controller = "Product",//odwołanie do kontrollera ProductController
                            action = "List", //odwołanie do metody akcji List()
                            productPage = 1  //przekazanie parametru strony (wyświetlanie od 1 strony)
                        }
                    );
                    
                    
                    routes.MapRoute(
                        name: null,
                        template:"",
                        defaults: new {
                            controller = "Product",
                            action="List",
                            productPage = 1
                        }
                     );

                    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");

                });
                SeedData.EnsurePopulated(app);
            }

        }
    }
}
