using Shopping.DataAccess;
using Shopping.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ShoppingContext _shoppingContext;
        private readonly ILogger<VoucherRepository> _logger;

        public VoucherRepository(ShoppingContext shoppingContext, ILogger<VoucherRepository> logger)
        {
            _shoppingContext = shoppingContext;
            _logger = logger;
        }

        public Voucher? SaveVoucher(Voucher voucher)
        {
            try
            {
                if (voucher != null && _shoppingContext.Vouchers.Any(x => x.Id == voucher.Id))
                {
                    var entry = _shoppingContext.Vouchers.Entry(voucher);
                    entry.State = EntityState.Modified;
                    _shoppingContext.SaveChanges();
                    voucher = entry.Entity;
                    _logger.LogInformation($"Modified voucher with id {entry.Entity.Id}");
                }
                else
                {
                    var entry = _shoppingContext.Vouchers.Add(voucher ?? new Voucher());
                    _shoppingContext.SaveChanges();
                    voucher = entry.Entity;
                    _logger.LogInformation($"Created voucher with id {entry.Entity.Id}");
                }

                return voucher;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, $"Error occurred when creating voucher");
                return default;
            }
        }

        public Voucher? GetVoucher(int id)
        {
            try
            {
                var voucher = _shoppingContext.Vouchers.FirstOrDefault(x => x.Id == id);

                if (voucher == null)
                {
                    _logger.LogWarning($"No voucher found with id {id}");
                    return null;
                }

                _logger.LogInformation($"Successfully obtained voucher with id {id}");

                return voucher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when getting voucher with id {id}");
                return null;
            }
        }

        public bool DeleteVoucher(int id) 
        {
            var voucher = _shoppingContext.Vouchers.FirstOrDefault(x => x.Id == id);

            if (voucher == null)
            {
                _logger.LogWarning($"Could not delete voucher: No voucher found with id {id}");
                return false;
            }

            return DeleteVoucher(voucher);
        }

        public bool DeleteVoucher(Voucher voucher)
        {
            try
            {
                _shoppingContext.Vouchers.Entry(voucher).State = EntityState.Deleted;

                _shoppingContext.SaveChanges();

                _logger.LogInformation($"Successfully delete voucher with id {voucher.Id}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred when deleting voucher with id {voucher.Id}");
                return false;
            }
        }

        public bool VoucherExists(int id)
        {
            return _shoppingContext.Vouchers.Any(x => x.Id == id);
        }
    }
}
