using Shopping.Models.Entities;

namespace Shopping.Services
{
    public interface IBasketService
    {
        public Basket? CreateNewBasket();
        public Basket? GetCurrentBasket(HttpRequest currentRequest);
        public Product? AddProductToBasket(int basketId, int productId, int quantity = 1);
        public bool RemoveProductFromBasket(int basketId, int productId, int quantity = 1);
        public Voucher? AddVoucherToBasket(int basketId, int voucherId);
        public bool RemoveVoucherFromBasket(int basketId);
    }
}
