using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaiXiu
{
    public partial class MatKhau : Form
    {
        public delegate void TruyenMKphong(string str, int soPhong);
        public TruyenMKphong truyenMK;
        int soPhong;
        public int done=0;
        public MatKhau(int soPhong)
        {
            InitializeComponent();
            this.soPhong = soPhong;
            textBox1.Focus();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            truyenMK(textBox1.Text, soPhong);
            done = 1;
        }

        private void MatKhau_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(done !=1) 
                done=-1;
        }
    }
}
