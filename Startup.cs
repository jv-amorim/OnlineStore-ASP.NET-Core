using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Database;
using OnlineStore.Repositories;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Session;
using OnlineStore.Libraries.Cookie;
using OnlineStore.Libraries.Middlewares;
using OnlineStore.Libraries.Helpers.XmlHelpers;
using OnlineStore.Libraries.Services.Shipping;
using ServiceReference;

namespace OnlineStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<INewsletterRepository, NewsletterRepository>();
            services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            
            services.AddScoped<CalcPrecoPrazoWSSoap>(options => 
                new CalcPrecoPrazoWSSoapClient(CalcPrecoPrazoWSSoapClient.EndpointConfiguration.CalcPrecoPrazoWSSoap));
                
            services.AddScoped<ShippingPackageFactory>();
            services.AddScoped<ShippingRateCalculator>();

            services.AddScoped<CookieManager>();
            services.AddScoped<CartCookieManager>();
            services.AddScoped<ShippingInfoCookieManager>();

            services.AddSession(options =>
            { 
                options.Cookie.IsEssential = true; 
            });
            services.AddScoped<SessionManager>();
            services.AddScoped<CustomerSession>();
            services.AddScoped<CollaboratorSession>();

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            string connectionString = 
                XMLReader.GetDataFromXMLFile("./Private/PrivateData.xml", "Database_Connection_String");
            services.AddDbContext<OnlineStoreContext>(options => options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                /* The default HSTS value is 30 days. You may want to change this for production scenarios, 
                see https://aka.ms/aspnetcore-hsts. */
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseSession();
            app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}