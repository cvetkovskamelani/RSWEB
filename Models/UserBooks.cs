using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;
namespace BookStore.Models
{
    public class UserBooks
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "App User")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(450, MinimumLength = 3)]
        [Required]
        public string? AppUser { get; set; }

        public Books Books { get; set; }
    }
}
