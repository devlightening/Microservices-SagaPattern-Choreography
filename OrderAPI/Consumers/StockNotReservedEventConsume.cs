using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.Context;
using OrderAPI.Models.Entites;
using Shared.Events;

namespace OrderAPI.Consumers
{
    public class StockNotReservedEventConsume(OrderAPIDbContext _context) : IConsumer<StockNotReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == context.Message.OrderId);
            if (order == null)
                throw new NullReferenceException();
            order.OrderStatu = Models.Enums.OrderStatusType.Failed;
            await _context.SaveChangesAsync();

        }
    }
}
