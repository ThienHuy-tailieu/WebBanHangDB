using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //lấy cart từ Session
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");
            if (cart == null)
            {
                cart = new Cart();
            }
            //gửi cart qua View thông qua ViewBag
            ViewBag.CART = cart;
            return View();
        }
        public IActionResult ProcessOrder(Order order)
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");
            if (ModelState.IsValid)
            {
                //Lưu trữ đơn hàng
                //b1 Thêm Order vào CSDL

                //tạo đơn hàng
                order.OrderDate = DateTime.Now;
                order.Total = cart.Total;
                order.State = "Pending";
                //thêm Order vào CSDL
                _db.Orders.Add(order);
                _db.SaveChanges();
                //b2 Thêm OrderDetail vào CSDL
                foreach (var item in cart.Items) //duyệt qua từng sản phẩm trong giỏ hàng
                {
                    //tạo đối tượng OrderDetail
                    var orderDetail = new OrderDetail
                    {

                        OrderId = order.Id,
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity
                    };
                    //them OrderDetail vào CSDL
                    _db.OrderDetails.Add(orderDetail);
                    _db.SaveChanges();

                }
                //b3 xóa giỏ hàng
                HttpContext.Session.Remove("CART");
                //b4 trả về View kết quả
                return View("Result");
            }
            //gửi cart qua View thông qua ViewBag
            ViewBag.CART = cart;
            return View("Index", order);
        }
    }
}
