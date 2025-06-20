using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProject.Controllers
{
    public class BookController : Controller
    {
        private readonly ApDbContext _context;

        public BookController(ApDbContext apDbContext)
        {
            _context = apDbContext;
        }
        public async Task<IActionResult> Index(int? id)
        {
            var data = await _context.Books.Include(p => p.Language).Include(p => p.Janr)
            .Include(p => p.BookImages)
            .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount).FirstOrDefaultAsync(n => n.Id == id);
            if (data == null) return NotFound();


            return View(new BookVM
            {
                BookId = data.Id,
                Name = data.Name,
                Price = data.Price,
                Description = data.Description,
                JanrId = data.JanrId,
                LanguageId = data.LanguageId,
                BookImages = data.BookImages
                 .Select(pi => new FinalProject.ViewModels.BookImageVM { Name = pi.Image })
                 .ToList(),
                BookDiscounts = data.ProductDiscounts
                 .Select(pd => new BookDiscountVM
                 {
                     Percentage = pd.Discount.Percentage,
                     DiscountId = pd.DiscountId
                 })
                 .ToList()

            });

           
        }
    }
}

