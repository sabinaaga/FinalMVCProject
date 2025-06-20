using FinalProject.Areas.Admin.ViewModels.Blog;
using FinalProject.Areas.Admin.ViewModels.Brand;
using FinalProject.Areas.Admin.ViewModels.NewsBlog;
using FinalProject.Areas.Admin.ViewModels.Picture;
using FinalProject.Areas.Admin.ViewModels.SlayderAutor;
using FinalProject.Areas.Admin.ViewModels.Slider;

namespace FinalProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<BrandVM> Brands { get; set; }
        public IEnumerable<BlogVM> Blogs { get; set; }
        public IEnumerable<NewsBlogVM> NewsBlogs { get; set; }

        public IEnumerable<BookVM> Books { get; set; }
        public IEnumerable<SlayderAutorVM> SlayderAutors { get; set; }
        public IEnumerable<PictureVM> Pictures { get; set; }
    }
}
