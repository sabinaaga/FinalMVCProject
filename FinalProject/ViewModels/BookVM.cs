using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class BookVM
    {
        public int BookId { get; set; } 
        public string Name { get; set; }
        public bool Stock { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string Edition { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public int JanrId { get; set; }
        public string JanrName { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public List<BookAutorsVM> BookAutors { get; set; }
        public List<BookDiscountVM> BookDiscounts { get; set; }
        public List<BookImageVM> BookImages { get; set; }
    }
}
