using Shopping.Models.Entities;

namespace Shopping.Models.ViewModels
{
    public class HomeViewModel
    {
        public BasketViewModel? Basket { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; } = Enumerable.Empty<ProductViewModel>();

        public bool HasProducts => Products?.Any() == true;
    }
}
