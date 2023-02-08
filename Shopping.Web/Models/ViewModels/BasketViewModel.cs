using Shopping.Models.Entities;

namespace Shopping.Models.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; } = Enumerable.Empty<ProductViewModel>();

        public decimal CurrentTotal { get; set; } = 0;

        public decimal OriginalTotal { get; set; } = 0;

        public Voucher? Voucher { get; set; }

        public string AddVoucherUrl { get; set; } = string.Empty;

        public string RemoveVoucherUrl { get; set; } = string.Empty;

        public bool HasProducts => Products?.Any() == true;

        public bool HasVoucher => Voucher != null;

        public string GetCurrentTotalStr => CurrentTotal.ToString("#.00");

        public string GetOriginalTotalStr => OriginalTotal.ToString("#.00");
    }
}
