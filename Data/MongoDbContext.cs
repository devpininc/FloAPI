using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using FloAPI.Models;

namespace FloAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var dbName = config["ConnectionStrings:DatabaseName"];
            _database = client.GetDatabase(dbName);
        }


        public IMongoCollection<User> Users => _database.GetCollection<User>("Flo_Users");

    }
}
