using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Genre ")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? GenreName { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
    }
}
