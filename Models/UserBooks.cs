using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class UserBooks
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "App User")]
        [StringLength(450, MinimumLength = 3)]
        [Required]
        public string? AppUser { get; set; }

        public Books Books { get; set; }
    }
}
