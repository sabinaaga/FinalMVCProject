using FinalProject.Areas.Admin.ViewModels.Autor;
using FinalProject.Areas.Admin.ViewModels.Discount;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AutorController : Controller
    {
       
        private readonly ApDbContext _context;



        public AutorController(ApDbContext dbContext)
        {
            _context = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            var data = await _context.Autors.Select
                (m => new AutorVM {Id=m.Id, Name=m.Name  }).ToListAsync();
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AtorCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var exsistData = await _context.Autors.FirstOrDefaultAsync(m => m.Name == request.Name);
            if (exsistData != null)
            {
                ModelState.AddModelError("Name", "DataExsist");
                return View(request);
            }
            await _context.Autors.AddAsync(new Autor { Name = request.Name });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var info = await _context.Autors.FirstOrDefaultAsync(m => m.Id == id);
            if (info == null) return NotFound();
            _context.Autors.Remove(info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var exsistData = await _context.Autors.FirstOrDefaultAsync(m => m.Id == id);
            if (exsistData == null) return NotFound();

            return View(new AutorEditVM
            {
                Name = exsistData.Name,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AutorEditVM request)
        {

            if (id is null) return BadRequest();
            if (!ModelState.IsValid) return View(request);

            var exsistData = await _context.Autors.FirstOrDefaultAsync(m => m.Id == id);
            if (exsistData == null) return NotFound();
            bool hasDiscount = await _context.Autors.AnyAsync(c => c.Name == request.Name && c.Id != id);
            if (hasDiscount)
            {
                ModelState.AddModelError("Name", "DataExsist");
                return View(request);
            }
            exsistData.Name = request.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
