namespace BlogAPIWebApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string PostText { get; set; }
        public DateTime PostDateTime { get; set; }

        public virtual BlogUser? Author { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public Post()
        {
            Likes = new List<Like>();
            PostTags = new List<PostTag>();
            Comments = new List<Comment>();
        }
    }
}
