using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChinh
{
    internal class TaiKhoan
    {
        private string Taikhoan;
        private string matKhau;
        private string Hoten;
        private string email;
        private Decimal tien;
        private Image anhcanhan;
        public TaiKhoan()
        {

        }
        public TaiKhoan(string taikhoan, string matkhau, string email, decimal tien, string hoten)
        {
            Hoten1 = hoten;
            this.Email = email;
            MatKhau = matkhau;
            Taikhoan = taikhoan;
            this.Tien = tien;
        }

        public string Taikhoan1 { get => Taikhoan; set => Taikhoan = value; }
        public string MatKhau { get => matKhau; set => matKhau = value; }
        public string Email { get => email; set => email = value; }
        public string Hoten1 { get => Hoten; set => Hoten = value; }
        public Image Anhcanhan { get => anhcanhan; set => anhcanhan = value; }
        public decimal Tien { get => tien; set => tien = value; }
    }
}
