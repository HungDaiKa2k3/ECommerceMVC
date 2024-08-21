using ECommerceMVC.Helpers;
using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ECommerceMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;
        public CartController(Hshop2023Context context)
        {
            db = context;
        }
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new
            List<CartItem>();
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHang == id);
            if ((item == null))
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã{id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHang = hangHoa.MaHh,
                    TenHang = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? "",
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int maHang, string action)
        {
            var gioHang = Cart;
            var cartItem = gioHang.FirstOrDefault(ci => ci.MaHang == maHang);

            if (cartItem != null)
            {
                if (action == "increase")
                {
                    cartItem.SoLuong += 1;
                }
                else if (action == "decrease" && cartItem.SoLuong > 1)
                {
                    cartItem.SoLuong -= 1;
                }

                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }

            return PartialView("Cart", gioHang);
        }

        [HttpPost]
        public IActionResult RemoveCart(int maHang)
        {
            var gioHang = Cart;
            var cartItem = gioHang.FirstOrDefault(ci => ci.MaHang == maHang);
            if (cartItem != null)
            {
                gioHang.Remove(cartItem);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            }          
            return PartialView("Cart", gioHang);
        }


        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/HangHoa");
            }
            return View(Cart);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var customerID= HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER_ID).Value;

                var khachHang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerID);
                }

                var hoaDon = new HoaDon()
                {
                    MaKh = customerID,
                    HoTen = model.HoTen ?? khachHang.HoTen,
                    DiaChi = model.DiaChi ?? khachHang.DiaChi,
                    DienThoai = model.DienThoai ?? khachHang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachVanChuyen = "GRAB",
                    CachThanhToan = "COD",
                    MaTrangThai = 0,
                    GhiChu=model.GhiChu
                };

                db.Database.BeginTransaction();
                try
                {
                    
                    db.Add(hoaDon);
                    db.SaveChanges();

                    var cthds=new List<ChiTietHd>();
                    foreach(var item in Cart){
                        cthds.Add(new ChiTietHd
                        {
                            MaHd = hoaDon.MaHd,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia,
                            MaHh = item.MaHang,
                            GiamGia = 0,
                            
                        });
                    }
                    db.AddRange(cthds);
                    db.SaveChanges();
                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY,new List<CartItem>());

                    db.Database.CommitTransaction();
                    return View("Success");
                }
                catch (Exception)
                {

                    db.Database.RollbackTransaction();
                }
            }
            return View(model);
        }
    }
}
