using eStore.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Infrastructure.Persistence.Context
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();

                if (!context.CatalogBrands.Any())
                {
                    context.CatalogBrands.AddRange(
                        GetPreconfiguredCatalogBrands());

                    await context.SaveChangesAsync();
                }

                if (!context.CatalogTypes.Any())
                {
                    context.CatalogTypes.AddRange(
                        GetPreconfiguredCatalogTypes());

                    await context.SaveChangesAsync();
                }

                if (!context.CatalogItems.Any())
                {
                    context.CatalogItems.AddRange(
                        GetPreconfiguredItems());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    await SeedAsync(context, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>()
            {
                new CatalogBrand("Azure"),
                new CatalogBrand(".NET"),
                new CatalogBrand("Visual Studio"),
                new CatalogBrand("SQL Server"),
                new CatalogBrand("Other")
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new CatalogType("Mug"),
                new CatalogType("T-Shirt"),
                new CatalogType("Sheet"),
                new CatalogType("USB Memory Stick")
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem(2,2, ".NET Bot Black Sweatshirt", ".NET Bot Black Sweatshirt", 19.5M,  "http://catalogbaseurltobereplaced/img/products/1.png"),
                new CatalogItem(1,2, ".NET Black & White Mug", ".NET Black & White Mug", 8.50M, "http://catalogbaseurltobereplaced/img/products/2.png"),
                new CatalogItem(2,5, "Prism White T-Shirt", "Prism White T-Shirt", 12,  "http://catalogbaseurltobereplaced/img/products/3.png"),
                new CatalogItem(2,2, ".NET Foundation Sweatshirt", ".NET Foundation Sweatshirt", 12, "http://catalogbaseurltobereplaced/img/products/4.png"),
                new CatalogItem(3,5, "Roslyn Red Sheet", "Roslyn Red Sheet", 8.5M, "http://catalogbaseurltobereplaced/img/products/5.png"),
                new CatalogItem(2,2, ".NET Blue Sweatshirt", ".NET Blue Sweatshirt", 12, "http://catalogbaseurltobereplaced/img/products/6.png"),
                new CatalogItem(2,5, "Roslyn Red T-Shirt", "Roslyn Red T-Shirt",  12, "http://catalogbaseurltobereplaced/img/products/7.png"),
                new CatalogItem(2,5, "Kudu Purple Sweatshirt", "Kudu Purple Sweatshirt", 8.5M, "http://catalogbaseurltobereplaced/img/products/8.png"),
                new CatalogItem(1,5, "Cup<T> White Mug", "Cup<T> White Mug", 12, "http://catalogbaseurltobereplaced/img/products/9.png"),
                new CatalogItem(3,2, ".NET Foundation Sheet", ".NET Foundation Sheet", 12, "http://catalogbaseurltobereplaced/img/products/10.png"),
                new CatalogItem(3,2, "Cup<T> Sheet", "Cup<T> Sheet", 8.5M, "http://catalogbaseurltobereplaced/img/products/11.png"),
                new CatalogItem(2,5, "Prism White TShirt", "Prism White TShirt", 12, "http://catalogbaseurltobereplaced/img/products/12.png")
            };
        }
    }
}
