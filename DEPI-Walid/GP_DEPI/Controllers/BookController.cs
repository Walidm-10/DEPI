using CustomIdentity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CustomIdentity.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        // Use Dependency Injection to get the LibraryDbContext
        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var books = from b in _context.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString)
                                      || b.Author.Contains(searchString)
                                      || b.Genre.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }
    }
}
