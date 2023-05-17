using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using BookStore.Areas.Identity.Data;
using System.Data;
namespace BookStore.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly UserManager<BookStoreUser> _userManager;
        public ReviewsController(BookStoreContext context, UserManager<BookStoreUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var BookStoreContext = _context.Review.Include(m => m.Books);
            return View(await BookStoreContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(m => m.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            // Who is making the review
            ViewData["User"] = user.Email;
            if (User.IsInRole("Admin"))
            {
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
                return View();
            }

            // id must not be null if the user is logged in and is not the admin
            if (id == null)
            {
                return NotFound("Error");
            }

            // The user has to own the book
            var own = _context.UserBooks.Where(s => s.BookId == id && s.AppUser == user.Email).ToListAsync();
            if (own == null)
            {
                return NotFound("Error");
            }

            // Return a list of only one item
            var book = _context.Books.Where(b => b.Id == id);
            if (book.FirstOrDefaultAsync() == null)
            {
                return NotFound("Error");
            }
            ViewData["BookId"] = new SelectList(book, "Id", "Title");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,BookId,AppUser,Comment,Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", review.BookId);
            return View(Index);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            var book = _context.Review.Include(b => b.Books).Where(b => b.Id == id).Select(b => b.Books).ToList();
            ViewData["BookId"] = new SelectList(book, "Id", "Title", review.BookId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,AppUser,Comment,Rating")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Books");
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", review.BookId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(m => m.Books)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Review == null)
            {
                return Problem("Entity set 'BookStoreContext.Review'  is null.");
            }
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                _context.Review.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Books");
        }

        private bool ReviewExists(int id)
        {
          return (_context.Review?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
