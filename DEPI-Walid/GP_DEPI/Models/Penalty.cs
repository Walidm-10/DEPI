namespace CustomIdentity.Models
{
    public class Penalty
    {
        public int PenaltyId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime ImposedDate { get; set; }

        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }
    }

}
