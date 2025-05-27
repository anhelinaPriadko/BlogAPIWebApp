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
    public class CommentsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public CommentsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments
                .Include(c => c.Reader)
                .Include(c => c.Post)
                .ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Reader)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> PostComment(CommentDTO comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reader = await _context.BlogUsers.FindAsync(comment.ReaderId);
            if (reader == null)
                return BadRequest($"Blog user {comment.ReaderId} is not found");

            var post = await _context.Posts.FindAsync(comment.PostId);
            if (post == null)
                return BadRequest($"Post {comment.PostId} is not found");

            var new_comment = new Comment
            {
                ReaderId = comment.ReaderId,
                PostId = comment.PostId,
                CommentText = comment.CommentText,
                CommentDateTime = comment.CommentDateTime
            };

            new_comment.Post = post;
            new_comment.Reader = reader;

            _context.Comments.Add(new_comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = new_comment.Id }, new_comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
