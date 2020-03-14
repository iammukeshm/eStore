using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Domain.Entities.CartAggregate
{
    public class CartItem : BaseEntity
    {
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public int CatalogItemId { get; private set; }
        public int CartId { get; private set; }

        public CartItem(int catalogItemId, int quantity, decimal unitPrice)
        {
            CatalogItemId = catalogItemId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void SetNewQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
