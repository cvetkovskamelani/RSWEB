using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;
namespace BookStore.Models
{
    public class Books
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; }

        [Display(Name = "Release Year")]
        public int? ReleaseYear { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Number of pages")]
        public int NumPages { get; set; }

        [StringLength(500, MinimumLength = 3)]
        [Required]
        public string? Description { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? Publisher { get; set; }

        [StringLength(500, MinimumLength = 3)]
        [Required]
        [Display(Name = "Front Page")]
        public string? FrontPage { get; set; }

        [StringLength(500, MinimumLength = 3)]
        [Required]
        [Display(Name = "Download URL")]
        public string? DownloadUrl { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        public ICollection<BookGenre>? BookGenres { get; set; }

        public ICollection<UserBooks>? Users { get; set; }

    }
}
