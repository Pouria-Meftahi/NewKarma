using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models;
using NewKarma.Repository;
using NewKarma.Repository.UOW;
using NewKarma.Tools;
using ReflectionIT.Mvc.Paging;
using System;

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
                options.LoginPath = "/Account/Login";
                //options.AccessDeniedPath = "/Home/AccessDenied";
            });
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("NewsKarmaDatabase")));

            services.AddTransient<AppDbContext>();
            services.AddTransient<ConvertDate>();
            services.AddScoped<IEmailSender, EmailSender>();


            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            
            //services.AddTransient<AppDbContext>();//What The Fuck!

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IProductRepo, ProductRepo>();
            services.AddTransient<IConvertDate, ConvertDate>();
            services.AddTransient<ConvertDate>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<ProductRepo>();

            services.AddAuthentication();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //AutoMapper Config //Todo:Check This Replace With Di In Controller
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Helper.Helper());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            //ReflectionIT
            services.AddPaging(options =>
            {
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
