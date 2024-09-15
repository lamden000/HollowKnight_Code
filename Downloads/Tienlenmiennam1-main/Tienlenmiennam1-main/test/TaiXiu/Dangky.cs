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
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace TaiXiu
{
    public partial class Dangky : Form
    {
        int stt;
        private bool showPassword = false;
        private bool showPassword1 = false;
        public Dangky()
        {
            InitializeComponent();
            Connect();
        }
        public bool Checkaccount(string ac)
        {
            return Regex.IsMatch(ac, "^[a-zA-Z0-9]{6,24}$");
        }
        public bool checkemail(string e)
        {
            return Regex.IsMatch(e, @"^[a-zA-Z0-9_.]{3,20}@gmail.com(.vn|)$");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = textBox1.Text;
            string mk = textBox2.Text;
            string email = textBox5.Text;
            string xmk = textBox3.Text;
            string hoten = textBox4.Text;
            string s = "DK-" + stt.ToString() + "-" + tentk + "-" + mk + "-" + hoten + "-" + email;
            if (!Checkaccount(tentk)) { MessageBox.Show("Vui lòng nhập tên tài khoản dài 6-24 ký tự, với các ký tự chữ và số,chữ hoa và chữ thường!"); }
            else if (!Checkaccount(mk)) { MessageBox.Show("Vui lòng nhập mật khẩu dài 6-24 ký tự, với các ký tự chữ và số,chữ hoa và chữ thường!"); }
            else if (mk != xmk) { MessageBox.Show("Vui lòng xác nhận mật khẩu chính xác!"); return; }
            else if (!checkemail(email)) { MessageBox.Show("Vui lòng nhập đúng định dang email!"); return; }
            else
                client.Send(serialize(s));

        }
        #region client
        IPEndPoint IP;
        Socket client;
        Thread listen;
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 8888);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Khong the ket noi", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listen = new Thread(Recieve);
            listen.IsBackground = true;
            listen.Start();
        }

        //
        void Recieve()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string str = (string)deserialize(data);
                    string[] chuoi = str.Split('-');
                    if (chuoi[0] == "STT")
                    {
                        stt = int.Parse(chuoi[1]);
                    }
                    else if (chuoi[0] == "5")
                    {
                        MessageBox.Show("Email này đã được đăng ký!");
                    }
                    else if (chuoi[0] == "6")
                    {
                        MessageBox.Show("Tên này đã được đăng ký!");
                    }
                    else if (chuoi[0] == "7")
                    {
                        MessageBox.Show("Tên tài khoản này đã được đăng ký!");
                    }
                    else if (chuoi[0] == "8")
                    {
                        if (MessageBox.Show("Đăng ký thành công! Bạn có muốn đăng nhập luôn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            Thread k = new Thread(() =>
                            {
                                Login f = new Login();
                                f.ShowDialog();
                            });
                            k.Start();
                            this.Close();
                        }
                    }
                    else if (chuoi[0] == "9")
                    {
                        MessageBox.Show("Tên tài khoản này đã được đăng ký, vui lòng đăng ký tên tài khoản khác!");
                    }
                }
                catch
                {
                    return;
                }
            }
        }
        byte[] serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
        }
        object deserialize(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
        #endregion

        private void Dangky_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            showPassword = !showPassword;

            // Đặt giá trị PasswordChar tùy thuộc vào trạng thái của showPassword
            textBox2.PasswordChar = showPassword ? '\0' : '*';

            // Đổi hình ảnh của PictureBox tùy thuộc vào trạng thái của showPassword
            pictureBox2.Image = showPassword ? Image.FromFile("hide.png") : Image.FromFile("show.png");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            showPassword1 = !showPassword1;

            // Đặt giá trị PasswordChar tùy thuộc vào trạng thái của showPassword
            textBox3.PasswordChar = showPassword1 ? '\0' : '*';

            // Đổi hình ảnh của PictureBox tùy thuộc vào trạng thái của showPassword
            pictureBox3.Image = showPassword1 ? Image.FromFile("hide.png") : Image.FromFile("show.png");
        }
    }
}
