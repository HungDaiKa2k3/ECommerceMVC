using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.ViewComponents
{
    public class MenuLoaiViewComponent:ViewComponent
    {
        private readonly Hshop2023Context db;
        public  MenuLoaiViewComponent(Hshop2023Context context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await db.Loais.Select(loai => new MenuLoaiVM
            {
                MaLoai=loai.MaLoai,
                TenLoai = loai.TenLoai,
                SoLuong = loai.HangHoas.Count()
            }).ToListAsync();
            return View(data);
            // return View("Default",data); Default thay đổi tùy ý
        }
        //Tạo thư mục ViewComponents/cs->ViewModels/cs->View/Shared/MenuLoai/cshtml
    }
}
