using CustomIdentity.Models;

namespace CustomIdentity.ViewModels
{
    public class CheckoutCreateViewModel
    {

            public Checkout Checkout { get; set; }
            public List<Book> AvailableBooks { get; set; }
            public List<Member> Members { get; set; }
        
    }
}
