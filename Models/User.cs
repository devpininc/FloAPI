using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FloAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //Login info
        public string Email { get; set; }
        public string DisplayName { get; set; }

        // Agent-specific fields
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? RecoNumber { get; set; }
        public string? BrokerageId { get; set; }

        // Common
        public string Phone { get; set; }
        public string Role { get; set; } = "Agent";  // Roles: "SysAdmin", "Agent", "Client"

        // For clients only
        public string? AgentId { get; set; }

        // Trial and billing
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime TrialExpiresAt { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsTrial { get; set; } = true;
        public DateTime? LastPaymentDate { get; set; }

        // Magic login
        public string? MagicToken { get; set; }
        public DateTime? MagicTokenExpiresAt { get; set; }
        public bool MagicTokenUsed { get; set; }

    }
}
