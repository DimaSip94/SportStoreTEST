using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using React.AspNet;
using SportsStore.Infrastructure;
using SportsStore.Managers;
using SportsStore.Models;
using SportsStore.Repositories;
using StackExchange.Profiling.Storage;

namespace SportsStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Envir { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment _env)
        {
            Configuration = configuration;
            Envir = _env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddMiniProfiler(options => {
                options.RouteBasePath = "/profiler";
                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
                options.ResultsAuthorize = request => !Program.DisableProfilingResults;
            });

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IClaimsTransformation, LocatinClaimsProvider>();
            services.AddSingleton<IQueueRequests, QueueRequests>();

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IEFProductManager>(s => new EFProductManager(connection, Envir.WebRootPath));
            services.AddTransient<IOrderManager>(s => new OrderManager(connection, Envir.WebRootPath));
            services.AddTransient<IFileManager, FileManager>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSignalR();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IMemoryCache cache)
        {                          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseReact(config => { });
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMiniProfiler();
            app.UseSession();
            app.UseAuthentication();
            app.UseSignalR(conf => {
                conf.MapHub<ChatHub>("/chat");
            });
            app.UseMvc(routes=>{

                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { Controller = "Product", action = "List" });

                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
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
                    name:null,
                    template:"{controller}/{action}/{id?}");   
            });
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
