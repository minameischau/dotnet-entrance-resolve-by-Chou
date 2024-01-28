using Microsoft.EntityFrameworkCore;
using TestAVinh.Configuration;

namespace TestAVinh.Models
{
    public class BookAuthorContext : DbContext
    {
        public BookAuthorContext(DbContextOptions<BookAuthorContext> options) : base(options)
        {
          
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set;}
    }
}
