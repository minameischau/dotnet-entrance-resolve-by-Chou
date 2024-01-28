using System.ComponentModel.DataAnnotations.Schema;
using TestAVinh.Models;

namespace TestAVinh.DTOs
{
    public class BookDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Topic { get; set; }
        public int? PublishYear { get; set; }
        public decimal? Price { get; set; }
        public int? Rating { get; set; }
        public AuthorDetail Author { get; set;}
    }
}
