using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using FloApi.Contacts.Models;
using FloApi.Contacts.Data;

namespace FloApi.Contacts.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly MongoDbContext _db;

        public ContactsController(MongoDbContext db)
        {
            _db = db;
        }

        // POST: api/contacts
        [HttpPost]
        public IActionResult AddContact([FromBody] Contact contact)
        {
            contact.Id = Guid.NewGuid().ToString();
            contact.CreatedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;

            _db.Contacts.InsertOne(contact);
            return Ok(contact);
        }

        // GET: api/contacts
        [HttpGet]
        public IActionResult GetAll()
        {
            var contacts = _db.Contacts.Find(_ => true).ToList();
            return Ok(contacts);
        }

        // GET: api/contacts/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var contact = _db.Contacts.Find(c => c.Id == id).FirstOrDefault();
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        // PUT: api/contacts/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateContact(string id, [FromBody] Contact updated)
        {
            var existing = _db.Contacts.Find(c => c.Id == id).FirstOrDefault();
            if (existing == null) return NotFound();

            updated.Id = id;
            updated.CreatedAt = existing.CreatedAt;
            updated.UpdatedAt = DateTime.UtcNow;

            _db.Contacts.ReplaceOne(c => c.Id == id, updated);
            return Ok(updated);
        }
    }
}
