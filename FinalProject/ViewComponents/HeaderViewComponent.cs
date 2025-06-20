using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalProject.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly ApDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;


        public HeaderViewComponent(IHttpContextAccessor contextAccessor,
                          UserManager<AppUser> userManager,
                          ApDbContext dbContext)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _context = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<BasketVM> basketDatas = [];

            if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
            }
            int basketCount = basketDatas.Sum(m => m.ProductCount);


            AppUser user = new();
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }



            Dictionary<string, string> datas = await _context.Settings.
                ToDictionaryAsync(m => m.Key, m => m.Value);

           


            return View(new HeaderVM
            {
                Settings = datas,
                UserFullName = user.FullName,
                BasketProductCount = basketCount
            });
        }

    }
}
