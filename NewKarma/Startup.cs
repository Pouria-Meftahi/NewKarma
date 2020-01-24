using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewKarma.Areas.Identity.Data;
using NewKarma.Areas.Identity.Services;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Repository;
using NewKarma.Repository.UOW;
using NewKarma.Tools;
using ReflectionIT.Mvc.Paging;

namespace NewKarma
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin/Account/Login";
                //options.AccessDeniedPath = "/Home/AccessDenied";
            });


            services.AddTransient<AppDbContext>();
            services.AddTransient<ConvertDate>();
            services.AddScoped<IEmailSender, EmailSender>();
            //Authentication
            services.AddScoped<IApplicationRoleManager, ApplicationRoleManager>();
            services.AddTransient<ApplicationUserManager>();
            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            //Dynamic Policy
            services.AddSingleton<IAuthorizationHandler, DynamicPermissionsAuthorizationHandler>();
            services.AddSingleton<IMvcActionsDiscoveryService, MvcActionsDiscoveryService>();
            services.AddSingleton<ISecurityTrimmingServices, SecurityTrimmingServices>();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ConstantPolicies.DynamicPermission, policy => policy.Requirements.Add(new DynamicPermissionRequirement()));
            });
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IProductRepo, ProductRepo>();
            services.AddTransient<IConvertDate, ConvertDate>();
            services.AddTransient<ConvertDate>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<ProductRepo>();
            services.AddTransient<AppDbContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //AutoMapper Config
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Helper.Helper());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            //ReflectionIT
            services.AddPaging(options => {
                options.ViewName = "Bootstrap4";
                //options.PageParameterName = "pageindex";
                //options.SortExpressionParameterName = "sort";
                options.HtmlIndicatorDown = " <span>&darr;</span>";
                options.HtmlIndicatorUp = " <span>&uarr;</span>";
            });

        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                 name: "areas",
                 template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
