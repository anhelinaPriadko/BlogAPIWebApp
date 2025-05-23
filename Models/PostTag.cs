using Microsoft.Extensions.Hosting;

namespace BlogAPIWebApp.Models
{
    public class PostTag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Post Post { get; set; }
    }
}
