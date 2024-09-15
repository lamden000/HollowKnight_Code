using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChinh
{
    internal class ThongtinPlayer
    {
        public string taitk;
        public string hoten;
        public Image daidien;
        public decimal tien;
        public string dangtien;

        public decimal Tien { get => tien; set
            {
                tien = value;
                dangtien = xulytien(tien);
            }
        }

        public void capnhat(decimal s)
        {
            tien = s;
            dangtien = xulytien(tien);
        }
        public ThongtinPlayer()
        {
            hoten = "";
        }
        public ThongtinPlayer(string taitk, string hoten, decimal tien)
        {
            this.taitk = taitk;
            this.hoten = hoten;
            this.daidien = daidien;
            this.tien = tien;
            dangtien = xulytien(tien);
        }
        string xulytien(decimal s)
        {
            if (s < 1000)
            {
                return s.ToString() + "K";
            }
            decimal k = Math.Round(s / 1000, 2);
            if (s < 10000) return k.ToString() + "M";
            return k.ToString() + "G";
        }
    }
}
