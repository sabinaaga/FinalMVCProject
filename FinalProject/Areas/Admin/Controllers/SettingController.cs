using FinalProject.Areas.Admin.ViewModels.Setting;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SettingController : Controller
	{
		private readonly ApDbContext _context;

		public SettingController(ApDbContext dbContext)
		{
			_context = dbContext;
		}

		public async Task<IActionResult> Index()
		{

			var settings = await _context.Settings
	.Select(n => new SettingVM
	{
		Id = n.Id,
		Settings = new Dictionary<string, string> { { n.Key, n.Value } }
	})
	.ToListAsync();

			return View(settings);
		}



		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View(new CreateSettingVM());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateSettingVM model)
		{
			if (ModelState.IsValid)
			{
				var setting = new Settings
				{
					Key = model.Key,
					Value = model.Value
				};
				_context.Add(setting);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(model);
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{

			var setting = await _context.Settings.FindAsync(id);
			if (setting == null)
			{
				return NotFound();
			}

			var model = new EditSettingVM
			{
				Key = setting.Key,
				Value = setting.Value
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EditSettingVM model)
		{
			if (ModelState.IsValid)
			{

				var setting = await _context.Settings.FindAsync(id);
				if (setting == null)
				{
					return NotFound();
				}

				setting.Key = model.Key;
				setting.Value = model.Value;

				_context.Update(setting);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}

			return View(model);
		}

	}
}
