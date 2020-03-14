using eStore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Domain.Entities.Catalog
{
    public class CatalogBrand : BaseEntity, IAggregateRoot
    {
        public string Brand { get; private set; }
        public CatalogBrand(string brand)
        {
            Brand = brand;
        }
    }
}
