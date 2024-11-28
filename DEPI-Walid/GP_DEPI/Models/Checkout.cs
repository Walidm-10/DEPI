using System.ComponentModel.DataAnnotations;

namespace CustomIdentity.Models
{
    public class Checkout
    {
        public int CheckoutId { get; set; }

        public DateTime CheckoutDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Book is required.")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required(ErrorMessage = "Member is required.")]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public ICollection<Penalty> Penalties { get; set; }
    }
}