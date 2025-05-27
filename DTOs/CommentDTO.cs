using System.ComponentModel.DataAnnotations;

namespace BlogAPIWebApp.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int PostId { get; set; }
        [StringLength(250, MinimumLength = 1)]
        public string CommentText { get; set; }
        public DateTime CommentDateTime { get; set; }
    }
}
