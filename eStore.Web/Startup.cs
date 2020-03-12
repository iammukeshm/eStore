using eStore.Application;
using eStore.Domain.Settings;
using eStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using NToastNotify;

namespace eStore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public IConfiguration _configuration { get; }

        public IWebHostEnvironment _environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Load From AppSettings
            services.Configure<EmailSettings>(_configuration.GetSection("Email"));
            services.Configure<ClientAppSettings>(_configuration.GetSection("ClientApp"));
            services.Configure<JwtSecurityTokenSettings>(_configuration.GetSection("JwtSecurityToken"));

            services.AddApplication();

            //DI for Infrastructure.Persitence
            services.ConfigureIdentity(_configuration, _environment);
            //services.AddScoped<ICurrentUserService, CurrentUserService>();

            //Hangfire
            ///services.AddHangfireService(Configuration, Environment);
            // Add memory cache services
            services.AddMemoryCache();

            services.AddRouting();
            services.AddMvc().AddNToastNotifyNoty(new NotyOptions
            {
                ProgressBar = true,
                Timeout = 5000,
                Theme = "metroui",
                Layout = "bottomRight"

            });
            services.AddMvc();
            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

              services.AddHealthChecks();

            

            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
            services.AddRazorPages();
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //NOTE this line must be above .UseMvc() line.
            app.UseNToastNotify();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
