using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStore.Application.Features.Catalog.Queries;
using eStore.Application.Features.Catalog.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eStore.Web.Pages.Catalog
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public CatalogDetailsViewModel CatalogModel { get; set; } = new CatalogDetailsViewModel();
        public async Task OnGet(CatalogDetailsViewModel catalogModel, int? id)
        {
            var result = await _mediator.Send(new GetCatalogDetailsQuery()
            {
                Id = id
            }) ;

            if (result.Succeeded)
            {
                CatalogModel = result.Data;
            }
            else
            {
                CatalogModel = new CatalogDetailsViewModel();
            }
        }
    }
}