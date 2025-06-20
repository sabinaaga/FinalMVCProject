using FinalProject.Areas.Admin.ViewModels.Discount;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {

        private readonly ApDbContext _context;



        public DiscountController(ApDbContext dbContext)
        {
            _context = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            var data = await _context.Discounts.Select
                (m => new DiscountVM { Id = m.Id, Percentage = m.Percentage }).ToListAsync();
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var exsistData = await _context.Discounts.FirstOrDefaultAsync(m => m.Percentage == request.Percentage);
            if (exsistData != null)
            {
                ModelState.AddModelError("Name", "DataExsist");
                return View(request);
            }
            await _context.Discounts.AddAsync(new Discount { Percentage = request.Percentage });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var info = await _context.Discounts.FirstOrDefaultAsync(m => m.Id == id);
            if (info == null) return NotFound();
            _context.Discounts.Remove(info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var exsistData = await _context.Discounts.FirstOrDefaultAsync(m => m.Id == id);
            if (exsistData == null) return NotFound();

            return View(new DiscountEditVM
            {
                Percentage = exsistData.Percentage,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, DiscountEditVM request)
        {

            if (id is null) return BadRequest();
            if (!ModelState.IsValid) return View(request);

            var exsistData = await _context.Discounts.FirstOrDefaultAsync(m => m.Id == id);
            if (exsistData == null) return NotFound();
            bool hasDiscount = await _context.Discounts.AnyAsync(c => c.Percentage == request.Percentage && c.Id != id);
            if (hasDiscount)
            {
                ModelState.AddModelError("Name", "DataExsist");
                return View(request);
            }
            exsistData.Percentage = request.Percentage;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
