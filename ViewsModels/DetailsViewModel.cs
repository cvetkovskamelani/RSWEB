using BookStore.Models;
namespace BookStore.ViewsModels
{
    public class DetailsViewModel
    {
        public Books Book;
        public IList<Review> Reviews;
        public string Purchased;
    }
}
