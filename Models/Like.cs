using Microsoft.Extensions.Hosting;

namespace BlogAPIWebApp.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int PostId { get; set; }

        public virtual BlogUser Reader { get; set; }
        public virtual Post Post { get; set; }
    }
}
