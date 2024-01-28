using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAVinh.Models
{
    [Table("Author")]
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Female { get; set; }
        public int Born { get; set; }
        public int? Died { get; set; }
        public List<Book> Books { get; set; }

    }
}
