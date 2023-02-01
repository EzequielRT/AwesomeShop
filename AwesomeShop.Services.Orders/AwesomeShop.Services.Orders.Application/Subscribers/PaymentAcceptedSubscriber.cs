using AwesomeShop.Services.Orders.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Subscribers
{
    public class PaymentAcceptedSubscriber : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private const string QUEUE = "order-service/payment-accepted";
        private const string EXCHANGE = "order-service";
        private const string ROUTING_KEY = "payment-accepted";

        public PaymentAcceptedSubscriber(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = connectionFactory.CreateConnection("order-service-payment-accepted-subscriber");

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(EXCHANGE, "topic", true);
            _channel.QueueDeclare(QUEUE, true, false, false, null);
            _channel.QueueBind(QUEUE, "payment-service", ROUTING_KEY);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (send, eventArgs) =>
            {
                var byteArray = eventArgs.Body.ToArray();

                var contentString = Encoding.UTF8.GetString(byteArray);
                var message = JsonConvert.DeserializeObject<PaymentAccepted>(contentString);

                Console.WriteLine($"Message PaymentAccepted received with Id {message.Id}");

                var result = await UpdateOrder(message);

                if (result)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE, false, consumer);

            return Task.CompletedTask;
        }

        private async Task<bool> UpdateOrder(PaymentAccepted paymentAccepted)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();

                var order = await orderRepository.GetByIdAsync(paymentAccepted.Id);

                order.SetAsCompleted();

                await orderRepository.UpdateAsync(order);

                return true;
            }
        }

        public class PaymentAccepted
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
        }
    }
}