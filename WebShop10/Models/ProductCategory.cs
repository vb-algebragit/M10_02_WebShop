using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop10.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        [NotMapped]
        public string CategoryTitle { get; set; } = string.Empty;

        [NotMapped]
        public string ProductTitle { get; set; } = string.Empty;
    }
}
