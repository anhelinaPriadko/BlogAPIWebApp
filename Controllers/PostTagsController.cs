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
    public class PostTagsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public PostTagsController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/PostTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostTag>>> GetPostTags()
        {
            return await _context.PostTags
                .Include(pt => pt.Tag)
                .Include(pt => pt.Post)
                .ToListAsync();
        }

        // GET: api/PostTags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostTag>> GetPostTag(int id)
        {
            var postTag = await _context.PostTags
                .Include(pt => pt.Tag)
                .Include(pt => pt.Post)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (postTag == null)
            {
                return NotFound();
            }

            return postTag;
        }

        // PUT: api/PostTags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPostTag(int id, PostTag postTag)
        //{
        //    if (id != postTag.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(postTag).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PostTagExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/PostTags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostTag>> PostPostTag(PostTag postTag)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = await _context.Tags.FindAsync(postTag.TagId);
            if (tag == null)
                return BadRequest($"Tag{postTag.TagId} is not found");

            var post = await _context.Posts.FindAsync(postTag.PostId);
            if (post == null)
                return BadRequest($"Post{postTag.PostId} is not found");

            var new_post_tag = new PostTag
            {
                PostId = postTag.PostId,
                TagId = postTag.TagId
            };

            new_post_tag.Tag = tag;
            new_post_tag.Post = post;

            _context.PostTags.Add(postTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostTag", new { id = postTag.Id }, postTag);
        }

        // DELETE: api/PostTags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostTag(int id)
        {
            var postTag = await _context.PostTags.FindAsync(id);
            if (postTag == null)
            {
                return NotFound();
            }

            _context.PostTags.Remove(postTag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostTagExists(int id)
        {
            return _context.PostTags.Any(e => e.Id == id);
        }
    }
}
