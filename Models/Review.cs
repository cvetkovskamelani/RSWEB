using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "App User")]
        [StringLength(450, MinimumLength = 3)]
        [Required]
        public string? AppUser { get; set; }

        [StringLength(500, MinimumLength = 3)]
        [Required]
        public string? Comment { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        public Books? Books { get; set; }
    }
}
