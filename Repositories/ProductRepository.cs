using Shopping.DataAccess;
using Shopping.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Shopping.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingContext _shoppingContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ShoppingContext shoppingContext, ILogger<ProductRepository> logger)
        {
            _shoppingContext = shoppingContext;
            _logger = logger;
        }

        public Product? SaveProduct(Product product)
        {
            try
            {
                if (product != null && _shoppingContext.Products.Any(x => x.Id == product.Id))
                {
                    var entry = _shoppingContext.Products.Entry(product);
                    entry.State = EntityState.Modified;
                    _shoppingContext.SaveChanges();
                    product = entry.Entity;
                    _logger.LogInformation($"Modified product with id {entry.Entity.Id}");
                }
                else
                {
                    var entry = _shoppingContext.Products.Add(product ?? new Product());
                    _shoppingContext.SaveChanges();
                    product = entry.Entity;
                    _logger.LogInformation($"Created product with id {entry.Entity.Id}");
                }

                return product;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred when creating product");
                return default;
            }
        }

        public Product? GetProduct(int id)
        {
            try
            {
                var product = _shoppingContext.Products.FirstOrDefault(x => x.Id == id);

                if (product == null)
                {
                    _logger.LogWarning($"No product found with id {id}");
                    return null;
                }

                _logger.LogInformation($"Successfully obtained product with id {id}");

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when getting product with id {id}");
                return null;
            }
        }

        public IEnumerable<Product> GetAll()
        {
            try
            {
                return _shoppingContext.Products.OrderByDescending(x => x.Id).ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred obtaining all products");
                return Enumerable.Empty<Product>();
            }
        }

        public bool DeleteProduct(int id) 
        {
            var product = _shoppingContext.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                _logger.LogWarning($"Could not delete product: No product found with id {id}");
                return false;
            }

            return DeleteProduct(product);
        }

        public bool DeleteProduct(Product product)
        {
            try
            {
                _shoppingContext.Products.Entry(product).State = EntityState.Deleted;

                _shoppingContext.SaveChanges();

                _logger.LogInformation($"Successfully delete product with id {product.Id}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when deleting product with id {product.Id}");
                return false;
            }
        }

        public bool ProductExists(int id)
        {
            return _shoppingContext.Products.Any(x => x.Id == id);
        }
    }
}
