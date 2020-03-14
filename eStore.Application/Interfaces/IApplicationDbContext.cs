using eStore.Domain.Entities.CartAggregate;
using eStore.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eStore.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<CatalogItem> CatalogItems { get; set; }
        DbSet<CatalogBrand> CatalogBrands { get; set; }
        DbSet<CatalogType> CatalogTypes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
