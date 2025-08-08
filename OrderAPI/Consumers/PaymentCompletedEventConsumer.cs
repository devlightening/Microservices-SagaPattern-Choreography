using MassTransit;
using OrderAPI.Models.Context;
using Shared.Events;

namespace OrderAPI.Consumers
{
    public class PaymentCompletedEventConsumer(OrderAPIDbContext _context) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order == null)
                throw new NullReferenceException($"Sipariş bulunamadı. Sipariş ID: {context.Message.OrderId}");
            order.OrderStatu = Models.Enums.OrderStatusType.Completed;
            await _context.SaveChangesAsync();
        }
    }
}
