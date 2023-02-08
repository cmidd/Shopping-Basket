using Shopping.Models.Entities;

namespace Shopping.Repositories
{
    public interface IBasketRepository
    {
        Basket? SaveBasket(Basket basket);

        Basket? GetBasket(int id);

        bool DeleteBasket(int id);

        bool DeleteBasket(Basket basket);

        bool BasketExists(int id);
    }
}
