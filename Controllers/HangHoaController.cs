using ECommerceMVC.Models;
using ECommerceMVC.Sevices;
using ECommerceMVC.ViewComponents;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;



namespace ECommerceMVC.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly ProductListService productListService;
        public HangHoaController(Hshop2023Context context, ProductListService listService) 
        { 
            db= context;
            productListService= listService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            var hangHoas = db.HangHoas.AsQueryable();
            var  result = await hangHoas.Select(p => new HangHoaVM
            {
                MaHang = p.MaHh,
                TenHang = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTa = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }).ToListAsync();

            var pagedResult = result.ToPagedList(pageNumber, pageSize);

            // Kiểm tra xem yêu cầu có phải là AJAX không
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("ProductItem", pagedResult);
            }

            return View(pagedResult);
        }
        public IActionResult Search(int?page, string? query)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            var result=productListService.SearchByName(query);
            var pagedResult = result.ToPagedList(pageNumber, pageSize);
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                
                return PartialView("ProductItem3", pagedResult);
            }
            return View(pagedResult);
        }

        public IActionResult SearchDM(int? loai, int? page)
        {
            TempData["loai"] = loai;
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            var hangHoas = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHang = p.MaHh,
                TenHang = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTa = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }).ToList();
            var pagedResult = result.ToPagedList(pageNumber, pageSize);

            return PartialView("ProductItem2", pagedResult);
        }


        public IActionResult Detail(int id)
        {
            var data=db.HangHoas
                .Include(p=>p.MaLoaiNavigation)
                .SingleOrDefault(p=>p.MaHh==id);
            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHang = data.MaHh,
                TenHang = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                Hinh=data.Hinh ??"",
                MoTa = data.MoTaDonVi ??"",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon=10,
                DiemDanhGia = 5
            };
            return View(result); 
        }

        public IActionResult OnPost()
        {
            var message = new MessageViewComponent.Message();
            message.title = "Thông báo";
            message.htmlcontent = "Cảm ơn bạn đã gửi thông báo";
            message.secondwait = 10;
            message.urlredirect = "/KhachHang/DangNhap";
            return ViewComponent("Message",message);
        }
    }
}
