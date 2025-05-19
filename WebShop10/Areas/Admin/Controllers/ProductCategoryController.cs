using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop10.Data;
using WebShop10.Models;

namespace WebShop10.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductCategory
        public async Task<IActionResult> Index(int productId)
        {
            List<ProductCategory> list = await (
                from product_cat in _context.ProductCategory
                where product_cat.ProductId == productId
                select new ProductCategory
                {
                    Id = product_cat.Id,
                    ProductId = product_cat.ProductId,
                    ProductTitle = (from pro in _context.Product where pro.Id == product_cat.ProductId select pro.Title).FirstOrDefault(),
                    CategoryId = product_cat.CategoryId,
                    CategoryTitle = (from cat in _context.Category where cat.Id == product_cat.CategoryId select cat.Title).FirstOrDefault(),
                }
                ).ToListAsync();

            ViewBag.ProductId = productId;

            return View(list);
        }

        // GET: Admin/ProductCategory/Details/5
        public async Task<IActionResult> Details(int? id, int productId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = productId;

            return View(productCategory);
        }

        // GET: Admin/ProductCategory/Create
        public IActionResult Create(int productId)
        {
            ViewBag.Categories = _context.Category.Select(cat =>
                                  new SelectListItem
                                  {
                                      Value = cat.Id.ToString(),
                                      Text = cat.Title
                                  }).ToList();

            ViewBag.ProductId = productId;

            return View();
        }

        // POST: Admin/ProductCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,ProductId")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { productId = productCategory.ProductId });
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategory/Edit/5
        public async Task<IActionResult> Edit(int? id, int productId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Category.Select(cat =>
                                  new SelectListItem
                                  {
                                      Value = cat.Id.ToString(),
                                      Text = cat.Title
                                  }).ToList();

            ViewBag.Products = _context.Product.Select(prod =>
                                  new SelectListItem
                                  {
                                      Value = prod.Id.ToString(),
                                      Text = prod.Title
                                  }).ToList();

            ViewBag.ProductId = productId;


            return View(productCategory);
        }

        // POST: Admin/ProductCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,ProductId")] ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { productId = productCategory.ProductId });
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategory/Delete/5
        public async Task<IActionResult> Delete(int? id, int productId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = productId;

            return View(productCategory);
        }

        // POST: Admin/ProductCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int productId)
        {
            var productCategory = await _context.ProductCategory.FindAsync(id);
            if (productCategory != null)
            {
                _context.ProductCategory.Remove(productCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { productId = productId });
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategory.Any(e => e.Id == id);
        }
    }
}
