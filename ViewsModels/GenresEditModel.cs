using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models;
using System.Collections.Generic;
using System.Security.Policy;

namespace BookStore.ViewsModels
{
    public class GenresEditModel
    {
        public Books Book { get; set; }
        public IEnumerable<SelectListItem>? GenresList { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        
    }
}
