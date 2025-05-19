using WebShop10.Models;

namespace WebShop10.Extensions
{
    public class CartItem
    {
        public ProductDTO Product { get; set; }
        public decimal Quantity { get; set; }

        public decimal GetTotal()
        {
            return Quantity * Product.Price;
        }
    }
}
