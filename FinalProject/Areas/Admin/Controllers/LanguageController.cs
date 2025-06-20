using FinalProject.Areas.Admin.ViewModels.Janr;
using FinalProject.Areas.Admin.ViewModels.Language;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LanguageController : Controller
    {
        private readonly ApDbContext _context;


        public LanguageController(ApDbContext dbContext)
        {
            _context = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            var datas = await _context.Languages.Include(n => n.Books).Select(m => new LanguageVM
            { Id = m.Id, Name = m.Name }).ToListAsync();
            return View(datas);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LanguageCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var exsistData = await _context.Languages.FirstOrDefaultAsync(m => m.Name == request.Name);
            if (exsistData != null)
            {
                ModelState.AddModelError("Name", "DataExsist");
                return View(request);
            }

            await _context.Languages.AddAsync(new Language { Name = request.Name });


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Languages.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();



            _context.Languages.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Languages.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();
            return View(new LanguageEditVM { Name = data.Name });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, LanguageEditVM request)
        {
            if (id == null) return BadRequest();

            var data = await _context.Languages.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();

            data.Name = request.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
