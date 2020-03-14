using eStore.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Interfaces.Repository
{
    interface ICatalogRepository : IAsyncRepository<CatalogItem>
    {
    }
}
