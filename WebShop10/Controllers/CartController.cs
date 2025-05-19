using Microsoft.AspNetCore.Mvc;
using WebShop10.Data;
using WebShop10.Extensions;
using WebShop10.Models;

namespace WebShop10.Controllers
{
    public class CartController : Controller
    {
        public const string SessionKeyName = "_cart";
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartItem> cart =
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            decimal sum = 0;
            ViewBag.TotalPrice = cart.Sum(item => sum + item.GetTotal());

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int productId)
        {
            // 1. dohvat iz Sessiona
            List<CartItem> cartItems = 
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            // 2. provjera dohvaćenih podataka
            if (cartItems.Count == 0)
            {
                // 2.1. ako nema podataka, dodajemo novi proizvod
                Product product = _context.Product.Find(productId);
                CartItem cartItem = new CartItem()
                {
                    Product = new ProductDTO()
                    {
                        Id = product.Id,
                        Title = product.Title,
                        Description = product.Description,
                        Quantity = product.Quantity,
                        Price = product.Price
                    },
                    Quantity = 1
                };
                cartItems.Add(cartItem);
            }
            else
            {
                // 2.2. ako ima podataka, provjeravamo koji su
                int productIndex = ExistsInCart(productId);

                if (productIndex == -1)
                {
                    // 2.2.1. ako proizvoda nema u košarici, dodajemo ga
                    Product product = _context.Product.Find(productId);
                    CartItem cartItem = new CartItem()
                    {
                        Product = new ProductDTO()
                        {
                            Id = product.Id,
                            Title = product.Title,
                            Description = product.Description,
                            Quantity = product.Quantity,
                            Price = product.Price
                        },
                        Quantity = 1
                    };
                    cartItems.Add(cartItem);
                }
                else
                {
                    // 2.2.2. ako proizvod već postoji, povećavamo količinu za 1
                    cartItems[productIndex].Quantity++;
                }
            }

            // 3. spremanje podataka u Session
            HttpContext.Session.SetObjectAsJson(SessionKeyName, cartItems);

            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int productId)
        {
            // 1. dohvat iz Sessiona
            List<CartItem> cartItems =
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            // 2. provjera dohvaćenih podataka
            int productIndex = ExistsInCart(productId);
            if (productIndex != -1)
            {
                // PAZI!!! - ovdje brišemo cijeli "index"
                cartItems.RemoveAt(productIndex);

                // dorada: omogućiti uklanjanje jednog komada proizvoda i umanjivanje količine

                // 3. spremanje podataka u Session
                HttpContext.Session.SetObjectAsJson(SessionKeyName, cartItems);
            }

            return RedirectToAction(nameof(Index));
        }

        private int ExistsInCart(int productId)
        {
            List<CartItem> cartItems =
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName);

            for (int i = 0; i < cartItems.Count; i++)
            {
                if (cartItems[i].Product.Id == productId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
