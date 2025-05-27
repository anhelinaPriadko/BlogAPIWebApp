using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPIWebApp.Models;
using BlogAPIWebApp.DTOs;

namespace BlogAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public SubscriptionsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/Subscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
            return await _context.Subscriptions
                .Include(s => s.Reader)
                .Include(s => s.Author)
                .ToListAsync();
        }

        // GET: api/Subscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var subscription = await _context.Subscriptions
                .Include(s => s.Reader)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscriptionDTO>> PostSubscription(SubscriptionDTO subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reader = await _context.BlogUsers.FindAsync(subscription.ReaderId);
            if (reader == null)
                return BadRequest($"Blog user {subscription.ReaderId} is not find");

            var author = await _context.BlogUsers.FindAsync(subscription.AuthorId);
            if (author == null)
                return BadRequest($"Blog user {subscription.AuthorId} is not found");

            var new_subscription = new Subscription
            {
                ReaderId = subscription.ReaderId,
                AuthorId = subscription.AuthorId
            };

            new_subscription.Author = author;
            new_subscription.Reader = reader;

            _context.Subscriptions.Add(new_subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = new_subscription.Id }, new_subscription);
        }

        // DELETE: api/Subscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}
