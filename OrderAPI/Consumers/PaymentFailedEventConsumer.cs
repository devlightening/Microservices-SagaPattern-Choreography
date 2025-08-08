using MassTransit;
using OrderAPI.Models.Context;
using Shared.Events;

namespace OrderAPI.Consumers
{
    public class PaymentFailedEventConsumer(OrderAPIDbContext _context) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order == null)
                throw new NullReferenceException($"Sipariş bulunamadı. Sipariş ID: {context.Message.OrderId}");
            order.OrderStatu = Models.Enums.OrderStatusType.Failed;
            await _context.SaveChangesAsync();
        }
    }
}
