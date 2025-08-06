using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Context;
using OrderAPI.Models.Entites;
using OrderAPI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

builder.Services.AddDbContext<OrderAPIDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

var app = builder.Build();

// Swagger middleware'leri buraya eklendi
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/create-order", async (CreateOrderViewModel createOrderViewModel, OrderAPIDbContext _context) =>
{
    OrderAPI.Models.Entites.Order order = new()
    {
        BuyerId = Guid.TryParse(createOrderViewModel.BuyerId, out Guid _buyerId) ? _buyerId : Guid.NewGuid(),
        OrderItems = createOrderViewModel.OrderItems.Select(item => new OrderAPI.Models.Entites.OrderItem
        {
            ProductId = Guid.TryParse(item.ProductId, out Guid _productId) ? _productId : Guid.NewGuid(),
            Count = item.Count,
            Price = item.Price
        }).ToList(),
        OrderStatu = OrderAPI.Models.Enums.OrderStatusType.Suspend,
        CreatedDate = DateTime.UtcNow,
        TotalPrice = createOrderViewModel.OrderItems.Sum(item => item.Price * item.Count)
    };
    await _context.AddAsync(order);
    await _context.SaveChangesAsync();
});

app.Run();