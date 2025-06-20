namespace FinalProject.Areas.Admin.ViewModels.NewsBlog
{
    public class NewsBlogEditVM
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string ExistImage { get; set; }
        public IFormFile UploadImage { get; set; }

    }
}
