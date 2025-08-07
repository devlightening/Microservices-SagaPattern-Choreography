using MassTransit;
using Shared;
using StockAPI.Consumers;
using StockAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});


builder.Services.AddSingleton<MongoDBService>();
var app = builder.Build();




app.Run();
