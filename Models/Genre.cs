using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;
namespace BookStore.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Genre ")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? GenreName { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
    }
}
