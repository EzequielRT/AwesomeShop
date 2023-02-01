using System;

namespace AwesomeShop.Services.Orders.Core.Entities
{
    public class OrderItem : IEntityBase
    {
        public OrderItem(Guid produtoId, int quantity, decimal price)
        {
            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            Quantity = quantity;
            Price = price;
        }

        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
    }
}