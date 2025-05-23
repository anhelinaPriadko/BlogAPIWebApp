using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
namespace BlogAPIWebApp.Models
{
    public class BlogUser
    {
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string UserLogin { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string AvatarPath { get; set; }
        public string AboutYourself { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Subscription> SubscriptionsAsReader { get; set; }
        public virtual ICollection<Subscription> SubscriptionsAsAuthor { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public BlogUser()
        {
            Likes = new List<Like>();
            Posts = new List<Post>();
            SubscriptionsAsReader = new List<Subscription>();
            SubscriptionsAsAuthor = new List<Subscription>();
            Comments = new List<Comment>();
        }
    }
}
