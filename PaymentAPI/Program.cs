using MassTransit;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<StockReservedEvent>();
    configurator.UsingRabbitMq((context, _configure) =>
    {



        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

var app = builder.Build();




app.Run();
