using System.ComponentModel.DataAnnotations;
namespace BlogAPIWebApp.DTOs
{
    public class TagDTO
    {
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string TagText { get; set; }
    }
}
