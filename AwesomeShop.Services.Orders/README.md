# AwesomeShop.Services.Customers - Formação Microsserviços com ASP.NET Core

A arquitetura do AwesomeShop, sistema de e-commerce baseado na arquitetura de microsserviços, contém 6 microsserviços:
- [Customers](https://github.com/EzequielRT/AwesomeShop/tree/main/AwesomeShop.Services.Customers)
- [Products](https://github.com/EzequielRT/AwesomeShop/tree/main/AwesomeShop.Services.Products) 
- [Orders](https://github.com/EzequielRT/AwesomeShop/tree/main/AwesomeShop.Services.Orders)
- [Payments](https://github.com/EzequielRT/AwesomeShop/tree/main/AwesomeShop.Services.Payments)
- [Notifications](https://github.com/EzequielRT/AwesomeShop/tree/main/AwesomeShop.Services.Notifications)
- [API Gateway](https://github.com/EzequielRT/AwesomeShop/tree/main/AwesomeShop.Services.ApiGateway)

## Tecnologias, práticas e arquitetura utilizadas
- ASP.NET Core com .NET 5
- Princípios do Domain-Driven Design
- Clean Architecture
- CQRS
- MongoDB
- Event-Driven Architecture com RabbitMQ
- Service Discovery com Consul
- API Gateway com Ocelot
- Cache Distribuído com Redis

## Funcionalidades do Orders
- Cadastro
- Busca por Id
- Atualização de Status consumindo evento PaymentAccepted
