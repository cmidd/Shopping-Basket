using Shopping.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shopping.DataAccess
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }
    }
}
