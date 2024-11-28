using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomIdentity.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string? Author { get; set; }
        public bool IsAvailable { get; set; } = true; // Default to true if you want.

        [Required(ErrorMessage = "Category is required.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        public string? Genre { get; set; }

        [Range(1000, 9999, ErrorMessage = "Published Year must be between 1000 and 9999.")]
        public int PublishedYear { get; set; }

        public ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();
    }
}
