using eStore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eStore.Domain.Entities.CartAggregate
{
    public class Cart : BaseEntity, IAggregateRoot
    {
        public string BuyerId { get; private set; }
        private readonly List<CartItem> _items = new List<CartItem>();
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public Cart(string buyerId)
        {
            BuyerId = buyerId;
        }

        public void AddItem(int catalogItemId, decimal unitPrice, int quantity = 1)
        {
            if (!Items.Any(i => i.CatalogItemId == catalogItemId))
            {
                _items.Add(new CartItem(catalogItemId, quantity, unitPrice));
                return;
            }
            var existingItem = Items.FirstOrDefault(i => i.CatalogItemId == catalogItemId);
            existingItem.AddQuantity(quantity);
        }

        public void RemoveEmptyItems()
        {
            _items.RemoveAll(i => i.Quantity == 0);
        }

        public void SetNewBuyerId(string buyerId)
        {
            BuyerId = buyerId;
        }
    }
}
