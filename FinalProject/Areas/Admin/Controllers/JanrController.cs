using FinalProject.Areas.Admin.ViewModels.Janr;
using FinalProject.Data;
using FinalProject.Helpers.Extentions;
using FinalProject.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JanrController : Controller
    {
        
        private readonly ApDbContext _context;


        public JanrController(ApDbContext dbContext)
        {
            _context = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            var datas = await _context.Janrs.Include(n => n.Books).Select(m => new JanrVM
            { Id = m.Id, Name=m.Name }).ToListAsync();
            return View(datas);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JanrCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var exsistData = await _context.Janrs.FirstOrDefaultAsync(m => m.Name == request.Name);
            if (exsistData != null)
            {
                ModelState.AddModelError("Name", "DataExsist");
                return View(request);
            }

            await _context.Janrs.AddAsync(new Janr { Name = request.Name});


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Janrs.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();

           

            _context.Janrs.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Janrs.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();
            return View(new JanrEditVM {Name = data.Name });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, JanrEditVM request)
        {
            if (id == null) return BadRequest();

            var data = await _context.Janrs.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();
           
            data.Name = request.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }
}
