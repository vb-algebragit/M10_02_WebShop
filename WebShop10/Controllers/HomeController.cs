using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop10.Data;
using WebShop10.Extensions;
using WebShop10.Models;

namespace WebShop10.Controllers
{
    public class HomeController : Controller
    {
        public const string SessionKeyName = "_cart";
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View();
        }

        public IActionResult Products(int? categoryId)
        {
            List<Product> products = null;

            if (categoryId == null)
            {
                products = _context.Product.ToList();
            }
            else
            {
                products = (
                    from prod in _context.Product
                    join prodCat in _context.ProductCategory on prod.Id equals prodCat.ProductId
                    where prodCat.CategoryId == categoryId
                    select new Product
                    {
                        Id = prod.Id,
                        Title = prod.Title,
                        Description = prod.Description,
                        Quantity = prod.Quantity,
                        Price = prod.Price
                    }
                    ).ToList();
            }

            ViewBag.Categories = _context.Category.Select(cat =>
                                  new SelectListItem
                                  {
                                      Value = cat.Id.ToString(),
                                      Text = cat.Title
                                  }).ToList();

            return View(products);
        }

        public IActionResult Order(List<string> errors)
        {
            List<CartItem> cart =
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            decimal sum = 0;
            ViewBag.TotalPrice = cart.Sum(item => sum + item.GetTotal());

            ViewBag.Errors = errors;

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(
            //[Bind("Total,ShippingFirstName,ShippingLastName,ShippingAddress,ShippingZipCode,ShippingCity,ShippingCountry,ShippingEmail,ShippingPhone,Message")]
            [Bind("UserId,Total,ShippingFirstName,ShippingLastName,ShippingAddress,ShippingZipCode,ShippingCity,ShippingCountry,ShippingEmail,ShippingPhone,Message")]
            Order order)
        {
            List<CartItem> cart =
                HttpContext.Session.GetObjectFromJson<List<CartItem>>(SessionKeyName) ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            //// bez UserId u View-u...
            //var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userId != null)
            //{
            //    order.UserId = userId;
            //    ModelState.ClearValidationState(nameof(order.UserId));
            //    TryValidateModel(order);
            //}

            var modelErrors = new List<string>();

            if (ModelState.IsValid)
            {
                // unos je ispravan...

                // kopiraj Shipping u Billing
                if (order.UseSameAddress)
                {
                    order.BillingFirstName = order.ShippingFirstName;
                    order.BillingLastName = order.ShippingLastName;
                    // primjer eventualne provjere podataka...
                    //if (!string.IsNullOrEmpty(order.ShippingAddress))
                    //{
                        order.BillingAddress = order.ShippingAddress;
                    //}
                    order.BillingZipCode = order.ShippingZipCode;
                    order.BillingCity = order.ShippingCity;
                    order.BillingCountry = order.ShippingCountry;
                    order.BillingEmail = order.ShippingEmail;
                    order.BillingPhone = order.ShippingPhone;
                }

                // spremanje narudžbe...
                _context.Order.Add(order);
                _context.SaveChanges();

                // spremanje stavaka narudžbe...
                int orderId = order.Id;

                foreach (var item in cart)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        OrderId = orderId,
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity,
                        Total = item.GetTotal()
                    };
                    _context.OrderItem.Add(orderItem);
                    //_context.SaveChanges();
                }
                _context.SaveChanges();

                // pražnjenje košarice...
                HttpContext.Session.SetObjectAsJson(SessionKeyName, "");

                return RedirectToAction(nameof(Index), new { message = "Thank You for your order!" });
            }
            else
            {
                // unos nije ispravan...
                // "pokupi" sve greške...
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
            }

            return RedirectToAction(nameof(Order), new { errors = modelErrors });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
