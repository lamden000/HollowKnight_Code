using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static System.Windows.Forms.DataFormats;

namespace TaiXiu
{
    public partial class TaoBan : Form
    {
        public delegate void Data(string tien, string matkhau);
        public Data TruyenMaPhong;
        public bool mo = false;
        public TaoBan(Form1 f)
        {
            InitializeComponent();
            comboBox1.Text = "5k";
            mo = true;
            TruyenMaPhong += f.XuLySukienTruyenDuLieuTuForm2;
        }
        public void TruyenDuLieu(string duLieu)
        {
            MessageBox.Show(duLieu + "!");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TruyenMaPhong?.Invoke(comboBox1.Text, textBox1.Text);
        }

        private void TaoBan_FormClosing(object sender, FormClosingEventArgs e)
        {
            mo = false;
        }
    }
}
