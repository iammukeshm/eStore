using eStore.Domain.Entities.CartAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Interfaces.Repository
{
    public interface ICartRepository : IAsyncRepository<Cart>
    {
    }
}
