using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStore.Application.Features.Catalog;
using eStore.Application.Features.Common.ViewModels;
using eStore.Application.Features.Shop.Queries;
using eStore.Application.Features.Shop.ViewModels;
using eStore.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace eStore.Web.Pages.Shop
{
    
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public ShopViewModel ShopModel { get; set; } = new ShopViewModel();
        public async Task OnGet(ShopViewModel shopModel, int? pageId)
        {
            var result = await _mediator.Send(new GetShopModelQuery() 
            { 
                itemsPage = Constants.ITEMS_PER_PAGE, 
                pageIndex = pageId ?? 0 ,
                brandId = shopModel.BrandFilterApplied,
                typeId = shopModel.TypesFilterApplied
            });

            if (result.Succeeded)
            {
                ShopModel = result.Data;
            }
            else
            {
                ShopModel = new ShopViewModel();
            }
        }
        
    }
}