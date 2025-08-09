using MassTransit;
using Shared.Events;
using StockAPI.Services;
using MongoDB.Driver;
using StockAPI.Models.Entites;

namespace StockAPI.Consumers
{
    public class PaymentFailedEventConsumer(MongoDBService mongoDBService) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var stocksCollection = mongoDBService.GetCollection<Stock>();

            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await (await stocksCollection.FindAsync(s => s.ProductId == orderItem.ProductId.ToString())).FirstOrDefaultAsync();
                if(stock == null)
                {
                    stock.Count -= orderItem.Count;
                    await stocksCollection.FindOneAndReplaceAsync(s=>s.ProductId == orderItem.ProductId.ToString(), stock);

                }


            }
        }
    }
}