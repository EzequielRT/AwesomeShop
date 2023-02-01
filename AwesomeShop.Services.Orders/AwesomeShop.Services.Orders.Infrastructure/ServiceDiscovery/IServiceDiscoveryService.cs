using System;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery
{
    public interface IServiceDiscoveryService
    {
        Task<Uri> GetServiceUrl(string serviceName, string requestUrl);
    }
}