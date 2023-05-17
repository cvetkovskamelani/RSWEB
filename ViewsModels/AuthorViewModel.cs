using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace BookStore.ViewsModels;

public class AuthorViewModel
{
    public IList<Author> Authors { get; set; }
    public string FullNameSearch { get; set; }
    public string NationalitySearch { get; set; }
    public SelectList Nationalities { get; set; }
}
