using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewsModels;
namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BooksApiController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: api/BooksApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooks(string searchString, string BookGenre)
        {
            IQueryable<Books> books = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(s => s.Genre.GenreName == BookGenre);
            }
            //books = books.Include(m => m.Director);
            return await books.ToListAsync();
        }

        // GET: api/BooksApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBooks(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            var books = await _context.Books.FindAsync(id);

            if (books == null)
            {
                return NotFound();
            }

            return books;
        }

        // PUT: api/BooksApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooks([FromRoute] int id, [FromBody] GenresEditModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (id != model.Book.Id) { return BadRequest(); }
         

            _context.Entry(model.Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                IEnumerable<int> newGenreList = model.SelectedGenres;
                IEnumerable<int> prevGenreList = _context.BookGenres.Where(s => s.BookId == id).Select(s => s.GenreId);
                IQueryable<BookGenre> toBeRemoved = _context.BookGenres.Where(s => s.BookId == id);
                if (newGenreList != null)
                {
                    toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));
                    foreach (int GenreId in newGenreList)
                    {
                        if (!prevGenreList.Any(s => s == GenreId))
                        {
                            _context.BookGenres.Add(new BookGenre { GenreId = GenreId, BookId = id });
                        }
                    }
                }
                _context.BookGenres.RemoveRange(toBeRemoved);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BooksApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Books>> PostBooks(Books books)
        {
          if (_context.Books == null)
          {
              return Problem("Entity set 'BookStoreContext.Books'  is null.");
          }
            _context.Books.Add(books);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooks", new { id = books.Id }, books);
        }

        // DELETE: api/BooksApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooks(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }

            _context.Books.Remove(books);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BooksExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
