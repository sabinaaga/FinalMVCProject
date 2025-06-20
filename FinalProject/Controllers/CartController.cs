using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Areas.Admin.ViewModels.Book;
using FinalProject.Data;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalProject.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApDbContext _context;


        public CartController(ApDbContext dbContext, 
                                  IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
            _context = dbContext;

        }


        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas = [];

            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }

            Dictionary<BookDetailVM, int> products = new();

            foreach (var item in basketDatas)
            {
                var datas = await _context.Books.Include(p => p.Language).Include(p => p.Janr)
            .Include(p => p.BookImages)
            .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount).ToListAsync();


                var viewModel = datas.Select(p => new BookDetailVM
                {
                    Id=p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    JanrId = p.JanrId,
                    LanguageId = p.LanguageId,
                    BookImages = p.BookImages
                     .Select(pi => new FinalProject.ViewModels.BookImageVM { Name = pi.Image, IsMain=pi.IsMain })
                     .ToList(),
                    Discounts = p.ProductDiscounts
    .Select(pd => new SelectListItem
    {
        Value = pd.DiscountId.ToString(),
        Text = pd.Discount.Percentage.ToString() + "%"
    }).ToList()


                }).ToList();

                var model = viewModel.FirstOrDefault(h => h.Id == item.ProductId);

                products.Add(model, item.ProductCount);
            }

            decimal total = products.Sum(m => m.Key.Price * m.Value);

            return View(new BasketDetailVM { Products = products, Total = total });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            List<BasketVM> basketDatas = [];

            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(
                    _contextAccessor.HttpContext.Request.Cookies["basket"]);
            }

            var existingItem = basketDatas.FirstOrDefault(n => n.ProductId == id);
            if (existingItem != null)
            {
                if (existingItem.ProductCount > 1)
                {
                    existingItem.ProductCount--; 
                }
                else
                {
                    basketDatas.Remove(existingItem); 
                }
            }

            _contextAccessor.HttpContext.Response.Cookies
                .Append("basket", JsonConvert.SerializeObject(basketDatas));

            int count = basketDatas.Sum(v => v.ProductCount);

            Dictionary<BookDetailVM, int> products = new();

            var datas = await _context.Books
                .Include(p => p.Language)
                .Include(p => p.Janr)
                .Include(p => p.BookImages)
                .Include(p => p.ProductDiscounts)
                    .ThenInclude(pd => pd.Discount)
                .ToListAsync();

            var viewModel = datas.Select(p => new BookDetailVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                JanrId = p.JanrId,
                LanguageId = p.LanguageId,
                BookImages = p.BookImages
                    .Select(pi => new BookImageVM { Name = pi.Image, IsMain = pi.IsMain })
                    .ToList(),
                Discounts = p.ProductDiscounts
                    .Select(pd => new SelectListItem
                    {
                        Value = pd.DiscountId.ToString(),
                        Text = pd.Discount.Percentage.ToString() + "%"
                    }).ToList()
            }).ToList();

            foreach (var item in basketDatas)
            {
                var model = viewModel.FirstOrDefault(h => h.Id == item.ProductId);
                if (model != null)
                {
                    products.Add(model, item.ProductCount);
                }
            }

            decimal total = products.Sum(m => m.Key.Price * m.Value);

            return Ok(new { total = total, count = count });
        }


    }
}
