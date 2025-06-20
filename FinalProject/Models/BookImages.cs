namespace FinalProject.Models
{
    public class BookImages:BaseEntity
    {
        public string Image { get; set; }
        public int BookId { get; set; }
        public Book  Book { get; set; }
        public bool IsMain { get; set; }

    }
}
