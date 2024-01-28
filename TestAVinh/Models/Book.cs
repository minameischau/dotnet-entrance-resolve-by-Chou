using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAVinh.Models
{
    //[Table("Book")]
    public class Book 
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Topic { get; set; }
        public int? PublishYear { get; set; }

        [ForeignKey(nameof(Book))]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
        public decimal? Price { get; set; }
        public int? Rating { get; set; }
    }
}
