using ECommerceMVC.Models;
using ECommerceMVC.Sevices;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class NewProductsViewComponent:ViewComponent
    {
        //private readonly Hshop2023Context db;
        //public NewProductsViewComponent(Hshop2023Context context)
        //{
        //    db = context;
        //}

        ProductListService productSevice;
        public NewProductsViewComponent(ProductListService _product) 
        {
            productSevice= _product;
        }
        public IViewComponentResult Invoke()
        {
            var data = productSevice.GetAllProducts().OrderByDescending(p=>p.NgaySx).Take(3);
            return View("NewProducts",data);
        }
    }
}
