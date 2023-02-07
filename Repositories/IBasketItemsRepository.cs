using Shopping.Models.Entities;

namespace Shopping.Repositories
{
    public interface IBasketItemsRepository
    {
        BasketItem? SaveBasketItem(BasketItem basketItem);

        BasketItem? GetBasketItem(int id);

        bool DeleteBasketItem(int id);

        bool DeleteBasketItem(BasketItem basketItem);

        IEnumerable<BasketItem> GetBasketItemsForBasket(Basket basket);

        bool BasketItemExists(int id);
    }
}
