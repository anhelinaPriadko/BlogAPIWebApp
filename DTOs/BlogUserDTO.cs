using System.ComponentModel.DataAnnotations;

namespace BlogAPIWebApp.DTOs
{
    public class BlogUserDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string UserLogin { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string AvatarPath { get; set; }
        [MaxLength(250)]
        public string AboutYourself { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
    }
}
