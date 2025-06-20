using FinalProject.Areas.Admin.ViewModels.Blog;
using FinalProject.Areas.Admin.ViewModels.Book;
using FinalProject.Areas.Admin.ViewModels.Brand;
using FinalProject.Areas.Admin.ViewModels.NewsBlog;
using FinalProject.Areas.Admin.ViewModels.Picture;
using FinalProject.Areas.Admin.ViewModels.SlayderAutor;
using FinalProject.Areas.Admin.ViewModels.Slider;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApDbContext _context;

        private readonly IHttpContextAccessor _contextAccessor;


        public HomeController(ApDbContext dbContext,
            IHttpContextAccessor contextAccessor)
        {
            _context = dbContext;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {

            IEnumerable<BookVM> books = await _context.Books.Select(n => new BookVM
            {
                BookId = n.Id,
                Name = n.Name,
                Price = n.Price,
                Description = n.Description,
                JanrId = n.JanrId,
                LanguageId = n.LanguageId,
                LanguageName=n.Language.Name.ToString(),
                BookImages = n.BookImages != null
            ? n.BookImages.Select(pi => new BookImageVM
            {
                Name = pi.Image,
                IsMain = pi.IsMain
            }).ToList()
            : new List<BookImageVM>(),
                BookDiscounts = n.ProductDiscounts
                 .Select(pd => new BookDiscountVM
                 {
                     Percentage = pd.Discount.Percentage,
                     DiscountId = pd.DiscountId
                 })
                 .ToList()

            }).ToListAsync();

            IEnumerable<BlogVM> blogs = await _context.Blogs.Take(3).Select(n => new BlogVM
            {
                Imeg = n.Image,
                Description = n.Descriotion,
                Id = n.Id,
                Title = n.Title,
            }).ToListAsync();

            IEnumerable<NewsBlogVM> newBlogs = await _context.NewsBlogs.Take(3).Select(n => new NewsBlogVM
            {
                Imeg = n.Image,
                Description = n.Descriotion,
                Id = n.Id,
                Title = n.Title,
            }).ToListAsync();


            List<SliderVM> sliders = await _context.Sliders.Select(n => new SliderVM
            {
                Id = n.Id,
                Image = n.Image
            }).ToListAsync();

          


            List<BrandVM> brands = await _context.Brands.Select(n => new BrandVM
            {
                Id = n.Id,
                Image = n.Image
            }).ToListAsync();

            List<SlayderAutorVM> slayderAutor = await _context.SlayderAutors.Select(n => new SlayderAutorVM
            {
                Id = n.Id,
                Image = n.Image
            }).ToListAsync();

            List<PictureVM> pictures = await _context.Pictures.Select(n => new PictureVM
            {
                Id = n.Id,
                Image = n.Images
            }).ToListAsync();

            HomeVM model = new()
            {
                Blogs = blogs,
                Sliders = sliders,
                Brands = brands,
                Books = books,
                SlayderAutors= slayderAutor,
                Pictures= pictures,
                NewsBlogs=newBlogs
            };


            return View(model);
        }



        [HttpPost]
        public IActionResult AddProductToBasket(int id)
        {

            List<BasketVM> basketDatas = [];

            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }

            var exsistData = basketDatas.FirstOrDefault(m => m.ProductId == id);

            if (exsistData == null)
            {
                basketDatas.Add(new BasketVM { ProductId = id, ProductCount = 1 });
            }
            else
            {
                exsistData.ProductCount++;
            }
            _contextAccessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            int basketCount = basketDatas.Sum(v => v.ProductCount);

            return Ok(basketCount);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<BookVM>());
            }

            var results = await _context.Books
                .Where(b => b.Name.Contains(query) || b.Description.Contains(query))
                .Select(n => new BookVM
                {
                    BookId = n.Id,
                    Name = n.Name,
                    Price = n.Price,
                    Description = n.Description,
                    JanrId = n.JanrId,
                    LanguageId = n.LanguageId,
                    BookImages = n.BookImages != null
                        ? n.BookImages.Select(pi => new BookImageVM
                        {
                            Name = pi.Image,
                            IsMain = pi.IsMain
                        }).ToList()
                        : new List<BookImageVM>(),
                    BookDiscounts = n.ProductDiscounts
                        .Select(pd => new BookDiscountVM
                        {
                            Percentage = pd.Discount.Percentage,
                            DiscountId = pd.DiscountId
                        })
                        .ToList()
                })
                .ToListAsync();

            return View(results);
        }



        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var datas = await _context.Books.Include(p => p.Language).Include(p => p.Janr)
           .Include(p => p.BookImages)
           .Include(p => p.ProductDiscounts)
               .ThenInclude(pd => pd.Discount).ToListAsync();

            var viewModel = datas.Select(p => new BookDetailVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                JanrId = p.JanrId,
                LanguageId = p.LanguageId,
                BookImages = p.BookImages
                     .Select(pi => new FinalProject.ViewModels.BookImageVM { Name = pi.Image, IsMain = pi.IsMain })
                     .ToList(),
                Discounts = p.ProductDiscounts
    .Select(pd => new SelectListItem
    {
        Value = pd.DiscountId.ToString(),
        Text = pd.Discount.Percentage.ToString() + "%"
    }).ToList()


            }).ToList();

            var model = viewModel.FirstOrDefault(h => h.Id == id);

            if (model is null) return RedirectToAction("NotFoundException", "Error");
            return View(model);
        }

    }
}
