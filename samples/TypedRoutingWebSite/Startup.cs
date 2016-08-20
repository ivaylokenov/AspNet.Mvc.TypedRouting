using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TypedRoutingWebSite.Data;
using TypedRoutingWebSite.Models;
using TypedRoutingWebSite.Services;

namespace TypedRoutingWebSite
{
    using Controllers;
    using Microsoft.AspNetCore.Mvc.Internal;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ApplicationDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddMvc()
                .AddTypedRouting(routes => routes
                    .Get("CustomController/{action}", route => route.ToController<ExpressionsController>())
                    .Get("CustomContact", route => route.ToAction<HomeController>(a => a.Contact()))
                    .Get("WithParameter/{id}", route => route.ToAction<HomeController>(a => a.Index(With.Any<int>())))
                    .Get("Async", route => route.ToAction<AccountController>(a => a.LogOff()))
                    .Get("Named", route => route
                        .ToAction<AccountController>(a => a.Register(With.Any<string>()))
                        .WithName("CustomName"))
                    .Add("Constraint", route => route
                        .ToAction<AccountController>(c => c.Login(With.Any<string>()))
                        .WithActionConstraints(new HttpMethodActionConstraint(new[] { "PUT" })))
                    .Add("MultipleMethods", route => route
                        .ToAction<HomeController>(a => a.About())
                        .ForHttpMethods("GET", "POST")));
            
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes
                    .MapRoute(
                        name: "areaRoute",
                        template: "{area:exists}/{controller=Home}/{action=Index}")
                    .MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
