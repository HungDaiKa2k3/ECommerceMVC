namespace ECommerceMVC.ViewModels
{
    public class CartItem
    {
        public int MaHang { get; set; }
        public string TenHang { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien=>DonGia*SoLuong;
    }
}
