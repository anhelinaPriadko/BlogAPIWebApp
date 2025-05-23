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
    public class PostsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public PostsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var dtos = await _context.Posts
                .Include(p => p.Author)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.Name,
                    PostText = p.PostText,
                    PostDateTime = p.PostDateTime
                })
                .ToListAsync();

            return Ok(dtos);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var dto = await _context.Posts
                .Include(p => p.Author)
                .Where(p => p.Id == id)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.Name,
                    PostText = p.PostText,
                    PostDateTime = p.PostDateTime
                })
                .FirstOrDefaultAsync();

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<PostDto>> PostPost(PostCreateDto input)
        {
            var post = new Post
            {
                AuthorId = input.AuthorId,
                PostText = input.PostText,
                PostDateTime = input.PostDateTime
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var dto = new PostDto
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                AuthorName = (await _context.BlogUsers.FindAsync(post.AuthorId))?.Name,
                PostText = post.PostText,
                PostDateTime = post.PostDateTime
            };

            return CreatedAtAction(nameof(GetPost), new { id = dto.Id }, dto);
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostCreateDto input)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

            post.PostText = input.PostText;
            post.PostDateTime = input.PostDateTime;
            // Не дозволяємо міняти автора через PUT

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
