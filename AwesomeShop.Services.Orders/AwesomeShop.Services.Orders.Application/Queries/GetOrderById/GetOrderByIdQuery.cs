using AwesomeShop.Services.Orders.Application.DTOs.ViewModels;
using MediatR;
using System;

namespace AwesomeShop.Services.Orders.Application.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderViewModel>
    {
        public GetOrderByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}