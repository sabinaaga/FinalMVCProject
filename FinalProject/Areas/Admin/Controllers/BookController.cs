using FinalProject.Areas.Admin.ViewModels.Book;
using FinalProject.Data;
using FinalProject.Helpers.Extentions;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class BookController : Controller
    {
        private readonly ApDbContext _context;
        private readonly IWebHostEnvironment _env;


        public BookController(ApDbContext dbContext,
             IWebHostEnvironment webHost)
        {
            _context = dbContext;
            _env = webHost;

        }
        public async Task<IActionResult> Index()
        {
            var datas = await _context.Books.Include(p=>p.Language).Include(p=>p.Janr)
            .Include(p => p.BookImages)
            .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount).ToListAsync();


            var viewModel = datas.Select(p => new BookVM
            {
                BookId = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                JanrId = p.JanrId,
                LanguageId = p.LanguageId,
                BookImages = p.BookImages
                 .Select(pi => new FinalProject.ViewModels.BookImageVM { Name = pi.Image })
                 .ToList(),
                BookDiscounts = p.ProductDiscounts
                 .Select(pd => new BookDiscountVM
                 {
                     Percentage = pd.Discount.Percentage,
                     DiscountId = pd.DiscountId
                 })
                 .ToList()

            }).ToList();

           

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new BookCreateVM
            {
                Janrs = _context.Janrs.Select(j => new SelectListItem { Value = j.Id.ToString(), Text = j.Name }),
                Languages = _context.Languages.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }),
                Autors = _context.Autors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }),
                Discounts = _context.Discounts.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Percentage.ToString() })
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Janrs = _context.Janrs.Select(j => new SelectListItem { Value = j.Id.ToString(), Text = j.Name });
                vm.Languages = _context.Languages.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name });
                vm.Autors = _context.Autors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name });
                vm.Discounts = _context.Discounts.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Percentage.ToString() });

                return View(vm);
            }


            List<BookImages> bookImages = new();
            if (vm.Images != null)
            {
               

                foreach (var item in vm.Images)
                {
                    string fileName = Guid.NewGuid() + "-" + item.FileName;
                    string filePath = _env.GenerateFilePath("assets/images", fileName);

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    bookImages.Add(new BookImages { Image = fileName});
                }

                bookImages.FirstOrDefault().IsMain = true;

            }

            var book = new Book
            {
                Name = vm.Name,
                Stock = vm.Stock,
                Description = vm.Description,
                PageCount = vm.PageCount,
                Edition = vm.Edition,
                Price = vm.Price,
                JanrId = vm.JanrId,
                BookImages= bookImages,
                LanguageId = vm.LanguageId,
                BookAutors = vm.SelectedAutorIds.Select(id => new BookAutor { AutorId = id }).ToList(),
                ProductDiscounts = vm.SelectedDiscountIds?.Select(id => new ProductDiscount { DiscountId = id }).ToList()
            };

           await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();
            var datas = await _context.Books.Include(p => p.Language).Include(p => p.Janr)
           .Include(p => p.BookImages)
           .Include(p => p.ProductDiscounts)
               .ThenInclude(pd => pd.Discount).ToListAsync();


            var data = datas.FirstOrDefault(m => m.Id == id);
            if (data == null) return NotFound();


            IEnumerable<BookImages> images = data.BookImages.ToList();
            foreach (var image in images)
            {
                string filePath = _env.GenerateFilePath("assets/images", image.Image);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

            }
            _context.Books.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAutors)
                .Include(b => b.ProductDiscounts)
                .Include(b => b.BookImages)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            var vm = new BookCreateVM
            {
                Name = book.Name,
                Description = book.Description,
                Price = book.Price,
                Edition = book.Edition,
                PageCount = book.PageCount,
                Stock = book.Stock,
                JanrId = book.JanrId,
                LanguageId = book.LanguageId,
                SelectedAutorIds = book.BookAutors.Select(x => x.AutorId).ToList(),
                SelectedDiscountIds = book.ProductDiscounts.Select(x => x.DiscountId).ToList(),

                BookImages = book.BookImages.Select(img => new BookImageVM
                {
                    Name = img.Image,
                    IsMain = img.IsMain
                }).ToList(),

                Janrs = _context.Janrs.Select(j => new SelectListItem { Value = j.Id.ToString(), Text = j.Name }),
                Languages = _context.Languages.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }),
                Autors = _context.Autors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }),
                Discounts = _context.Discounts.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Percentage.ToString() })
            };

            return View(vm);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Janrs = _context.Janrs.Select(j => new SelectListItem { Value = j.Id.ToString(), Text = j.Name });
                vm.Languages = _context.Languages.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name });
                vm.Autors = _context.Autors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name });
                vm.Discounts = _context.Discounts.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Percentage.ToString() });
                return View(vm);
            }

            var book = await _context.Books
                .Include(b => b.BookAutors)
                .Include(b => b.ProductDiscounts)
                .Include(b => b.BookImages)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            _context.BookAutors.RemoveRange(book.BookAutors);
            _context.ProductDiscounts.RemoveRange(book.ProductDiscounts);
            await _context.SaveChangesAsync();

            book.Name = vm.Name;
            book.Description = vm.Description;
            book.Price = vm.Price;
            book.Edition = vm.Edition;
            book.PageCount = vm.PageCount;
            book.Stock = vm.Stock;
            book.JanrId = vm.JanrId;
            book.LanguageId = vm.LanguageId;

            book.BookAutors = vm.SelectedAutorIds.Select(id => new BookAutor { AutorId = id, Book = book }).ToList();
            book.ProductDiscounts = vm.SelectedDiscountIds?.Select(id => new ProductDiscount { DiscountId = id, Book = book }).ToList();

            if (vm.Images != null && vm.Images.Any())
            {
                foreach (var image in book.BookImages)
                {
                    string filePath = _env.GenerateFilePath("assets/images", image.Image);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.BookImages.RemoveRange(book.BookImages);

                List<BookImages> newImages = new();
                foreach (var img in vm.Images)
                {
                    string fileName = Guid.NewGuid() + "-" + img.FileName;
                    string filePath = _env.GenerateFilePath("assets/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    newImages.Add(new BookImages { Image = fileName });
                }

                newImages.First().IsMain = true;
                book.BookImages = newImages;
            }
            else if (!string.IsNullOrEmpty(vm.SelectedMainImageName))
            {
                foreach (var image in book.BookImages)
                {
                    image.IsMain = image.Image == vm.SelectedMainImageName;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
