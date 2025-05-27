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
    public class PostsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public PostsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponseDTO>>> GetPosts()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .Select(p => new PostResponseDTO
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.Name,
                    PostText = p.PostText,
                    PostDateTime = p.PostDateTime,
                    Tags = p.PostTags
                        .Select(pt => new TagDTO { Id = pt.Tag.Id, TagText = pt.Tag.TagText })
                        .ToList()
                })
                .ToListAsync();

            return Ok(posts);
        }

        // GET: api/Posts/5
        // У PostsController.cs
        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponseDTO>> GetPost(int id)
        {
            var dto = await _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
                .Select(p => new PostResponseDTO
                {
                    Id = p.Id,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author.Name,
                    PostText = p.PostText,
                    PostDateTime = p.PostDateTime,
                    Tags = p.PostTags
                        .Select(pt => new TagDTO
                        {
                            Id = pt.Tag.Id,
                            TagText = pt.Tag.TagText
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }


        // POST: api/Posts
        // PostsController.cs

        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost([FromBody] PostDTO post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = await _context.BlogUsers.FindAsync(post.AuthorId);
            if (author == null)
                return BadRequest($"Blog user {post.AuthorId} is not found");

            var newPost = new Post
            {
                AuthorId = post.AuthorId,
                PostText = post.PostText,
                PostDateTime = post.PostDateTime
            };

            foreach (var tagId in post.TagIds)
                newPost.PostTags.Add(new PostTag { TagId = tagId });

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);
        }


        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, [FromBody] PostDTO post)
        {
            if (post.Id != id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var old_post = await _context.Posts
                .Include(p => p.PostTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (old_post == null)
                return NotFound();

            var author = await _context.BlogUsers.FindAsync(post.AuthorId);
            if (author == null)
                return BadRequest($"Blog user {post.AuthorId} is not found");

            // оновлюємо властивості
            old_post.PostText = post.PostText;
            old_post.PostDateTime = post.PostDateTime;
            old_post.AuthorId = post.AuthorId;

            // оновлюємо теги: очищаємо старі та додаємо нові
            _context.PostTags.RemoveRange(old_post.PostTags);
            foreach (var tagId in post.TagIds)
            {
                old_post.PostTags.Add(new PostTag { TagId = tagId, PostId = old_post.Id });
            }

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
