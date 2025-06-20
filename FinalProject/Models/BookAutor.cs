namespace FinalProject.Models
{
    public class BookAutor : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int AutorId { get; set; }
        public Autor  Autor { get; set; }

    }
}
