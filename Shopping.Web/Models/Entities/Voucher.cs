using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.Entities
{
    [Serializable]
    public class Voucher
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default;

        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal DiscountPercentage { get; set; } = 0; 
    }
}
