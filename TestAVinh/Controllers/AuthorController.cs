using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAVinh.Models;

namespace TestAVinh.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BookAuthorContext _context;
        public AuthorController(BookAuthorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAuthors()
        {
            _context.Database.EnsureCreated();
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors);
        }
    }
}
