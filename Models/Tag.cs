using System.ComponentModel.DataAnnotations;

namespace BlogAPIWebApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string TagText { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }

        public Tag()
        {
            PostTags = new List<PostTag>();
        }
    }
}