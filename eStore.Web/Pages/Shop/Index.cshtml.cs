using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStore.Application.Features.Catalog;
using eStore.Application.Features.Common.ViewModels;
using eStore.Application.Features.Shop.Queries;
using eStore.Application.Features.Shop.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eStore.Web.Pages.Shop
{
    
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task OnGet()
        {
            await SetShopModelAsync();
        }
        public ShopViewModel ShopModel { get; set; } = new ShopViewModel();
        private async Task SetShopModelAsync()
        {
            var result = await _mediator.Send(new GetShopModelQuery() { itemsPage = 10, pageIndex = 0});

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