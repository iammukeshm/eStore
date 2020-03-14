using eStore.Application.Features.Catalog;
using eStore.Application.Features.Common.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Features.Shop.ViewModels
{
    public class ShopViewModel
    {
        public IEnumerable<CatalogItemViewModel> CatalogItems { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public int? BrandFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
