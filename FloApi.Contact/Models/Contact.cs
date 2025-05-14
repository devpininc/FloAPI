using System;

namespace FloApi.Contacts.Models
{
    public class Contact
    {
        public string Id { get; set; }
        // MongoDB ObjectId (string) — unique identifier for the client document

        public string AgentId { get; set; }
        // Reference to the agent who owns this client

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsEmailVerified { get; set; }
        // Flag set when client verifies email (manually or via tracking pixel/open)

        public bool IsPhoneVerified { get; set; }
        // Optional flag for phone verification (e.g., OTP via SMS)

        public string ContactType { get; set; }
        // Replaces "Type" — values like Buyer | Seller | Landlord | Tenant
        // Optionally use an Enum or separate lookup later if standardization needed

        public string Source { get; set; }
        // Source of the lead: Manual | Website | Open House | Import | Ad
        // ✅ Could become a lookup collection ("LeadSources") for reporting consistency

        public string Status { get; set; }
        // Lead stage: Lead | Prospect | Active | Closed | Idle | Hot
        // ✅ Can be static OR mapped to a "LeadStatuses" collection for central control

        public int EngagementScore { get; set; }
        // System-assigned score (0–100) based on behavior:
        // 0-19 = Cold, 20–49 = Idle, 50–79 = Active, 80+ = Hot
        // Calculated from email opens, clicks, visits, time spent, saved listings

        public DateTime LastInteractionAt { get; set; }
        // Updated automatically when client interacts with system (email open, click, etc.)

        public string Preferences { get; set; }
        // Placeholder: JSON of saved filters (price, location, etc.)
        // ✅ Each client can have 0–N SavedSearches (stored separately)
        // ✅ This is for quick last-used info — not a substitute for real search records

        public string[] Tags { get; set; }
        // Tags like ["VIP", "Investor", "Follow-up"]
        // ✅ Tags can be assigned manually or automatically by rules (e.g., “Opened 3 listings in 24h” → “Hot”)

        public string Notes { get; set; }
        // Internal notes by the agent (e.g., “Prefers south-facing units, called 2x last week”)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        // Useful for tracking stale data or auto-reminders
    }
}
