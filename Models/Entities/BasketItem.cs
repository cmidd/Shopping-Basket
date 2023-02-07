using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.Entities
{
    [Serializable]
    public class BasketItem
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default;

        [Required]
        public int BasketId { get; set; } = default;

        [Required]
        public int ProductId { get; set; } = default;

        [Required]
        public int Quantity { get; set; } = default;
    }
}
