namespace ECommerceMVC.ViewModels
{
    public class HangHoaVM
    {
        public int MaHang {  get; set; }
        public string TenHang { get; set; }
        public string Hinh {  get; set; }
        public double DonGia {  get; set; }
        public string MoTa {  get; set; }
        public string TenLoai { get; set; }
        public DateTime NamSanXuat { get; set; }
    }

    public class ChiTietHangHoaVM
    {
        public int MaHang { get; set; }
        public string TenHang { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public string MoTa { get; set; }
        public string TenLoai { get; set; }
        public string ChiTiet {  get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon {  get; set; }
    }
}
