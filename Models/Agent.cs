using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FloAPI.Models
{
    public class Agent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // This is their trade name (public-facing name)
        public string TradeName { get; set; }

        // Optional: Legal name, not required unless needed
        public string? LegalName { get; set; }

        public string RecoNumber { get; set; }  // RECO Registration #
        public string Phone { get; set; }       // Official board-linked phone
        public string Email { get; set; }       // Official board-linked email

        public string BrokerageId { get; set; } // FK to Brokerage table

        public string Role { get; set; } = "Agent"; // Future use: Admin, Staff, etc.

        // Trial and billing
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime TrialExpiresAt { get; set; } // 7-day or 14-day trial logic
        public bool IsActive { get; set; } = false; // Becomes true after payment
        public bool IsTrial { get; set; } = true;

        public DateTime? LastPaymentDate { get; set; }
        public string MagicToken { get; set; }
        public DateTime? MagicTokenExpiresAt { get; set; }
        public bool MagicTokenUsed { get; set; }

    }
}
