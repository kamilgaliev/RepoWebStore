﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.DAL.Context;
using WebStore.Services.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Infrastructure.Middleware;
using WebStore.Services.Services.InCookies;
using WebStore.Interfaces.TestAPI;
using WebStore.Clients.Values;
using WebStore.Clients.Employees;
using WebStore.Clients.Products;
using WebStore.Clients.Orders;
using WebStore.Clients.Identity;
using Microsoft.Extensions.Logging;
using WebStore.Logger;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        //private IConfiguration Configuration { get; }
        //public Startup(IConfiguration Configuration)
        //{
        //    this.Configuration = Configuration;
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default"))
            //.UseLazyLoadingProxies()
            //);

            //services.AddTransient<WebStoreDbInitializer>();



            //services.AddTransient<IProductData, InMemoryProductData>();

            services.AddIdentity<User, Role>()
                //.AddEntityFrameworkStores<WebStoreDB>()
                .AddIdentityWebStoreWebAPIClients() //через builder
                .AddDefaultTokenProviders();

            //services.AddIdentityWebStoreWebAPIClients();

            //#region Identity stores custom implementation

            //services.AddTransient<IUserStore<User>, UsersClient>();
            //services.AddTransient<IUserRoleStore<User>, UsersClient>();
            //services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            //services.AddTransient<IUserEmailStore<User>, UsersClient>();
            //services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            //services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            //services.AddTransient<IUserClaimStore<User>, UsersClient>();
            //services.AddTransient<IUserLoginStore<User>, UsersClient>();

            //services.AddTransient<IRoleStore<Role>, RolesClient>();

            //#endregion

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();

            services.AddTransient<IEmployeesData, EmployeesClient>();

            //services.AddTransient<IProductData, SqlProductData>();
            services.AddTransient<IProductData, ProductsClient>();

            services.AddTransient<ICartService, InCookiesCartService>();

            //services.AddTransient<IOrderService, SqlOrderService>();
            services.AddTransient<IOrderService, OrdersClient>();

            services.AddTransient<IValuesService, ValuesClient>();

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, WebStoreDbInitializer db*/,ILoggerFactory log)
        {
            //db.Initialize();

            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWelcomePage("/welcome");

            app.UseMiddleware<TestMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                // http://localhost:5000 -> controller = "Home" action = "Index" параметр = null
            });
        }
    }
}
