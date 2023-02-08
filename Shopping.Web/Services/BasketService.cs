using Shopping.Models.Entities;
using Shopping.Repositories;

namespace Shopping.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IProductRepository _productRepo;
        private readonly IBasketItemsRepository _basketItemsRepo;
        private readonly IVoucherRepository _voucherRepo;
        private readonly ILogger<BasketService> _logger;

        public BasketService(IBasketRepository basketRepo,
            ILogger<BasketService> logger,
            IProductRepository productRepo,
            IBasketItemsRepository basketItemsRepo,
            IVoucherRepository voucherRepo)
        {
            _basketRepo = basketRepo;
            _logger = logger;
            _productRepo = productRepo;
            _basketItemsRepo = basketItemsRepo;
            _voucherRepo = voucherRepo;
        }

        public Basket? CreateNewBasket()
        {
            var basket = _basketRepo.SaveBasket(new Basket());

            return basket;
        }

        public Basket? GetCurrentBasket(HttpRequest currentRequest)
        {
            var basketIdStr = currentRequest?.Cookies[Constants.BasketCookieName];

            if (string.IsNullOrWhiteSpace(basketIdStr)) 
            {
                _logger.LogInformation("No basket id found for current HttpRequest");
                return null;
            }

            if (!int.TryParse(basketIdStr, out var basketId))
            {
                _logger.LogWarning($"Invalid basket id {basketIdStr} in current HttpRequest");
                return null;
            }

            var basket = _basketRepo.GetBasket(basketId);

            if (basket == null) 
            {
                _logger.LogInformation($"No basket found with id {basketId}");
            }

            return basket;
        }

        public Product? AddProductToBasket(int basketId, int productId, int quantity = 1)
        {
            try
            {
                var basket = _basketRepo.GetBasket(basketId);

                if (basket == null)
                {
                    _logger.LogInformation($"Could not add product to basket: Basket with id {basketId} does not exist");
                    return null;
                }

                var product = _productRepo.GetProduct(productId);

                if (product == null)
                {
                    _logger.LogInformation($"Could not add product to basket: Product with id {productId} does not exist");
                    return null;
                }

                var basketItems = _basketItemsRepo.GetBasketItemsForBasket(basket);

                if (basketItems?.Any() != true)
                {
                    _basketItemsRepo.SaveBasketItem(new BasketItem()
                    {
                        BasketId = basketId,
                        ProductId = productId,
                        Quantity = quantity
                    });
                }
                else
                {
                    var basketItem = basketItems.FirstOrDefault(x => x.BasketId == basketId && x.ProductId == productId);

                    if (basketItem == null)
                    {
                        _basketItemsRepo.SaveBasketItem(new BasketItem()
                        {
                            BasketId = basketId,
                            ProductId = productId,
                            Quantity = quantity
                        });
                    }
                    else
                    {
                        basketItem.Quantity += quantity;
                        _basketItemsRepo.SaveBasketItem(basketItem);
                    }
                }

                _logger.LogInformation($"Successfully added {quantity} x productId {productId} to basketId {basketId}");
                return product;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred when adding {quantity} x product {productId} to basket {basketId}");
                return null;
            }
        }

        public bool RemoveProductFromBasket(int basketId, int productId, int quantity = 1)
        {
            try
            {
                var basket = _basketRepo.GetBasket(basketId);

                if (basket == null)
                {
                    _logger.LogInformation($"Could not remove product from basket: Basket with id {basketId} does not exist");
                    return false;
                }

                if (!_productRepo.ProductExists(productId))
                {
                    _logger.LogInformation($"Could not remove product from basket: Product with id {productId} does not exist");
                    return false;
                }

                var basketItems = _basketItemsRepo.GetBasketItemsForBasket(basket);

                if (basketItems?.Any() != true)
                {
                    _logger.LogInformation($"Could not remove product from basket: Basket with id {basketId} contains no items");
                }
                else
                {
                    var basketItem = basketItems.FirstOrDefault(x => x.BasketId == basketId && x.ProductId == productId);

                    if (basketItem == null)
                    {
                        _logger.LogInformation($"Could not remove product from basket: Basket with id {basketId} contains no products with id {productId}");
                    }
                    else
                    {
                        basketItem.Quantity -= quantity;

                        if (basketItem.Quantity <= 0)
                            _basketItemsRepo.DeleteBasketItem(basketItem);
                        else
                            _basketItemsRepo.SaveBasketItem(basketItem);
                    }
                }

                _logger.LogInformation($"Successfully removed {quantity} x productId {productId} from basketId {basketId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when removign {quantity} x product {productId} from basket {basketId}");
                return false;
            }
        }

        public Voucher? AddVoucherToBasket(int basketId, int voucherId)
        {
            try
            {
                var basket = _basketRepo.GetBasket(basketId);

                if (basket == null)
                {
                    _logger.LogInformation($"Could not add voucher to basket: Basket with id {basketId} does not exist");
                    return null;
                }

                var voucher = _voucherRepo.GetVoucher(voucherId);

                if (voucher == null)
                {
                    _logger.LogInformation($"Could not add voucher to basket: Voucher with id {voucherId} does not exist");
                    return null;
                }

                basket.VoucherId = voucherId;

                _basketRepo.SaveBasket(basket);

                return voucher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when adding voucher {voucherId} to basket {basketId}");
                return null;
            }
        }

        public bool RemoveVoucherFromBasket(int basketId)
        {
            try
            {
                var basket = _basketRepo.GetBasket(basketId);

                if (basket == null)
                {
                    _logger.LogInformation($"Could not remove voucher from basket: Basket with id {basketId} does not exist");
                    return false;
                }

                basket.VoucherId = null;

                _basketRepo.SaveBasket(basket);

                _logger.LogInformation($"Successfully removed voucher from basketId {basketId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when removing voucher from basket {basketId}");
                return false;
            }
        }
    }
}
