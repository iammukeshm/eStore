using eStore.Domain.Entities.CartAggregate;
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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
