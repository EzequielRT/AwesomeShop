using AwesomeShop.Services.Orders.Application.DTOs.IntegrationsDtos;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using MediatR;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Commands.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Guid>
    {
        private readonly IServiceDiscoveryService _serviceDiscovery;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBusClient _messageBus;
        private readonly HttpClient _httpClient;

        public AddOrderCommandHandler(
            IOrderRepository orderRepository,
            IMessageBusClient messageBus,
            IServiceDiscoveryService serviceDiscovery,
            IHttpClientFactory httpClient)
        {
            _orderRepository = orderRepository;
            _messageBus = messageBus;
            _serviceDiscovery = serviceDiscovery;
            _httpClient = httpClient.CreateClient();
        }

        public async Task<Guid> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = request.ToEntity();

            var customerUrl = await _serviceDiscovery
                .GetServiceUrl("CustomerServices", $"/api/customers/{order.Customer.Id}");

            var customerDto = await _httpClient.GetFromJsonAsync<GetCustomerById>(customerUrl);

            Console.WriteLine(customerDto.FullName);

            await _orderRepository.AddAsync(order, cancellationToken);

            foreach (var @event in order.Events)
            {
                var routingKey = @event.GetType().Name.ToDashCase();
                _messageBus.Publish(@event, routingKey, "order-service");
            }

            return order.Id;
        }
    }
}