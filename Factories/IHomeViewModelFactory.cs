using Shopping.Models.Entities;
using Shopping.Models.ViewModels;

namespace Shopping.Factories
{
    public interface IHomeViewModelFactory
    {
        HomeViewModel Create(Basket basket);
    }
}
