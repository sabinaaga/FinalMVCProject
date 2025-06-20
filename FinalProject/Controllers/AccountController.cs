using FinalProject.Helpers.Enums;
using FinalProject.Models;
using FinalProject.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        private readonly SignInManager<AppUser> signInManager;

        //private readonly IEmailServices _emailService;

        public AccountController(UserManager<AppUser> user,
                                        SignInManager<AppUser> signIn)
        {
            userManager = user;
            signInManager = signIn;
            //_emailService = email;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            AppUser user = new AppUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(request);
            }

            await userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            //string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            //string url = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme, Request.Host.ToString());



            //string html = $"<a href='{url}'>Click here</a>";

            //_emailService.Send(user.Email, "Email confirmation for account", html);

            return RedirectToAction(nameof(LogIn));
        }

        //public async Task<IActionResult> ConfirmEmail(string userId, string token)
        //{
        //    AppUser exsistUser = await userManager.FindByIdAsync(userId);
        //    await userManager.ConfirmEmailAsync(exsistUser, token);
        //    return RedirectToAction(nameof(VerifyEmail));
        //}



        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }


        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM request)
        {

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var exsistUser = await userManager.FindByEmailAsync(request.EmailOrUserName);

            if (exsistUser == null)
            {
                exsistUser = await userManager.FindByNameAsync(request.EmailOrUserName);
            }

            if (exsistUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong");
                return View(request);
            }

            var result = await userManager.CheckPasswordAsync(exsistUser, request.Password);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Email or password is wrong");
                return View(request);
            }

            await signInManager.SignInAsync(exsistUser, false);

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

