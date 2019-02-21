using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace myWebApp
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Add AppDbContext as a service for Products-database.
            services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();
        
            //Add ApplicaitonDbContext as a service for Users-database.
            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();

            //Add IdentiryUser and IdentityRole as a service to use.
            services.AddIdentity<ApplicationDbUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Usage of Interface IEmailSender to send mails.
            services.AddTransient<IEmailSender, EmailSender>(i => 
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"]
                )
            );

            
            
            /*
            //Used of Mssql local database server.
            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            */

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlite(
            //        Configuration.GetConnectionString("ApplicationDbContextConnection")));

            //services.AddDefaultIdentity<ApplicationDbUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            /*
            //for products to save database
            services.AddDbContext<AppDbContext>(option =>
                option.UseInMemoryDatabase("products"));

            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseInMemoryDatabase("Users"));
            */
            
            //Used of local InMemeryDatabae server.
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
