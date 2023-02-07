using Microsoft.Extensions.Options;
using Shopping.Models.Entities;
using Shopping.Models.ViewModels;
using Shopping.Repositories;
using Shopping.Settings;

namespace Shopping.Factories
{
    public class BasketViewModelFactory : IBasketViewModelFactory
    {
        private readonly IBasketItemsRepository _basketItemsRepo;
        private readonly IProductRepository _productRepo;
        private readonly IVoucherRepository _voucherRepo;
        private readonly ApiEndpoints _apiEndpoints;
        private readonly ProductViewModelFactory _productViewModelFactory;

        public BasketViewModelFactory(IBasketItemsRepository basketItemsRepo,
            IVoucherRepository voucherRepo,
            IProductRepository productRepo,
            IOptions<ApiEndpoints> apiEndpoints)
        {
            _basketItemsRepo = basketItemsRepo;
            _voucherRepo = voucherRepo;
            _productRepo = productRepo;
            _apiEndpoints = apiEndpoints.Value;
            _productViewModelFactory = new ProductViewModelFactory(apiEndpoints);
        }

        public BasketViewModel Create(Basket basket)
        {
            var viewModel = new BasketViewModel()
            {
                AddVoucherUrl = _apiEndpoints.AddVoucher,
                RemoveVoucherUrl = _apiEndpoints.RemoveVoucher
            };

            if (basket == null)
                return viewModel;

            viewModel.Id = basket.Id;

            var basketItems = _basketItemsRepo.GetBasketItemsForBasket(basket);

            if (basketItems?.Any() != true)
                return viewModel;

            var productIds = basketItems.Select(b => b.ProductId);

            var products = new List<ProductViewModel>();

            foreach (var basketItem in basketItems)
            {
                var product = _productRepo.GetProduct(basketItem.ProductId);

                if (product == null)
                    continue;

                var productViewModel = _productViewModelFactory.Create(product, basket.Id);

                for (var i = 0; i < basketItem.Quantity; i++) 
                {
                    products.Add(_productViewModelFactory.Create(product, basket.Id));
                }
            }

            viewModel.Products = products;
            viewModel.CurrentTotal = viewModel.OriginalTotal = products?
                .Where(p => p.Info != null)?
                .Select(p => p.Info.Price)
                .Sum() ?? 0;
            
            if (basket.VoucherId.HasValue) 
            {
                var voucher = _voucherRepo.GetVoucher(basket.VoucherId.Value);

                if (voucher != null)
                {
                    viewModel.Voucher = voucher;
                    viewModel.CurrentTotal = viewModel.OriginalTotal - (viewModel.OriginalTotal * (voucher.DiscountPercentage / 100));
                }
            }

            return viewModel;
        }
    }
}
