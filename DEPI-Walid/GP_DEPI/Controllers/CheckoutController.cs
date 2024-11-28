using CustomIdentity.Data;
using CustomIdentity.Models;
using CustomIdentity.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentity.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Checkout
        public IActionResult Index()
        {
            var checkouts = _context.Checkouts
                .Include(c => c.Book)
                .Include(c => c.Member)
                .Select(c => new
                {
                    c.CheckoutId,
                    BookTitle = c.Book.Title,
                    MemberName = c.Member.Name,
                    c.CheckoutDate,
                    c.DueDate,
                    c.ReturnDate,
                    PenaltyAmount = c.Penalties.Sum(p => p.Amount)
                })
                .ToList();

            return View(checkouts);
        }

        // GET: Checkout/Create
        public IActionResult Create()
        {
            var availableBooks = _context.Books.Where(b => b.IsAvailable).ToList();
            var members = _context.Members.ToList();

            var model = new CheckoutCreateViewModel
            {
                AvailableBooks = availableBooks,
                Members = members,
                Checkout = new Checkout()
            };

            return View(model);
        }

        // POST: Checkout/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CheckoutCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Set checkout dates
                model.Checkout.CheckoutDate = DateTime.Now;
                model.Checkout.DueDate = model.Checkout.CheckoutDate.AddDays(14); // 14-day checkout

                // Add checkout to the database
                _context.Checkouts.Add(model.Checkout);

                // Update book availability
                var book = await _context.Books.FindAsync(model.Checkout.BookId);
                if (book != null)
                {
                    book.IsAvailable = false; // Mark book as not available
                }

                await _context.SaveChangesAsync(); // Save changes asynchronously

                return RedirectToAction(nameof(Index)); // Redirect to Index action
            }

            // Log validation errors
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                }
            }

            // Repopulate model for the view
            model.AvailableBooks = await _context.Books.Where(b => b.IsAvailable).ToListAsync();
            model.Members = await _context.Members.ToListAsync();

            return View(model); // Return the view with validation errors
        }

        // POST: Checkout/Return/{id}
        [HttpPost]
        public IActionResult Return(int id)
        {
            var checkout = _context.Checkouts
                .Include(c => c.Book)
                .Include(c => c.Penalties) // Include penalties for this checkout
                .FirstOrDefault(c => c.CheckoutId == id);

            if (checkout != null)
            {
                checkout.ReturnDate = DateTime.Now;

                if (checkout.ReturnDate > checkout.DueDate)
                {
                    int overdueDays = (checkout.ReturnDate.Value - checkout.DueDate).Days;
                    var penalty = new Penalty 
                    {
                        Amount = overdueDays * 1.00m, // $1 per day
                        IsPaid = false,
                        ImposedDate = DateTime.Now,
                        CheckoutId = checkout.CheckoutId
                    };

                    _context.Penalties.Add(penalty); // Add the penalty to the database
                }

                checkout.Book.IsAvailable = true; // Mark book as available
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}