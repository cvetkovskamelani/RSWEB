using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;
namespace BookStore.Models
{
    public class BookGenre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int GenreId { get; set; }

        public Books Books { get; set; }
        public Genre Genre { get; set; }
    }
}
