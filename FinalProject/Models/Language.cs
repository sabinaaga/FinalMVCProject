namespace FinalProject.Models
{
    public class Language:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
