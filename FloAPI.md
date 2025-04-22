📘 FloAPI – Core API & Logic Layer
🔹 Purpose
FloAPI is the central logic and API service powering the ListingFlo ecosystem. It handles all business rules, data access, and third-party integration—serving as the backend brain for agent and client tools.

🔹 Responsibilities
Business Logic Execution
All real estate logic (matching, filtering, notifications, etc.) is handled in FloAPI.

Database Communication
Communicates directly with MongoDB (or MySQL) for storing and retrieving listings, clients, and interactions.

Third-Party Integration

ChatGPT/OpenAI for natural language queries

Constant Contact for email sending

Follow Up Boss for CRM activity logging

API Gateway for Frontend
AgentFlo or other frontends consume clean, structured endpoints exposed by FloAPI.

🔹 Key Features for Clients (via FloAPI)
Plain English Query Handling

Uses ChatGPT to convert unstructured queries into structured + vector-based database queries.

Supports advanced filtering and AI-powered search logic.

Door-Knocking Voice Notes

Field agents can leave voice notes directly via mobile; voice gets transcribed and stored per client.

CMA Feature (Comparative Market Analysis)

Generates property comparisons automatically from database.

Option to send CMA PDF/email to clients.

Automated Listing Emails

Emails matching listings to clients based on preferences.

Uses Constant Contact API for templated designs.

Agent Task Alerts & Notifications

Tracks important dates, follow-ups, or lead activity

Sends reminders to the agent (SMS, email, dashboard ping)

Fallback Communication

If the client doesn’t respond to email (no open/click in X days), system auto-triggers:

SMS

Voicemail (optional future)

Internal note for agent to follow-up manually

🔹 Structure & Naming Best Practices
Group APIs by feature: /Clients, /Listings, /Emails, /CMA, /Tasks

Naming pattern: GetClientPreferences, SendListingEmail, LogDoorKnockNote

Comment with [Tag] keywords in code for easy Ctrl+F lookup

Use Swagger (Swashbuckle) for auto-generated API reference