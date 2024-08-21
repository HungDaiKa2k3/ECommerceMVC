using AutoMapper;
using ECommerceMVC.Helpers;
using ECommerceMVC.Models;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace ECommerceMVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;

        public KhachHangController(Hshop2023Context context, IMapper mapper) 
        { 
            db = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKey = MyUtil.GenerrateRandomKey();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;//sử lý khi dùng Mail để active
                    khachHang.VaiTro = 0;
                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UpLoadHinh(Hinh, "KhachHang");
                    }
                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "HangHoa");
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl) 
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khahHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khahHang == null)
                {
                    ModelState.AddModelError("loi", "Thông tin đăng nhập sai");
                }
                else
                {
                    if (!khahHang.HieuLuc)
                    {
                        ModelState.AddModelError("loi", "Tài khoản bị khóa");
                    }
                    else
                    {
                        if (khahHang.MatKhau != model.Password.ToMd5Hash(khahHang.RandomKey))
                        {
                            ModelState.AddModelError("loi", "Thông tin đăng nhập sai");
                        }
                        else
                        {
                            //
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email,khahHang.Email),
                                new Claim(ClaimTypes.Name,khahHang.HoTen),
                                new Claim(MySetting.CLAIM_CUSTOMER_ID,khahHang.MaKh),

                                //Claim động
                                new Claim(ClaimTypes.Role,"Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);
                            //Nếu dùng chức năng mà chưa đăng nhập->chuyển sang trang đăng nhập-> Đăng nhập thành công->chuyển về trang chức năng ban đầu
                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                //Đăng nhập trực tiếp -> chuyển sang trang home
                                return Redirect("/");
                            }
                        }
                    }
                }
            }
            return View();
        }

       
        [Authorize]
        public IActionResult Profile()
        {
            return View(); 
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Logout
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
