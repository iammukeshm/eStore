using eStore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Domain.Entities.Catalog
{
    public class CatalogType : BaseEntity, IAggregateRoot
    {
        public string Type { get; private set; }
        public CatalogType(string type)
        {
            Type = type;
        }
    }
}
