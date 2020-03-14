using eStore.Application.Interfaces;
using eStore.Application.Interfaces.Repository;
using eStore.Infrastructure.Persistence.Context;
using eStore.Infrastructure.Persistence.Repositories;
using eStore.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace eStore.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplicationPersistence(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Application"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDateTimeService>(provider => provider.GetService<DateTimeService>());

            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }

    }
}
