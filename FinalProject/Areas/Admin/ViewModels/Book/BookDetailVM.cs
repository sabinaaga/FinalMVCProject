using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels.Book
{
    public class BookDetailVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Stock { get; set; }

        public string Description { get; set; }

        public int PageCount { get; set; }

        public string Edition { get; set; }

        public decimal Price { get; set; }

        public int JanrId { get; set; }

        public int LanguageId { get; set; }

        public List<int> SelectedAutorIds { get; set; } = new();

        public List<IFormFile> Images { get; set; } = new();
        public List<BookImageVM> BookImages { get; set; } = new();

        public List<int> SelectedDiscountIds { get; set; } = new();

        public IEnumerable<SelectListItem>? Janrs { get; set; }
        public IEnumerable<SelectListItem>? Languages { get; set; }
        public IEnumerable<SelectListItem>? Autors { get; set; }
        public IEnumerable<SelectListItem>? Discounts { get; set; }
        public string? SelectedMainImageName { get; set; }
    }
}
