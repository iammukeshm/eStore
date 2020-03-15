using eStore.Application.Interfaces;
using eStore.Infrastructure.Identity.Context;
using eStore.Infrastructure.Identity.Models;
using eStore.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace eStore.Infrastructure.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            ConfigureCookieSettings(services);

            CreateIdentityIfNotCreated(services);
            // Identity
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Identity"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));



            services.AddScoped<IIdentityContext>(provider => provider.GetService<IdentityContext>());


            //Adds/Injects UserManager Service
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();


            // Services
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IIdentityService, IdentityService>();


            //Keep this always at last. JWT
          //  AuthenticationHelper.ConfigureService(services, configuration["JwtSecurityToken:Issuer"], configuration["JwtSecurityToken:Audience"], configuration["JwtSecurityToken:Key"]);

            return services;
        }
        private static void CreateIdentityIfNotCreated(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var existingUserManager = scope.ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();
                if (existingUserManager == null)
                {
                    services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<IdentityContext>().AddDefaultUI()
                                        .AddDefaultTokenProviders();
                }
            }
        }

        private static void ConfigureCookieSettings(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
            });
        }

    }
}
