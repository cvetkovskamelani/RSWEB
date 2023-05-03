using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models;

namespace BookStore.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? LastName { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        public string? Nationality { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        public string? Gender { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public ICollection<Books>? Books { get; set; }
    }
}
