using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datalayer.BusinessLogic;
using Datalayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineClass.Hubs;
namespace OnlineClass
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<GetPayment, Payumoney>();
            services.AddTransient<Ipagination, PaginationRepositroy>();
            services.AddTransient<ITotalCount, TotalCountRepositery>();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSession();
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });



            //Added config end

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<OnlineclassContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
             services.Configure<CookiePolicyOptions>(options =>
            {
                // No consent check needed here
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSignalR();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //       .AddCookie(options =>
            //       {
            //           options.LoginPath = "/Home/Index/";

            //       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Onlineclasses}/{action=Index}/{id?}");
            });
        }

        //// This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.Configure<CookiePolicyOptions>(options =>
        //    {
        //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        //        options.CheckConsentNeeded = context => true;
        //        options.MinimumSameSitePolicy = SameSiteMode.None;
        //    });


        //    services.AddMvc();
        //    services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
        //    //services.AddSession(options =>
        //    //{
        //    //    options.IdleTimeout = TimeSpan.FromMinutes(30);//You can set Time   
        //    //});
        //    services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

        //    // Adds a default in-memory implementation of IDistributedCache.
        //    services.AddDistributedMemoryCache();
        //    //services.AddSession(opts =>
        //    //{
        //    //    opts.Cookie.IsEssential = true; // make the session cookie Essential
        //    //});
        //    services.AddSession();
        //    services.AddDbContext<OnlineclassContext>(options =>
        //        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        //}

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        app.UseExceptionHandler("/Home/Error");
        //        app.UseHsts();
        //    }

        //    app.UseHttpsRedirection();
        //    app.UseStaticFiles();
        //    app.UseCookiePolicy();
        //    app.UseSession();
        //    app.UseMvc(routes =>
        //    {
        //        routes.MapRoute(
        //            name: "default",
        //            template: "{controller=Onlineclasses}/{action=Index}/{id?}");
        //    });
        //}
    }
}
