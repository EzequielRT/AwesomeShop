using AwesomeShop.Services.Orders.Application.DTOs.ViewModels;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.CacheStorage;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderViewModel>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICacheService _cacheService;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, ICacheService cacheService)
        {
            _orderRepository = orderRepository;
            _cacheService = cacheService;
        }

        public async Task<OrderViewModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = request.Id.ToString();
            var orderViewModel = await _cacheService.GetAsync<OrderViewModel>(cacheKey);

            if (orderViewModel == null)
            {
                var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken); 
                orderViewModel = OrderViewModel.FromEntity(order);
                await _cacheService.SetAsync(cacheKey, orderViewModel);
            }

            return orderViewModel;
        }
    }
}