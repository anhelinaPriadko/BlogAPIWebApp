using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogAPIWebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int PostId { get; set; }
        [StringLength(250, MinimumLength = 1)]
        public string CommentText { get; set; }
        public DateTime CommentDateTime { get; set; }

        public virtual BlogUser Reader { get; set; }
        public virtual Post Post { get; set; }
    }
}
