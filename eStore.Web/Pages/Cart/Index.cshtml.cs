using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStore.Application.Features.Cart.Queries;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eStore.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;
        public IndexModel(SignInManager<ApplicationUser> signInManager,IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
        }
        public UserCartViewModel CartModel { get; set; } = new UserCartViewModel();
        public async Task OnGet()
        {
            await SetCartModelAsync();
        }

        private async Task SetCartModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var result = await _mediator.Send(new GetUserCartQuery());
                if(result.Succeeded)
                {
                    CartModel = result.Data;
                }
                else
                {
                    CartModel = new UserCartViewModel();
                }
            }
            else
            {
                //GetOrSetBasketCookieAndUserName();
                //CartModel = await _basketViewModelService.GetOrCreateBasketForUser(_username);
            }
        }
    }
}