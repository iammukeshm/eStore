using eStore.Application.Features.Cart.Commands;
using eStore.Application.Features.Cart.Queries;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Domain.Settings;
using eStore.Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eStore.Web.Pages.Shared.Components.Cart
{
    public class Cart : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Cart(IMediator mediator,
                        SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var vm = new CartCountViewModel();
            vm.ItemsCount = (await GetBasketViewModelAsync()).Items.Sum(i => i.Quantity);
            return View(vm);
        }

        private async Task<UserCartViewModel> GetBasketViewModelAsync()
        {

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var resultSignedIn = await _mediator.Send(new GetUserCartQuery() { userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) });
                if (resultSignedIn.Succeeded)
                {
                    return resultSignedIn.Data;
                }
                return new UserCartViewModel();
            }
            string anonymousId = GetBasketIdFromCookie();
            if (anonymousId == null) return new UserCartViewModel();
            var result = await _mediator.Send(new GetUserCartQuery() { userId = anonymousId });
            if (result.Succeeded)
            {
                return result.Data;
            }
            return new UserCartViewModel();
        }

        private string GetBasketIdFromCookie()
        {
            if (Request.Cookies.ContainsKey(Constants.CART_COOKIENAME))
            {
                return Request.Cookies[Constants.CART_COOKIENAME];
            }
            return null;
        }
    }
}
