using eStore.Domain.Entities.CartAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eStore.Application.Features.Cart.ViewModels
{
    public class UserCartViewModel
    {

        public int Id { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public string BuyerId { get; set; }

        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
    }
}
