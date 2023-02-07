using Shopping.DataAccess;
using Shopping.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Repositories
{
    public class BasketItemsRepository : IBasketItemsRepository
    {
        private readonly ShoppingContext _shoppingContext;
        private readonly ILogger<BasketItemsRepository> _logger;

        public BasketItemsRepository(ShoppingContext shoppingContext, ILogger<BasketItemsRepository> logger)
        {
            _shoppingContext = shoppingContext;
            _logger = logger;
        }

        public BasketItem? SaveBasketItem(BasketItem basketItem)
        {
            try
            {
                if (basketItem != null && _shoppingContext.BasketItems.Any(x => x.Id == basketItem.Id))
                {
                    var entry = _shoppingContext.BasketItems.Entry(basketItem);
                    entry.State = EntityState.Modified;
                    _shoppingContext.SaveChanges();
                    basketItem = entry.Entity;
                    _logger.LogInformation($"Modified basket item with id {entry.Entity.Id}");
                }
                else
                {
                    var entry = _shoppingContext.BasketItems.Add(basketItem ?? new BasketItem());
                    _shoppingContext.SaveChanges();
                    basketItem = entry.Entity;
                    _logger.LogInformation($"Created basket item with id {entry.Entity.Id}");
                }

                return basketItem;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred when creating basket item");
                return default;
            }
        }

        public BasketItem? GetBasketItem(int id)
        {
            try
            {
                var basketItem = _shoppingContext.BasketItems.FirstOrDefault(x => x.Id == id);

                if (basketItem == null)
                {
                    _logger.LogWarning($"No basket item found with id {id}");
                    return null;
                }

                _logger.LogInformation($"Successfully obtained basket item with id {id}");

                return basketItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when getting basket item with id {id}");
                return null;
            }
        }

        public bool DeleteBasketItem(int id) 
        {
            var basketItem = _shoppingContext.BasketItems.FirstOrDefault(x => x.Id == id);

            if (basketItem == null)
            {
                _logger.LogWarning($"Could not delete basket item: No basket item found with id {id}");
                return false;
            }

            return DeleteBasketItem(basketItem);
        }

        public bool DeleteBasketItem(BasketItem basketItem)
        {
            try
            {
                _shoppingContext.BasketItems.Entry(basketItem).State = EntityState.Deleted;

                _shoppingContext.SaveChanges();

                _logger.LogInformation($"Successfully delete basket item with id {basketItem.Id}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when deleting basket item with id {basketItem.Id}");
                return false;
            }
        }

        public IEnumerable<BasketItem> GetBasketItemsForBasket(Basket basket)
        {
            try
            {
                var basketItems = _shoppingContext.BasketItems
                    .Where(x => x.BasketId == basket.Id)?
                    .ToList();

                if (basketItems?.Any() != true)
                {
                    _logger.LogInformation($"No basket items found for basketId {basket.Id}");
                    return Enumerable.Empty<BasketItem>();
                }

                _logger.LogInformation($"Found {basketItems.Count()} basket items for basketId {basket.Id}");

                return basketItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when getting basket items for basketId {basket.Id}");
                return Enumerable.Empty<BasketItem>();
            }
        }

        public bool BasketItemExists(int id)
        {
            return _shoppingContext.BasketItems.Any(x => x.Id == id);
        }
    }
}
