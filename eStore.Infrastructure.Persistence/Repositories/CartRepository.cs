using eStore.Application.Interfaces.Repository;
using eStore.Domain.Entities.CartAggregate;
using eStore.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Infrastructure.Persistence.Repositories
{
    public class CartRepository : AsyncRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
