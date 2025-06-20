namespace FinalProject.Models
{
    public class Book:BaseEntity
    {
        public string Name { get; set; }
        public bool Stock { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string Edition { get; set; }
        public decimal Price { get; set; }
        public int JanrId { get; set; }
        public Janr Janr { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public ICollection<BookAutor> BookAutors { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public ICollection<BookImages> BookImages { get; set; }


    }
}
