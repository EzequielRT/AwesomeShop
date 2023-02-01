using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderRepository(IMongoDatabase mongoDatabase)
        {
            _ordersCollection = mongoDatabase.GetCollection<Order>("orders");
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await _ordersCollection.InsertOneAsync(order, cancellationToken: cancellationToken);
        }

        public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _ordersCollection.Find(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
        {
            await _ordersCollection.ReplaceOneAsync(c => c.Id == order.Id, order, cancellationToken: cancellationToken);
        }
    }
}