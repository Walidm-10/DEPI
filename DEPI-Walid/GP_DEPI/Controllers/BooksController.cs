using CustomIdentity.Data;
using CustomIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustomIdentity.ViewModels;

namespace CustomIdentity.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        // Use Dependency Injection to get the LibraryDbContext
        public BooksController(AppDbContext context)
        {
            _context = context;
        }



        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the Index action after creation
            }
            return View(book); // Return the view with the model to show validation errors
        }
    }
}
