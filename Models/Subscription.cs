namespace BlogAPIWebApp.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int AuthorId { get; set; }

        public virtual BlogUser Reader { get; set; }
        public virtual BlogUser Author { get; set; }
    }
}
