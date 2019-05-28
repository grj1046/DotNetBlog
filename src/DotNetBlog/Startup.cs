using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNetBlog.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using DotNetBlog.Services;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace DotNetBlog
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
            //services.AddDbContext<GuorjAccountDbContext>(options =>
            //{
            //    options.UseMySQL(Configuration.GetConnectionString("GuorjAccountConnection"));
            //});

            //services.AddDbContext<GuorjBlogDbContext>(options =>
            //{
            //    options.UseMySQL(Configuration.GetConnectionString("GuorjBlogConnection"));
            //});
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        //CookieBuilder builder = new CookieBuilder();
            //        //builder.Name = "s";
            //        //builder.Build(context: null);

            //        //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?tabs=aspnetcore2x
            //    });

            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "auth";
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Account/Manage");
                options.Conventions.AuthorizeFolder("/Blog/Manage");

                options.Conventions.AuthorizePage("/Account/Logout");

                options.Conventions.AddPageRoute("/Blog/Post", "Blog/Post/{PostURL?}");
                options.Conventions.AddPageRoute("/Blog/Manage/Edit", "Blog/Manage/Edit/{PostID?}");
                options.Conventions.AddPageRoute("/Blog/GetComments", "Blog/Post/GetComments/{PostID?}/{ContentID?}");
                options.Conventions.AddPageRoute("/Blog/AddComment", "Blog/Post/AddComment/{PostID?}/{ContentID?}");
            }).AddNewtonsoftJson();

            //services.AddMemoryCache(options =>
            //{
            //    options.SizeLimit = 1024 * 1024;
            //});
            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
