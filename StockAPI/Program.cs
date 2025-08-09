using MassTransit;
using MongoDB.Driver;
using Shared;
using StockAPI.Consumers;
using StockAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<PaymentFailedEventConsumer>();
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
    });
});


builder.Services.AddSingleton<MongoDBService>();
var app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var stockCollection = mongoDBService.GetCollection<StockAPI.Models.Entites.Stock>();
if (!stockCollection.FindSync(session => true).Any())
{
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 100, CreatedDate = DateTime.UtcNow });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 200, CreatedDate = DateTime.UtcNow });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 300, CreatedDate = DateTime.UtcNow });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 150, CreatedDate = DateTime.UtcNow });
    await stockCollection.InsertOneAsync(new() { ProductId = Guid.NewGuid().ToString(), Count = 160, CreatedDate = DateTime.UtcNow });
}



app.Run();
