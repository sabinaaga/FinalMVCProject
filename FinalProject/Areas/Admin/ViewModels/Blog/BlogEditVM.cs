namespace FinalProject.Areas.Admin.ViewModels.Blog
{
    public class BlogEditVM
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string ExistImage { get; set; }
        public IFormFile UploadImage { get; set; }


    }
}
