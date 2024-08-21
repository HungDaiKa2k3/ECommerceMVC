using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace ECommerceMVC.Sevices
{
    public class ProductListService
    {
        private readonly Hshop2023Context db;
        public ProductListService(Hshop2023Context context)
        {
            db = context;
        }

        public List<HangHoa> GetAllProducts()
        {
            return db.HangHoas.ToList();
        }

        public List<HangHoaVM> SearchByName(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
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

            return result;
        }
    }
}
