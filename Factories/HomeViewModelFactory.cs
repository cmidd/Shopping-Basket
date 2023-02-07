using Microsoft.Extensions.Options;
using Shopping.Models.Entities;
using Shopping.Models.ViewModels;
using Shopping.Repositories;
using Shopping.Settings;

namespace Shopping.Factories
{
    public class HomeViewModelFactory : IHomeViewModelFactory
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductViewModelFactory _productViewModelFactory;
        private readonly IBasketViewModelFactory _basketViewModelFactory;

        public HomeViewModelFactory(IProductRepository productRepo,
            IOptions<ApiEndpoints> apiEndpoints,
            IBasketViewModelFactory basketViewModelFactory)
        {
            _productRepo = productRepo;
            _productViewModelFactory = new ProductViewModelFactory(apiEndpoints);
            _basketViewModelFactory = basketViewModelFactory;
        }

        public HomeViewModel Create(Basket basket)
        {
            var viewModel = new HomeViewModel
            {
                Basket = _basketViewModelFactory.Create(basket)
            };

            if (viewModel.Basket.Id == default)
            {
                viewModel.Products = _productRepo.GetAll()?
                    .Select(x => _productViewModelFactory.Create(x))
                    ?? Enumerable.Empty<ProductViewModel>();
            }
            else
            {
                viewModel.Products = _productRepo.GetAll()?
                    .Select(x => _productViewModelFactory.Create(x, viewModel.Basket.Id))
                    ?? Enumerable.Empty<ProductViewModel>();
            }

            return viewModel;
        }
    }
}
