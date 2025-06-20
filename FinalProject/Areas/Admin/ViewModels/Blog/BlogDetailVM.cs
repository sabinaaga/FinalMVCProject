namespace FinalProject.Areas.Admin.ViewModels.Blog
{
    public class BlogDetailVM
    {
        public string Image { get; set; }
        public string Description { get; set; }
        public string Tile { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
