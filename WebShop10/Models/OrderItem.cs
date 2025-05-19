using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop10.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal Total { get; set; }

        [NotMapped]
        public string ProductTitle { get; set; } = string.Empty;
    }
}
