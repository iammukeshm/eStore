using eStore.Domain.Entities.CartAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Specifications.CartSpecification
{
    public sealed class CartWithItemsSpecification : BaseSpecification<Cart>
    {

        public CartWithItemsSpecification(int cartId, string buyerId)
            : base(b => b.Id == cartId && b.BuyerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
        public CartWithItemsSpecification(string buyerId)
            : base(b => b.BuyerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
    }
}
