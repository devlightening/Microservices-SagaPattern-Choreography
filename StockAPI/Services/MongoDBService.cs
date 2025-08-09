using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace StockAPI.Services
{
    public class MongoDBService
    {
        readonly IMongoDatabase _database;

        public MongoDBService(IConfiguration configuration)
        {
            // Bağlantı dizesi ile MongoClient oluştur
            MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDB"));

            // _database alanını başlat ve appsettings.json'dan veritabanı adını al
            _database = client.GetDatabase(configuration["SagaStockAPIDb"]);
        }

        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            // _database artık başlatıldığı için null olmayacaktır
            return _database.GetCollection<T>(typeof(T).Name);
        }
    }
}