using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FloApi.Models
{
    public class Brokerage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string OfficeCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Optional: linked admin user (SysAdmin or agent with admin role)
        public string AdminUserId { get; set; }

        // Future use
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
