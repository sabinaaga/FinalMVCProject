namespace FinalProject.Models
{
    public class Autor : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<BookAutor> BookAutors { get; set; }

    }
}
