using MassTransit;
using Shared.Events;

namespace PaymentAPI.Consumers
{
    public class StockReservedEventConsumer(IPublishEndpoint _publishEndpoint) : IConsumer<StockReservedEvent>
    {

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            // Gerçek ödeme işlemini taklit eden bir metot çağıralım
            bool isPaymentSuccessful = await ProcessPayment(context.Message.OrderId, context.Message.TotalPrice);

            if (isPaymentSuccessful) // Artık daha gerçekçi bir koşul var
            {
                // Ödemenin Başarıyla Tamamlanıldığı senaryo
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                await _publishEndpoint.Publish(paymentCompletedEvent);
                Console.WriteLine($"Ödeme Başarılı Bir Şekilde Tamamlandı. Sipariş ID: {context.Message.OrderId}");
            }
            else // Ödemenin başarısız olduğu senaryo
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    // Sipariş ürünlerini de failed event'e eklemek mantıklı olabilir
                    OrderItems = context.Message.OrderItems,
                    Reason = "Ödeme işlemi başarısız oldu. Yetersiz bakiye veya hatalı kart bilgisi."
                };
                await _publishEndpoint.Publish(paymentFailedEvent);
                Console.WriteLine($"Ödeme Başarısız. Sipariş ID: {context.Message.OrderId}");
            }
        }

        // Ödeme işlemini taklit eden bir örnek metot
        private Task<bool> ProcessPayment(Guid orderId, decimal totalPrice)
        {
            // Burada ödeme servisi çağrılabilir, kart bilgileri kontrol edilebilir.
            // Şimdilik rastgele bir sonuç dönelim.
            Random random = new Random();
            return Task.FromResult(random.Next(10) > 2); // %70 ihtimalle başarılı
        }
    }
}
