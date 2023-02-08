using Shopping.Models.Entities;

namespace Shopping.Repositories
{
    public interface IVoucherRepository
    {
        Voucher? SaveVoucher(Voucher voucher);

        Voucher? GetVoucher(int id);

        bool DeleteVoucher(int id);

        bool DeleteVoucher(Voucher voucher);

        bool VoucherExists(int id);
    }
}
