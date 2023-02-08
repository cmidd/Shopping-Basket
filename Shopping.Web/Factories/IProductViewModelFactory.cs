using Shopping.Models.Entities;
using Shopping.Models.ViewModels;

namespace Shopping.Factories
{
    public interface IProductViewModelFactory
    {
        ProductViewModel Create(Product product);

        ProductViewModel Create(Product product, int basketId);
    }
}
