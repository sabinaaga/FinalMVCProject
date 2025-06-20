using FinalProject.Areas.Admin.ViewModels.Blog;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApDbContext _context;

        public BlogController(ApDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.Select(n => new BlogVM
            {
                Id = n.Id,
                Imeg = n.Image,
                Title = n.Title,
                Description = n.Descriotion,
                Date = n.Date
            }).ToListAsync());
        }


        public async Task<IActionResult> Detail(int id)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            return View(new BlogDetailVM
            {
               
                Image = blog.Image,
                Tile = blog.Title,
                Description = blog.Descriotion,
                Date = blog.Date
            });
        }
    }
}
