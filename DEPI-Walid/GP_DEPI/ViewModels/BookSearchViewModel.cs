using CustomIdentity.Models;

namespace CustomIdentity.ViewModels
{
    public class BookSearchViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public List<Book> Results { get; set; } // To display search results
    }
}
