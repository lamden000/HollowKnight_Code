using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Media;
using System.Runtime.InteropServices;
namespace TaiXiu
{
    public partial class Login : Form
    {
        System.Media.SoundPlayer Player = new System.Media.SoundPlayer();
      
        int stt;
        Laylaimatkhau laylaimatkhau;
        ThongtinPlayer p1;
        Dangky dangky;
        bool mo = false;
        decimal tien;
        bool musicon = false;
        public Login()
        {
            InitializeComponent();
            Connect();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                laylaimatkhau = new Laylaimatkhau();
                laylaimatkhau.ShowDialog();
            });
            this.Close();
            thread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = textBox1.Text;
            string mk = textBox2.Text;
            string s = "Login-" + stt.ToString() + "-DN-" + tentk + "-" + mk;
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
                    else if (chuoi[0] == "1") // tu 1 den 4 lien quan den form dang nhap
                    {
                        MessageBox.Show("Vui lòng nhập tên tài khoản!");
                    }
                    else if (chuoi[0] == "2")
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu!");
                    }
                    else if (chuoi[0] == "3")
                    {
                        MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mo = true;
                        tien = decimal.Parse(chuoi[1]);                       
                        this.Close();
                    }
                    else if (chuoi[0] == "4")
                    {
                        MessageBox.Show("Tên tài khoản hoặc mật khẩu không chính xác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (chuoi[0] == "5")
                    {
                        MessageBox.Show("Đã có người chơi khác sử dụng tài khoản này!");
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
        void moform(string s, decimal tien)
        {
            Thread thread = new Thread(() =>
            {

                Form1 f = new Form1(s, tien);
                f.ShowDialog();
            });

            thread.Start();
        }
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mo)
            {
                string s = textBox1.Text;

                moform(s, tien);
            }
            Player.Stop();
            client.Close();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {           
        }
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                dangky = new Dangky();
                dangky.ShowDialog();
            });
            this.Close();
            thread.Start();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Player.SoundLocation = "WWU.wav";
            Player.PlayLooping();
            musicon = true;
        }


        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            if (musicon)
            {
                Player.Stop();
                musicon = false;
                return;
            }
            else
            {
                Player.Play();
                musicon = true;
                return;
            }
        }
    }
}
