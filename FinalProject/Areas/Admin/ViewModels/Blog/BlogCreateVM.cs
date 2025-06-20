namespace FinalProject.Areas.Admin.ViewModels.Blog
{
    public class BlogCreateVM
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public IEnumerable<IFormFile> UploadImages { get; set; }

    }
}
