namespace FinalProject.Areas.Admin.ViewModels.NewsBlog
{
    public class NewsBlogVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Imeg { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
