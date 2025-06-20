namespace FinalProject.Areas.Admin.ViewModels.NewsBlog
{
    public class NewsBlogCreateVM
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public IEnumerable<IFormFile> UploadImages { get; set; }

    }
}
