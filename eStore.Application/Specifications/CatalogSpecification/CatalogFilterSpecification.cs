using eStore.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Specifications.CatalogSpecification
{
    public class CatalogFilterSpecification : BaseSpecification<CatalogItem>
    {
        public CatalogFilterSpecification(int? brandId, int? typeId)
            : base(i => (!brandId.HasValue || i.CatalogBrandId == brandId) &&
                (!typeId.HasValue || i.CatalogTypeId == typeId))
        {
        }
    }
}
