using eStore.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Specifications.CatalogSpecification
{
    public class CatalogFilterPaginatedSpecification : BaseSpecification<CatalogItem>
    {
        public CatalogFilterPaginatedSpecification(int skip, int take, int? brandId, int? typeId)
            : base(i => (!brandId.HasValue || i.CatalogBrandId == brandId) &&
                (!typeId.HasValue || i.CatalogTypeId == typeId))
        {
            ApplyPaging(skip, take);
        }
    }
}
