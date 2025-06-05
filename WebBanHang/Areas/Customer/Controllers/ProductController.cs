using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int categoryId = 1)
        {
            var categories = _db.Categories.
                             Select(c => new CategoryViewModel
                             { Id = c.Id, Name = c.Name, TotalProduct = _db.Products.Where(p => p.CategoryId == c.Id).Count() }).ToList();
            var products = _db.Products.Where(p => p.CategoryId == categoryId).ToList();

            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.Title = _db.Categories.Find(categoryId).Name;
            ViewBag.SelectedCategoryId = categoryId;

            return View();
        }

        // Ajax: Lấy sản phẩm theo category
        [HttpGet]
        public IActionResult Ajax(int categoryId = 1)
        {
            var categories = _db.Categories.
                             Select(c => new CategoryViewModel
                             { Id = c.Id, Name = c.Name, TotalProduct = _db.Products.Where(p => p.CategoryId == c.Id).Count() }).ToList();
            var products = _db.Products.Where(p => p.CategoryId == categoryId).ToList();

            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.SelectedCategoryId = categoryId;

            return PartialView("_ProductListPartial", products);
        }
    }
}
