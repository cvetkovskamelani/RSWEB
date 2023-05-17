using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.ViewsModels
{
    public class CreateViewModel
    {
        public Books Book { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? PDF { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
