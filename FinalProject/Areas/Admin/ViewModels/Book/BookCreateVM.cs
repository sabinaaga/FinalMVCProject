using System.ComponentModel.DataAnnotations;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Areas.Admin.ViewModels.Book
{
    public class BookCreateVM
    {
        [Required]
        public string Name { get; set; }

        public bool Stock { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page count must be greater than 0")]
        public int PageCount { get; set; }

        public string Edition { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a genre")]
        [Display(Name = "Genre")]
        public int JanrId { get; set; }

        [Required(ErrorMessage = "Please select a language")]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Please select at least one author")]
        public List<int> SelectedAutorIds { get; set; } = new();

        public List<IFormFile>Images { get; set; } = new();
        public List<BookImageVM> BookImages { get; set; } = new();

        public List<int> SelectedDiscountIds { get; set; } = new();

        public IEnumerable<SelectListItem>? Janrs { get; set; }
        public IEnumerable<SelectListItem>? Languages { get; set; }
        public IEnumerable<SelectListItem>? Autors { get; set; }
        public IEnumerable<SelectListItem>? Discounts { get; set; }
        public string? SelectedMainImageName { get; set; }
    }
}
