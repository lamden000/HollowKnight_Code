using System.Net.Sockets;
using System.Net;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using static System.Windows.Forms.DataFormats;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;
using System.Security.Cryptography.Pkcs;

namespace TaiXiu
{
    public partial class Form1 : Form
    {
        int stt;
        string tentkhoan;
        string kich;
        List<phong> listphong;
        int phongdavao = 0;
        Danhsachphongcho a;
        public delegate void TruyenDuLieu(List<phong> phongs);
        public TruyenDuLieu DSPhong;
        public delegate void TruyDuLieu(ThongtinPlayer p);
        public TruyDuLieu player;
        public delegate void Data(string tien, string matkhau);
        public event Data TruyenDuLieuTuForm2;
        int DangXemDSP = 0;
        ThongtinPlayer p1;
        TaoBan tb;
        decimal Tien;
        System.Media.SoundPlayer nhac = new System.Media.SoundPlayer();
        bool ismusicon = false;
        public Form1(string tentk, decimal tien)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            listphong = new List<phong>();
            tentkhoan = tentk;
            Tien = tien;
            Connect();
        }

        bool kttontai(int port)
        {
            foreach (var i in listphong)
            {
                if (i.maphong == port)
                {
                    return true;
                }
            }
            return false;
        }
        void NhanMaPhong(int maphong)
        {
            if (a.done != -1)
            {
                client.Send(serialize("Form1-Ketnoi-" + stt + "," + maphong + "," + p1.taitk + "," + p1.tien));
            }
        }
        #region button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            kich = "Form1-Choingay-" + stt.ToString() + "-" + tentkhoan + "-" + phongdavao.ToString();
            client.Send(serialize(kich));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                tb = new TaoBan(this);
                tb.ShowDialog();
            });
            thread.Start();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            kich = "Form1-Chonban-" + stt.ToString();
            client.Send(serialize(kich));
        }
        #endregion
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
                        string s = "Form1-Laythongtin-" + stt.ToString() + "-" + tentkhoan;
                        client.Send(serialize(s));
                    }
                    else if (chuoi[0] == "Taoban")
                    {
                        if (tb != null && tb.mo == true)
                        {
                            tb.Close();
                        }
                        phong a = new phong();
                        a.maphong = int.Parse(chuoi[1]);
                        a.songuoi = 1;
                        Thread thread = new Thread(() =>
                        {
                            Game f = new Game(int.Parse(chuoi[1]), p1, decimal.Parse(chuoi[2]), false);
                            f.ShowDialog();
                        });
                        thread.Start();
                        this.Close();
                    }
                    else if (chuoi[0] == "Phòng đã đầy")
                    {
                        a.phongday();
                    }
                    else if (chuoi[0] == "Chophep")
                    {
                        bool kt;
                        decimal Tien = 0;
                        if (chuoi[3] == "1")
                            kt = true;
                        else kt = false;

                        if (chuoi[2].Contains("k"))
                        {
                            Tien = decimal.Parse(chuoi[2].Split("k")[0]) * 1000;
                        }
                        else if (chuoi[2].Contains("M"))
                        {
                            Tien = decimal.Parse(chuoi[2].Split("M")[0]) * 1000000;
                        }
                        Thread thread = new Thread(() =>
                        {
                            Game f = new Game(int.Parse(chuoi[1]), p1, Tien, kt);
                            phongdavao = int.Parse(chuoi[1]);
                            f.ShowDialog();
                        });
                        thread.Start();
                        this.Close();
                    }
                    else if (chuoi[0] == "Thongtin")
                    {
                        p1 = new ThongtinPlayer(tentkhoan, chuoi[1], decimal.Parse(chuoi[2]));
                    }
                    else if (chuoi[0] == "Napxong")
                    {

                        if (int.Parse(chuoi[1]) == stt)
                        {
                            MessageBox.Show("Da duoc nap them 150");
                            pictureBox1.Image = Image.FromFile("thuongdan.png");
                            Tien += 150;
                            textBox1.Text = Tien.ToString();
                        }
                    }
                    //Mở danh sách phòng
                    else if (chuoi[0] == "modanhsach")
                    {
                        if (chuoi.Length > 2)
                        {
                            listphong.Clear();
                            for (int i = 1; i < chuoi.Count() - 1; i++)
                            {
                                string[] k = chuoi[i].Split(',');
                                phong h = new phong();
                                h.songuoi = int.Parse(k[1]);
                                h.maphong = int.Parse(k[0]);
                                h.dangchoi = int.Parse(k[2]);
                                h.tienphong = k[3];
                                h.mk = k[4];
                                h.chuphong = k[5];
                                listphong.Add(h);
                            }
                            if (DangXemDSP == 0)
                            {
                                a = new Danhsachphongcho(this);
                                this.Enabled = false;
                                a.TruyenMaPhong = new Danhsachphongcho.Data(NhanMaPhong);
                                DangXemDSP = 1;
                                //Cập nhập tình trạng phòng cho form danh sách mỗi 1 giây
                                Thread updateRoominfo = new Thread(() =>
                                {
                                    while (true)
                                    {
                                        if (a.loaded == 1)
                                        {
                                            while (a.done == 0)
                                            {
                                                client.Send(serialize("Form1" + "-Chonban-" + stt.ToString()));
                                                DSPhong(listphong);
                                                player(p1);
                                                Thread.Sleep(1000);
                                            }
                                            DangXemDSP = 0;
                                            break;
                                        }
                                    }
                                }
                                );
                                Thread openListRoom = new Thread(() =>
                                {
                                    a.ShowDialog();
                                    if (a.done == 1) 
                                    { this.Close(); }
                                    else
                                    { this.Enabled = true; }
                                });
                                updateRoominfo.Start();
                                openListRoom.Start();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Chưa có người chơi nào tạo bàn, hãy ấn 'Tạo bàn' hoặc 'Chơi ngay' nếu bạn muốn chơi");
                        }
                    }
                    else if (chuoi[0] == "Không đủ tiền")
                    {
                        tb.TruyenDuLieu(chuoi[0]);
                    }
                    else if (chuoi[0] == "Không đủ tiền!")
                    {
                        MessageBox.Show(chuoi[0]);
                    }
                    //
                    else if (chuoi[0] == "choingay")
                    {
                        bool kt = false;
                        if (chuoi[3] == "1")
                            kt = true;
                        Thread thread = new Thread(() =>
                        {
                            Game f = new Game(int.Parse(chuoi[1]), p1, decimal.Parse(chuoi[2]), kt);
                            f.ShowDialog();
                        });
                        thread.Start();
                        this.Close();
                        phongdavao = int.Parse(chuoi[1]);
                    }
                    else if (chuoi[0] == "Msg")
                    {
                        richTextBox1.Focus();
                        richTextBox1.Text += tentkhoan + ": " + chuoi[1] + "\n";
                    }


                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        public void UpdateDataFromForm2(int dataFromForm2)
        {
            phongdavao = dataFromForm2;
            string s = stt.ToString() + "," + phongdavao.ToString() + "-Ketnoi";
            client.Send(serialize(s));
        }
        public void XuLySukienTruyenDuLieuTuForm2(string tien, string matkhau)
        {
            string s = "Form1-Taoban-" + p1.taitk + "-" + matkhau + "," + tien + "-" + stt.ToString();
            client.Send(serialize(s));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Thread f = new Thread(() =>
                {
                    Login dn = new Login();
                    dn.ShowDialog();
                });
                this.Close();

            }
        }
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            // Lấy đối tượng Graphics để vẽ trên PictureBox
            Graphics g = e.Graphics;

            // Vẽ hình tròn màu đỏ
            Pen redPen = new Pen(Color.Red, 3);
            g.DrawEllipse(redPen, 50, 50, 200, 200);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Visible = false;
            textBox1.Text = "Tien: " + Tien.ToString();
            textBox3.Text = "Welcome " + tentkhoan;
            label1.Text = textBox3.Text;
            textBox3.Size = label1.Size;
            nhac.SoundLocation = "MU.wav";
            nhac.PlayLooping();
            ismusicon = true;
            if (Tien <= 65)
            {
                pictureBox1.Image = Image.FromFile("anxin.png");
                string k = "Naptien-" + stt.ToString() + "-" + tentkhoan;
                client.Send(serialize(k));
            }
            if (Tien > 65 && Tien <= 500)
            {
                pictureBox1.Image = Image.FromFile("thuongdan.png");


            }
            if (Tien > 500 && Tien <= 2000)
            {
                pictureBox1.Image = Image.FromFile("doanhnhan.png");
            }
            if (Tien > 2000 && Tien <= 5000)
            {
                pictureBox1.Image = Image.FromFile("nguoigiau.png");
            }
            if (Tien > 5000)
            {
                pictureBox1.Image = Image.FromFile("typhu.png");
            }
        }
        private void btnsend_Click(object sender, EventArgs e)
        {
            string a = "Msg-" + textBox2.Text;
            client.Send(serialize(a));
            textBox2.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;

            richTextBox1.ScrollToCaret();
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            richTextBox1.Visible = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Size size = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
            textBox1.Width = size.Width;
            textBox1.Height = size.Height;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (ismusicon)
            {
                nhac.Stop();
                ismusicon = false;
                return;
            }
            else
            {
                nhac.Play();
                ismusicon = true;
                return;
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            nhac.Stop();
        }
    }
}