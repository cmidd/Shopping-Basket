using Shopping.Models.Entities;

namespace Shopping.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Info { get; set; }

        public string AddToBasketUrl { get; set; } = string.Empty;

        public string RemoveFromBasketUrl { get; set; } = string.Empty;
    }
}
