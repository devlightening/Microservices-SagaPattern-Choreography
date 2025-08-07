using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using StockAPI.Services;

namespace StockAPI.Consumers
{
    public class OrderCreatedEventConsumer(MongoDBService _mongoDBService, ISendEndpointProvider sendEndpointProvider ,IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        //readonly MongoDBService _mongoDBService;

        //public OrderCreatedEventConsumer(MongoDBService mongoDBService)
        //{
        //    _mongoDBService = mongoDBService;
        //}

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            IMongoCollection<Models.Entites.Stock> collection = _mongoDBService.GetCollection<Models.Entites.Stock>();
            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await (await collection.FindAsync(s => s.ProductId == item.ProductId && s.Count >= item.Count)).AnyAsync());

            }
            if (stockResult.TrueForAll(s => s.Equals(true)))
            {
                //Stock Güncellemesi..

                foreach (var item in context.Message.OrderItems)
                {
                    //Stok güncelleme işlemi
                    Models.Entites.Stock stock = await (await collection.FindAsync(s => s.ProductId == item.ProductId && s.Count >= item.Count)).FirstOrDefaultAsync();
                    stock.Count -= item.Count;

                    await collection.FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, stock);
                    //Stok güncelleme işlemi tamamlandıktan sonra, stok güncelleme event'i fırlatılacak.

                }
                //Payment'ı uyaracak event'in fırlatılması..
                var sendEndPoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                StockReservedEvent stockReservedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    TotalPrice = context.Message.TotalPrice,
                    OrderItems = context.Message.OrderItems

                };
                await sendEndPoint.Send(stockReservedEvent);
            }
            else
            {
                //Stok yetersizliği nedeniyle sipariş iptal edilecek.
                //Sipariş iptal event'i fırlatılacak. Order'ı uyaracak..
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    Message = "Stok yetersizliği nedeniyle sipariş iptal edildi."
                };
                await publishEndpoint.Publish(stockNotReservedEvent);
            }
        }
    }
}
