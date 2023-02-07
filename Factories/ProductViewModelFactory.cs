using Microsoft.Extensions.Options;
using Shopping.Models.Entities;
using Shopping.Models.ViewModels;
using Shopping.Settings;

namespace Shopping.Factories
{
    public class ProductViewModelFactory : IProductViewModelFactory
    {
        private readonly ApiEndpoints _apiEndpoints;

        public ProductViewModelFactory(IOptions<ApiEndpoints> apiEndpoints)
        {
            _apiEndpoints = apiEndpoints.Value;
        }

        public ProductViewModel Create(Product product)
        {
            return new ProductViewModel()
            {
                Info = product,
            };
        }

        public ProductViewModel Create(Product product, int basketId)
        {
            return new ProductViewModel()
            {
                Info = product,
                AddToBasketUrl = string.Format(_apiEndpoints.AddToBasket, basketId, product.Id),
                RemoveFromBasketUrl = string.Format(_apiEndpoints.RemoveFromBasket, basketId, product.Id),
            };
        }
    }
}
