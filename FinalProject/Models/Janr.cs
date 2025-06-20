namespace FinalProject.Models
{
    public class Janr:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
