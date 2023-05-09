using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewsModels;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(BookStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString, string BookGenre)
        {
            IQueryable<BookGenre> books = _context.BookGenres.AsQueryable();
            IQueryable<string> genreQuery = _context.Genre.Distinct().Select(m => m.GenreName).Distinct();
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Books.Title.Contains(searchString)); 
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(s => s.Genre.GenreName == BookGenre); 
            } 

            books = books.Include(b => b.Books).ThenInclude(b => b.Author);

            var bookGenreVM = new GenreViewModel
            {
                Genre = new SelectList(await genreQuery.ToListAsync()),
                Books = await books.Select(s => s.Books).Distinct().ToListAsync(),
                Reviews = await _context.Review.ToListAsync()
            };

            return View(bookGenreVM);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(m => m.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }
            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create() { 
            CreateViewModel viewmodel = new CreateViewModel
            {
                Book = new Books(),
                GenreList = new MultiSelectList(_context.Genre.AsEnumerable().OrderBy(s => s.GenreName), "Id", "GenreName"),
                SelectedGenres = Enumerable.Empty<int>()
            };
        
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FullName");
            return View(viewmodel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (viewmodel.Image != null)
                {
                    string FileName = UploadFile(viewmodel);
                    viewmodel.Book.FrontPage = FileName;
                }
                if (viewmodel.PDF != null)
                {
                    string FileName = UploadFile(viewmodel);
                    viewmodel.Book.DownloadUrl = FileName;
                }
                _context.Add(viewmodel.Book);
                await _context.SaveChangesAsync();
                if (viewmodel.SelectedGenres != null)
                {
                    foreach (int GenreId in viewmodel.SelectedGenres)
                    {
                        _context.BookGenres.Add(new BookGenre { BookId = viewmodel.Book.Id, GenreId = GenreId });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FullName", viewmodel.Book.AuthorId);
            return View(viewmodel);
        }
        private string UploadFile(CreateViewModel model)
        {
            string FileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "covers");
                FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
                model.Image = null;
                return FileName;
            }
            else if (model.PDF != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pdf");
                FileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.PDF.FileName);
                string filePath = Path.Combine(uploadsFolder, FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.PDF.CopyTo(fileStream);
                }
                return FileName;
            }
            return null;
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = _context.Books.Where(m => m.Id == id).Include(m => m.BookGenres).First();
            if (book == null) 
            {
                return NotFound();
            }
            var authors = _context.Author.AsEnumerable();
            authors = authors.OrderBy(s => s.FullName);
            GenresEditModel viewmodel = new GenresEditModel
            {
                Book = book,
                GenresList = new MultiSelectList(_context.Genre.AsEnumerable().OrderBy(s => s.GenreName), "Id", "GenreName"),
                SelectedGenres = Enumerable.Empty<int>()
            };
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FullName", book.AuthorId);
            return View(viewmodel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateViewModel viewmodel)
        {
            if (id != viewmodel.Book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewmodel.Image != null)
                    {
                        string FileName = UploadFile(viewmodel);
                        viewmodel.Book.FrontPage = FileName;
                    }
                    if (viewmodel.PDF != null)
                    {
                        string FileName = UploadFile(viewmodel);
                        viewmodel.Book.DownloadUrl = FileName;
                    }
                    _context.Update(viewmodel.Book);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> newGenresList = viewmodel.SelectedGenres;
                    IEnumerable<int> prevGenresList = _context.BookGenres.Where(s => s.BookId == id).Select(s => s.GenreId);
                    IQueryable<BookGenre> toBeRemoved = _context.BookGenres.Where(s => s.BookId == id);
                    if (newGenresList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenresList.Contains(s.GenreId));
                        foreach (int GenreId in newGenresList)
                        {
                            if (!prevGenresList.Any(s => s == GenreId))
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
                    if (!BookExists(viewmodel.Book.Id))
                        // (!(_context.Books?.Any(e => e.Id == viewmodel.Book.Id)).GetValueOrDefault()))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FullName", viewmodel.Book.AuthorId);
            return View(viewmodel);
        }
        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
