namespace FinalProject.Models
{
    public class Blog:BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Descriotion { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
