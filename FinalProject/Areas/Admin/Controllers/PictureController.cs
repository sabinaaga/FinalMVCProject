using FinalProject.Areas.Admin.ViewModels.Picture;
using FinalProject.Areas.Admin.ViewModels.Slider;
using FinalProject.Data;
using FinalProject.Helpers.Extentions;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PictureController : Controller
    {
        
        private readonly ApDbContext _context;
        private readonly IWebHostEnvironment _env;


        public PictureController(ApDbContext dbContext,
             IWebHostEnvironment webHost)
        {
            _context = dbContext;
            _env = webHost;

        }
        public async Task<IActionResult> Index()
        {
            var datas = await _context.Pictures.Select(m => new PictureVM
            { Id = m.Id, Image = m.Images }).ToListAsync();
            return View(datas);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PictureCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            foreach (var item in request.UploadImages)
            {


                if (!item.CheckFileTpe("image/"))
                {
                    {
                        ModelState.AddModelError("UploadImages", "Input type must be only image");
                        return View(request);
                    }

                }

            }
            foreach (var item in request.UploadImages)
            {
                string fileName = Guid.NewGuid() + "-" + item.FileName;

                string filePath = _env.GenerateFilePath("assets/images", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))

                {
                    await item.CopyToAsync(stream);
                }
                await _context.Pictures.AddAsync(new Pictures
                {

                    Images = fileName,

                });

            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Pictures.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();

            string filePath = _env.GenerateFilePath("assets/images", data.Images);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Pictures.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Pictures.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();
            return View(new PictureEditVM
            {
                ExistImage = data.Images,
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PictureEditVM request)
        {
            if (id == null) return BadRequest();

            var data = await _context.Pictures.FirstOrDefaultAsync(m => m.Id == id);
            if (data == null) return NotFound();
            string fileName = request.UploadImage.GenereteFileNmae();

            if (request.UploadImage is not null)
            {
                string filePath = _env.GenerateFilePath("assets/images", data.Images);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }


                string newfilePath = _env.GenerateFilePath("assets/images", fileName);

                using (FileStream stream = new(newfilePath, FileMode.Create))

                {
                    await request.UploadImage.CopyToAsync(stream);
                }
            }
            data.Images = fileName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            var datas = await _context.Pictures.FirstOrDefaultAsync(m => m.Id == id);
            if (datas == null) return NotFound();
            return View(new SliderDetailVM
            {
                Image = datas.Images,
            });
        }

    }
}
