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
                var filter = Builders<Stock>.Filter.Eq(s => s.ProductId, orderItem.ProductId);
                var update = Builders<Stock>.Update.Inc(s => s.Count, orderItem.Count);

                await stocksCollection.UpdateOneAsync(filter, update);
            }
        }
    }
}