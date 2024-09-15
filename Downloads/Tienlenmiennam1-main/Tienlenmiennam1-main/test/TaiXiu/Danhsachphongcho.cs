using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaiXiu
{
    public partial class Danhsachphongcho : Form
    {
        public delegate void Data(int maphong);
        public Data TruyenMaPhong;
        public int done = 0;
        public int loaded = 0;
        MatKhau nhapMk;
        ThongtinPlayer k;
        List<phong> lastLoadedData = new List<phong>();
        public Danhsachphongcho(Form1 form)
        {
            InitializeComponent();
            k = new ThongtinPlayer();
            form.player = new Form1.TruyDuLieu(LoadThongtinPlayer);
            form.DSPhong = new Form1.TruyenDuLieu(LoadDanhSach);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            loaded = 1;
        }
        private void LoadThongtinPlayer(ThongtinPlayer p1)
        {
            k = p1;
        }
        private void LoadDanhSach(List<phong> ListPhong)
        {
            if (!ListPhong.SequenceEqual(lastLoadedData))
            {
                lastLoadedData = new List<phong>(ListPhong);
                listView1.Items.Clear();
                string[] str = { "Chọn", "", "", "", "", "", "", "" };
                ListViewItem item;
                for (int i = 0; i < ListPhong.Count; i++)
                {
                    item = new ListViewItem(str);
                    item.SubItems[1].Text = ListPhong[i].maphong.ToString();
                    item.SubItems[2].Text = ListPhong[i].songuoi.ToString();
                    item.SubItems[3].Text = ListPhong[i].dangchoi == 1 ? "Đang Chơi" : "Chưa bắt đầu";
                    item.SubItems[4].Text = ListPhong[i].tienphong;
                    item.SubItems[5].Text = ListPhong[i].mk == "" ? "Công cộng" : "Riêng tư";
                    item.SubItems[6].Text = ListPhong[i].mk;
                    item.SubItems[7].Text = ListPhong[i].chuphong;
                    listView1.Items.Add(item);
                }
            }
        }
        public void phongday()
        {
            MessageBox.Show("Phòng đã đầy");
        }
        void ChonPhong(int soPhong, string loaiPhong)
        {
            if (loaiPhong == "Công cộng")
            {
                TruyenMaPhong(soPhong);
                done = 1;
                this.Close();
            }
            else
            {
                nhapMk = new MatKhau(soPhong);
                nhapMk.truyenMK = new MatKhau.TruyenMKphong(NhanMK);
                loaded = 0;
                nhapMk.ShowDialog();
                if (nhapMk.done == -1)
                {
                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(500);
                    });
                    thread.Start();
                }
            }

        }
        void NhanMK(string mk, int soPhong)
        {
            nhapMk.Close();
            if (listView1.Items[soPhong - 1].SubItems[6].Text == mk)
            {
                ChonPhong(soPhong, "Công cộng");
            }
            else
            {
                MessageBox.Show("Sai mật khẩu", "Sai mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Danhsachphongcho_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (done != 1)
                done = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int exist = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[7].Text == textBox1.Text)
                {
                    exist = 1;
                    ChonPhong(int.Parse(listView1.Items[i].SubItems[1].Text), listView1.Items[i].SubItems[5].Text);
                }
            }
            if (exist == 0)
                MessageBox.Show("Chủ phòng không tồn tại");
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);

            if (clickedItem != null)
            {
                if (clickedItem.SubItems[5].Text == "Công cộng")
                {
                    ChonPhong(int.Parse(clickedItem.SubItems[1].Text), "Công cộng");
                }
                else
                {
                    ChonPhong(int.Parse(clickedItem.SubItems[1].Text), "Riêng tư");
                }

            }
        }
    }
}
