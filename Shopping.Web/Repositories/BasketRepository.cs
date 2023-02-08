using Shopping.DataAccess;
using Shopping.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Shopping.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ShoppingContext _shoppingContext;
        private readonly ILogger<BasketRepository> _logger;

        public BasketRepository(ShoppingContext shoppingContext, ILogger<BasketRepository> logger)
        {
            _shoppingContext = shoppingContext;
            _logger = logger;
        }

        public Basket? SaveBasket(Basket basket)
        {
            try
            {
                if (basket != null && _shoppingContext.Baskets.Any(x => x.Id == basket.Id))
                {
                    var entry = _shoppingContext.Baskets.Entry(basket);
                    entry.State = EntityState.Modified;
                    _shoppingContext.SaveChanges();
                    basket = entry.Entity;
                    _logger.LogInformation($"Modified basket with id {basket.Id}");
                }
                else
                {
                    var entry = _shoppingContext.Baskets.Add(new Basket());
                    _shoppingContext.SaveChanges();
                    basket = entry.Entity;
                    _logger.LogInformation($"Created basket with id {basket.Id}");
                }

                return basket;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred when saving basket");
                return basket;
            }
        }

        public Basket? GetBasket(int id)
        {
            try
            {
                var basket = _shoppingContext.Baskets.FirstOrDefault(x => x.Id == id);

                if (basket == null)
                {
                    _logger.LogWarning($"No basket found with id {id}");
                    return null;
                }

                _logger.LogInformation($"Successfully obtained basket with id {id}");

                return basket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when getting basket with id {id}");
                return null;
            }
        }

        public bool DeleteBasket(int id) 
        {
            var basket = _shoppingContext.Baskets.FirstOrDefault(x => x.Id == id);

            if (basket == null)
            {
                _logger.LogWarning($"Could not delete basket: No basket found with id {id}");
                return false;
            }

            return DeleteBasket(basket);
        }

        public bool DeleteBasket(Basket basket)
        {
            try
            {
                _shoppingContext.Baskets.Entry(basket).State = EntityState.Deleted;

                _shoppingContext.SaveChanges();

                _logger.LogInformation($"Successfully delete basket with id {basket.Id}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when deleting basket with id {basket.Id}");
                return false;
            }
        }

        public bool BasketExists(int id)
        {
            return _shoppingContext.Baskets.Any(x => x.Id == id);
        }
    }
}
