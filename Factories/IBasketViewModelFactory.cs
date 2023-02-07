using Shopping.Models.Entities;
using Shopping.Models.ViewModels;

namespace Shopping.Factories
{
    public interface IBasketViewModelFactory
    {
        BasketViewModel Create(Basket basket);
    }
}
