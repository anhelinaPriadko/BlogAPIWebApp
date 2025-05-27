namespace BlogAPIWebApp.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string PostText { get; set; }
        public DateTime PostDateTime { get; set; }

        public List<int> TagIds { get; set; } = new();
    }

    public class PostResponseDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string PostText { get; set; }
        public DateTime PostDateTime { get; set; }
        public List<TagDTO> Tags { get; set; }
    }
}
