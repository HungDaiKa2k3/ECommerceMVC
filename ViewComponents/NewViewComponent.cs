using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class NewViewComponent:ViewComponent
    {
        private readonly Hshop2023Context db;
        public NewViewComponent(Hshop2023Context context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = db.HangHoas.Select(hang => new HangHoaVM
            {
                MaHang = hang.MaHh,
                TenHang = hang.TenHh,
                DonGia = hang.DonGia ?? 0,
                Hinh = hang.Hinh ?? "",
                TenLoai = hang.MaLoaiNavigation.TenLoai,
                NamSanXuat = hang.NgaySx
            }).OrderByDescending(hang => hang.NamSanXuat).Take(10);
            return View("New", data);
        }
    }
}
