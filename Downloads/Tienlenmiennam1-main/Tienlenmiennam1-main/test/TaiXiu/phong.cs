using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaiXiu
{
    public class phong : IEquatable<phong>
    {
        public int maphong;
        public int songuoi;
        public string chuphong;
        public Image nguoi;
        public int dangchoi;
        public string tienphong;
        public string mk;
        public bool Equals(phong other)
        {
            return this.maphong == other.maphong && this.songuoi == other.songuoi && this.dangchoi == other.dangchoi;
        }
    }
}
