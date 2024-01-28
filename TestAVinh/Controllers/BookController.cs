using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TestAVinh.Models;
using System.Linq;
using TestAVinh.DTOs;

namespace TestAVinh.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookAuthorContext _context;
        public BookController(BookAuthorContext context) { 
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetBook([FromQuery] int? authorId, [FromQuery] int? publishYear, [FromQuery] int? rating)
        {
            _context.Database.EnsureCreated();

            var book = await _context.Books.ToListAsync();

            if (authorId != null)
            {
                book = await _context.Books.Where(b => b.AuthorId == authorId).ToListAsync();
            }
            if (publishYear != null)
            {
                book = await _context.Books.Where(b => b.PublishYear == publishYear).ToListAsync();
            }

            if (rating != null)
            {
                book = await _context.Books.Where(b => b.Rating == rating).ToListAsync();
            }

            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult> GetBookDetailById(int bookId)
        {
            _context.Database.EnsureCreated();

            // 1. Build with DTO
            //var book = _context.Books.FirstOrDefault(v => v.Id == bookId);
            //var authorOfBook = _context.Authors.FirstOrDefault(v => v.Id == book.AuthorId);
            //var bookDetail = BookToDTO(book);
            //bookDetail.Author = AuthorToDTO(authorOfBook);

            //if (bookDetail == null)
            //{
            //    return NotFound();
            //}

            //return Ok(bookDetail);

            // 2. Build with Select Where method
            int currentYear = DateTime.Now.Year;

            var book = await _context.Books.Where(b => b.Id == bookId)
                                     .Select(b => new BookDetail()
                                         {
                                            Id = b.Id,
                                            Title = b.Title,
                                            Topic = b.Topic,
                                            Author = new AuthorDetail()
                                            {
                                                Id = b.Author.Id,
                                                Name = b.Author.Name,
                                                Female = b.Author.Female,
                                                Born = b.Author.Born,
                                                Died = b.Author.Died,
                                                Age = b.Author.Died == null ? currentYear-b.Author.Born : b.Author.Died-b.Author.Born

                                            },
                                            PublishYear = b.PublishYear,
                                            Price = b.Price,
                                            Rating = b.Rating,
                                         }
                                     ).FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);

        }

        [HttpPost]
        public async Task<ActionResult> CreateBook(BookRequest request)
        {
            _context.Database.EnsureCreated();
            var newBook = new Book()
            {
                Title = request.Title,
                Topic = request.Topic,
                AuthorId = request.AuthorId,
                PublishYear = request.PublishYear,
                Price = request.Price,
                Rating = request.Rating
            };
            var checkBook = await _context.Books.FirstOrDefaultAsync(c => c.Title == request.Title);   
            var checkAuthor = await _context.Authors.FirstOrDefaultAsync(c => c.Id == request.AuthorId);   
            if (checkBook != null)
            {
                return BadRequest("There are already other items in the database with the same name.");
            }
            if (checkAuthor == null)
            {
                return BadRequest("Author is not found.");
            }
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, BookRequest request)
        {
            _context.Database.EnsureCreated();
            var book = await _context.Books.FirstOrDefaultAsync(c => c.Id == id);
            var authorId = book.AuthorId;
            if (book == null )
            {
                return NotFound();
            }
            if (authorId != request.AuthorId)
            {
                return BadRequest("Author is not found.");
            }
            book.Title = request.Title;
            book.AuthorId = request.AuthorId;
            book.PublishYear = request.PublishYear;
            book.Price = request.Price;
            book.Rating = request.Rating;
            book.Topic = request.Topic;
            await _context.SaveChangesAsync();
            return await GetBookDetailById(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _context.Database.EnsureCreated();
            var book = await _context.Books.FindAsync(id);
            if ( book == null )
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        // Prepare for GetBook method 1. Build with DTOs
        private static BookDetail BookToDTO(Book book)
        {
            return new BookDetail
            {
                Id = book.Id,
                Title = book.Title,
                PublishYear = book.PublishYear,
                Price = book.Price,
                Rating = book.Rating,
                Topic = book.Topic
            };

        }
        private static AuthorDetail AuthorToDTO(Author author)
        {
            return new AuthorDetail
            {
                Id = author.Id,
                Female = author.Female,
                Born = author.Born,
                Name = author.Name,
                Died = author.Died,
                Age = author.Died - author.Born
            };
        }
    }
}
