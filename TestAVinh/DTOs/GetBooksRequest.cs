namespace TestAVinh.DTOs
{
    public class GetBooksRequest
    { 
        public int? AuthorId { get; set; }
        public int? PublishYear { get; set; }
        public int? Rating { get; set; }
    }
}
