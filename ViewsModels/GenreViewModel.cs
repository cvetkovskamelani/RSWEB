using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models;
using System.Collections.Generic;
namespace BookStore.ViewsModels
{
    public class GenreViewModel
    {
        public IList<Books> Books { get; set; }
        public SelectList Genre { get; set; }
        public string BookGenre { get; set; }
        public IList<Review> Reviews { get; set; }
        public string SearchString { get; set; }
    }
}
