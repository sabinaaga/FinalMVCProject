using FinalProject.Areas.Admin.ViewModels.Book;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class BasketDetailVM
    {
        public Dictionary<BookDetailVM, int> Products { get; set; }

        public decimal Total { get; set; }

    }
}
