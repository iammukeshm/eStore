using eStore.Domain.Entities.CartAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Features.Cart.ViewModels
{
    public class UserCartViewModel
    {
        public UserCartViewModel()
        {
            Items = new List<CartItem>();
        }
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public IReadOnlyCollection<CartItem> Items { get; set; }
    }
}
