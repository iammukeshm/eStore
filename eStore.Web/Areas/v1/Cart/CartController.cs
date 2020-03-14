using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStore.Application.Features.Cart.Queries;
using eStore.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStore.Web.Areas.Cart
{
    
    [ApiVersion("1.0")]
    public class CartController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetUserCartAsync(int id)
        {
            return Ok(await Mediator.Send(new GetUserCartQuery()));
        }
    }
}