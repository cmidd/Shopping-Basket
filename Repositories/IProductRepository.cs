using Shopping.Models.Entities;

namespace Shopping.Repositories
{
    public interface IProductRepository
    {
        Product? SaveProduct(Product product);

        Product? GetProduct(int id);

        IEnumerable<Product> GetAll();

        bool DeleteProduct(int id);

        bool DeleteProduct(Product product);

        bool ProductExists(int id);
    }
}
