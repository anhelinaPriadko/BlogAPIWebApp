using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPIWebApp.Models;
using BlogAPIWebApp.DTOs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BlogAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogUsersController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public BlogUsersController(BlogAPIContext context)
        {
            _context = context;
        }

        // GET: api/BlogUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogUser>>> GetBlogUsers()
        {
            return await _context.BlogUsers.ToListAsync();
        }

        // GET: api/BlogUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogUser>> GetBlogUser(int id)
        {
            var blogUser = await _context.BlogUsers.FindAsync(id);

            if (blogUser == null)
            {
                return NotFound();
            }

            return blogUser;
        }

        // PUT: api/BlogUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogUser(int id, [FromBody] BlogUserDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BlogUser blogUser = new BlogUser
            {
                Id = dto.Id,
                UserLogin = dto.UserLogin,
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                AvatarPath = dto.AvatarPath,
                AboutYourself = dto.AboutYourself,
                IsActive = dto.IsActive,
                IsOnline = dto.IsOnline
            };
            _context.Entry(blogUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/BlogUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlogUser>> PostBlogUser([FromBody] BlogUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BlogUser blogUser = new BlogUser
            {
                UserLogin = dto.UserLogin,
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                AvatarPath = dto.AvatarPath,
                AboutYourself = dto.AboutYourself,
                IsActive = dto.IsActive,
                IsOnline = dto.IsOnline
            };

            _context.BlogUsers.Add(blogUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogUser", new { id = blogUser.Id }, blogUser);
        }

        // DELETE: api/BlogUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogUser(int id)
        {
            var blogUser = await _context.BlogUsers.FindAsync(id);
            if (blogUser == null)
            {
                return NotFound();
            }

            _context.BlogUsers.Remove(blogUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogUserExists(int id)
        {
            return _context.BlogUsers.Any(e => e.Id == id);
        }
    }
}
