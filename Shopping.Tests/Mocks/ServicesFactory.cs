using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shopping.DataAccess;
using Shopping.Factories;
using Shopping.Repositories;
using Shopping.Services;
using Shopping.Settings;

namespace Shopping.Tests.Mocks.Factories
{
    public static class ServicesFactory
    {
        public static DbContextOptions<ShoppingContext> GetShoppingContextOptions()
        {
            var builder = new DbContextOptionsBuilder<ShoppingContext>()
                .UseSqlServer("");

            
            return builder.Options;
        }

        public static ShoppingContext GetShoppingContext()
        {
            return new ShoppingContext(GetShoppingContextOptions());
        }

        public static IBasketRepository GetBasketRepository()
        {
            var context = GetShoppingContext();
            return new BasketRepository(context, GetLogger<BasketRepository>());
        }

        public static IBasketItemsRepository GetBasketItemsRepository()
        {
            var context = GetShoppingContext();
            return new BasketItemsRepository(context, GetLogger<BasketItemsRepository>());
        }

        public static IProductRepository GetProductRepository()
        {
            var context = GetShoppingContext();
            return new ProductRepository(context, GetLogger<ProductRepository>());
        }

        public static IVoucherRepository GetVoucherRepository()
        {
            var context = GetShoppingContext();
            return new VoucherRepository(context, GetLogger<VoucherRepository>());
        }

        public static IHomeViewModelFactory GetHomeViewModelRepository()
        {
            var productRepo = GetProductRepository();
            var basketVMRepo = GetBasketViewModelRepository();
            return new HomeViewModelFactory(productRepo, GetApiOptions(), basketVMRepo);
        }

        public static IBasketViewModelFactory GetBasketViewModelRepository()
        {
            var basketItemsRepo = GetBasketItemsRepository();
            var productRepo = GetProductRepository();
            var voucherRepo = GetVoucherRepository();
            return new BasketViewModelFactory(basketItemsRepo, voucherRepo, productRepo, GetApiOptions());
        }

        public static IProductViewModelFactory GetProductViewModelRepository()
        {
            return new ProductViewModelFactory(GetApiOptions());
        }

        public static IBasketService GetBasketService()
        {
            var basketRepo = GetBasketRepository();
            var basketItemsRepo = GetBasketItemsRepository();
            var productRepo = GetProductRepository();
            var voucherRepo = GetVoucherRepository();
            return new BasketService(basketRepo, GetLogger<BasketService>(), productRepo, basketItemsRepo, voucherRepo);
        }

        public static ILogger<T> GetLogger<T>()
        {
            var factory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());

            return factory.CreateLogger<T>();
        }

        public static IOptions<ApiEndpoints> GetApiOptions()
        {
            var apiEndpoints = new ApiEndpoints()
            {
                GetBasket = "/api/basket/getbasket",
                AddToBasket = "/api/basket/add?basketId={0}&productId={1}",
                RemoveFromBasket = "/api/basket/remove?basketId={0}&productId={1}",
                AddVoucher = "/api/basket/applyVoucher?basketId={0}&voucherId={1}",
                RemoveVoucher = "/api/basket/removeVoucher?basketId={0}"
            };

            return Options.Create(apiEndpoints);
        }
    }
}
