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
using eStore.Infrastructure.Persistence;
using eStore.Web.Services;
using eStore.Application.Interfaces;

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
            services.Configure<CatalogSettings>(_configuration.GetSection("CatalogSettings"));
            services.AddSingleton<IUriComposer>(new UriComposer(_configuration.Get<CatalogSettings>()));
            services.AddApplication();

            //DI for Infrastructure.Persitence
            services.ConfigureIdentity(_configuration, _environment);
            services.ConfigureApplicationPersistence(_configuration, _environment);


            services.AddScoped<ICurrentUserService, CurrentUserService>();

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


            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\eStore.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "eStore WebApi",
                    Description = "ASP.NET Core 3.1 Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Mukesh Murugan",
                        Email = "iammukeshm@gmail.com",
                        Url = new Uri("https://mukeshmurugan.com/contact"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });

            });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PointOfSales WebApi V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("api_health_check");
            });
        }
    }
}
