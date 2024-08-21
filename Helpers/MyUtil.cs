using System.Text;

namespace ECommerceMVC.Helpers
{
    public class MyUtil
    {
        public static string UpLoadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                // Tạo đường dẫn đầy đủ
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

                // Kiểm tra thư mục tồn tại, nếu không thì tạo mới
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                // Kết hợp đường dẫn với tên file
                var filePath = Path.Combine(fullPath, Hinh.FileName);

                // Sử dụng FileMode.Create để ghi đè file nếu nó đã tồn tại
                using (var myfile = new FileStream(filePath, FileMode.Create))
                {
                    Hinh.CopyTo(myfile);
                }

                // Trả về tên file
                return Hinh.FileName;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                // Ví dụ: Console.WriteLine(ex.Message);

                return string.Empty;
            }
        }


        public static string GenerrateRandomKey(int length = 5)
        {

            var pattern = @"qazwscedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP!";
            var sb=new StringBuilder();
            var rd=new Random();
            for(int  i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0,pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
