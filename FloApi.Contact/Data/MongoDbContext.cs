using FloApi.Contacts.Models;
using MongoDB.Driver;

namespace FloApi.Contacts.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["ConnectionStrings:MongoDB"]);
            _database = client.GetDatabase(configuration["ConnectionStrings:DatabaseName"]);
        }
       
        public IMongoCollection<Contact> Contacts =>
            _database.GetCollection<Contact>("Flo_Contacts");
    }
}
