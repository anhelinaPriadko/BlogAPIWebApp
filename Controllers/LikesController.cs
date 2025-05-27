using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPIWebApp.Models;

namespace BlogAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public LikesController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/Likes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikes()
        {
            return await _context.Likes
                .Include(l => l.Reader)
                .Include(l => l.Post)
                .ToListAsync();
        }

        // GET: api/Likes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetLike(int id)
        {
            var like = await _context.Likes
                .Include(l => l.Reader)
                .Include(l => l.Post)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (like == null)
            {
                return NotFound();
            }

            return like;
        }

        // PUT: api/Likes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLike(int id, Like like)
        {
            if (id != like.Id)
            {
                return BadRequest();
            }

            _context.Entry(like).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(id))
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

        // POST: api/Likes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Like>> PostLike(Like like)
        {
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            var reader = await _context.BlogUsers.FindAsync(like.ReaderId);
            if (reader == null)
                return BadRequest($"Blog user {like.ReaderId} is not found");

            var post = await _context.Posts.FindAsync(like.PostId);
            if (post == null)
                return BadRequest($"Posr {like.PostId} is not found");

            var new_like = new Like
            {
                ReaderId = like.ReaderId,
                PostId = like.PostId
            };

            new_like.Reader = reader;
            new_like.Post = post;

            _context.Likes.Add(new_like);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = new_like.Id }, new_like);
        }

        // DELETE: api/Likes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LikeExists(int id)
        {
            return _context.Likes.Any(e => e.Id == id);
        }
    }
}
