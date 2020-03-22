using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eStore.Application.Features.Cart.Commands;
using eStore.Application.Features.Cart.Queries;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Application.Features.Catalog;
using eStore.Domain.Settings;
using eStore.Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace eStore.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMediator _mediator;
        private string _username = null;
        public IndexModel(SignInManager<ApplicationUser> signInManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _mediator = mediator;
        }
        public UserCartViewModel CartModel { get; set; } = new UserCartViewModel();
        public async Task<IActionResult> OnPost(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToPage("/Index");
            }
            await SetCartModelAsync();

            await _mediator.Send(new AddItemToCartCommand()
            {
                itemModel = productDetails,
                cartId = CartModel.Id
            });

            await SetCartModelAsync();

            return RedirectToPage();
        }
        public async Task OnGet()
        {
            await SetCartModelAsync();
        }

        private async Task SetCartModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var result = await _mediator.Send(new GetUserCartQuery() { userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) });
                if (result.Succeeded)
                {
                    CartModel = result.Data;
                }
                else
                {
                    var data = await _mediator.Send(new CreateCartForUserCommand() { userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) });
                    CartModel = data.Data;
                }
            }
            else
            {
                GetOrSetBasketCookieAndUserName();
                var result = await _mediator.Send(new GetUserCartQuery() { userId = _username });
                if (result.Succeeded)
                {
                    CartModel = result.Data;
                }
                else
                {
                    var data = await _mediator.Send(new CreateCartForUserCommand() { userId = _username });
                    CartModel = data.Data;
                }
            }
        }
        private void GetOrSetBasketCookieAndUserName()
        {
            if (Request.Cookies.ContainsKey(Constants.CART_COOKIENAME))
            {
                _username = Request.Cookies[Constants.CART_COOKIENAME];
            }
            if (_username != null) return;

            _username = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true };
            cookieOptions.Expires = DateTime.Today.AddYears(10);
            Response.Cookies.Append(Constants.CART_COOKIENAME, _username, cookieOptions);
        }
    }
}