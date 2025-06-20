using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.ViewComponents
{
    public class FooterViewComponent: ViewComponent
    {
        private readonly ApDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;


        public FooterViewComponent(IHttpContextAccessor contextAccessor,
                          UserManager<AppUser> userManager,
                          ApDbContext dbContext)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _context = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            Dictionary<string, string> datas = await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);


            return View(datas);
        }
    }
}
