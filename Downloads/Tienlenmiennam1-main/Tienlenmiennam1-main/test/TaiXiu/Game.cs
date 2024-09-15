using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;
using TaiXiu.Properties;

namespace TaiXiu
{
    public partial class Game : Form
    {
        #region sound effect
        System.Media.SoundPlayer tickef = new System.Media.SoundPlayer();
        System.Media.SoundPlayer Soundef = new System.Media.SoundPlayer();
        System.Media.SoundPlayer playcard= new System.Media.SoundPlayer();
        System.Media.SoundPlayer winsound = new System.Media.SoundPlayer();
        System.Media.SoundPlayer losesound = new System.Media.SoundPlayer();
        #endregion
        List<int> doithu;
        public Game(int port = 0, ThongtinPlayer p1 = null, decimal tienban = 5, bool dgchoi = false)
        {
            InitializeComponent();
            this.port = port;
            this.tienban = tienban;
            Player = p1;
            label2.Text = this.port.ToString();
            CheckForIllegalCrossThreadCalls = false;
            if (dgchoi)
            {
                phongdangchoi = true;
            }
            nhanduoc = new int[4];
            for (int i = 0; i < 4; i++)
                nhanduoc[i] = 0;
            doithu = new List<int>();
            Connect();

        }


        int getport()
        {
            return port;
        }
        int count = 1;
        ThongtinPlayer Player;
        int[] nhanduoc;
        private decimal tienban;
        private int port;
        bool phongdangchoi = false;
        int stt;
        class Card
        {
            int value;
            int typeid;
            Image image;

            public int Value { get => value; set => this.value = value; }
            public int Typeid { get => typeid; set => typeid = value; }

            public Card(int V, int T, Image I)
            {
                Value = V;
                Typeid = T;
                image = I;
            }
            public Card()
            {
                Value = 0;
                Typeid = 0;
                image = Image.FromFile("b1fv.png");
            }
            public Image getimg()
            {
                return image;
            }
            public int getValue() { return Value; }
            public int getTypeid() { return Typeid; }
        }

        #region 52 cards definition
        Card c2C = new Card(16, 2, Image.FromFile("2C.png"));
        Card c2S = new Card(16, 1, Image.FromFile("2S.png"));
        Card c2D = new Card(16, 3, Image.FromFile("2D.png"));
        Card c2H = new Card(16, 4, Image.FromFile("2H.png"));
        Card c3C = new Card(3, 2, Image.FromFile("3C.png"));
        Card c3S = new Card(3, 1, Image.FromFile("3S.png"));
        Card c3D = new Card(3, 3, Image.FromFile("3D.png"));
        Card c3H = new Card(3, 4, Image.FromFile("3H.png"));
        Card c4C = new Card(4, 2, Image.FromFile("4C.png"));
        Card c4S = new Card(4, 1, Image.FromFile("4S.png"));
        Card c4D = new Card(4, 3, Image.FromFile("4D.png"));
        Card c4H = new Card(4, 4, Image.FromFile("4H.png"));
        Card c5C = new Card(5, 2, Image.FromFile("5C.png"));
        Card c5S = new Card(5, 1, Image.FromFile("5S.png"));
        Card c5D = new Card(5, 3, Image.FromFile("5D.png"));
        Card c5H = new Card(5, 4, Image.FromFile("5H.png"));
        Card c6C = new Card(6, 2, Image.FromFile("6C.png"));
        Card c6S = new Card(6, 1, Image.FromFile("6S.png"));
        Card c6D = new Card(6, 3, Image.FromFile("6D.png"));
        Card c6H = new Card(6, 4, Image.FromFile("6H.png"));
        Card c7C = new Card(7, 2, Image.FromFile("7C.png"));
        Card c7S = new Card(7, 1, Image.FromFile("7S.png"));
        Card c7D = new Card(7, 3, Image.FromFile("7D.png"));
        Card c7H = new Card(7, 4, Image.FromFile("7H.png"));
        Card c8C = new Card(8, 2, Image.FromFile("8C.png"));
        Card c8S = new Card(8, 1, Image.FromFile("8S.png"));
        Card c8D = new Card(8, 3, Image.FromFile("8D.png"));
        Card c8H = new Card(8, 4, Image.FromFile("8H.png"));
        Card c9C = new Card(9, 2, Image.FromFile("9C.png"));
        Card c9S = new Card(9, 1, Image.FromFile("9S.png"));
        Card c9D = new Card(9, 3, Image.FromFile("9D.png"));
        Card c9H = new Card(9, 4, Image.FromFile("9H.png"));
        Card c10C = new Card(10, 2, Image.FromFile("10C.png"));
        Card c10S = new Card(10, 1, Image.FromFile("10S.png"));
        Card c10D = new Card(10, 3, Image.FromFile("10D.png"));
        Card c10H = new Card(10, 4, Image.FromFile("10H.png"));
        Card cJC = new Card(11, 2, Image.FromFile("JC.png"));
        Card cJS = new Card(11, 1, Image.FromFile("JS.png"));
        Card cJD = new Card(11, 3, Image.FromFile("JD.png"));
        Card cJH = new Card(11, 4, Image.FromFile("JH.png"));
        Card cQC = new Card(12, 2, Image.FromFile("QC.png"));
        Card cQS = new Card(12, 1, Image.FromFile("QS.png"));
        Card cQD = new Card(12, 3, Image.FromFile("QD.png"));
        Card cQH = new Card(12, 4, Image.FromFile("QH.png"));
        Card cKC = new Card(13, 2, Image.FromFile("KC.png"));
        Card cKS = new Card(13, 1, Image.FromFile("KS.png"));
        Card cKD = new Card(13, 3, Image.FromFile("KD.png"));
        Card cKH = new Card(13, 4, Image.FromFile("KH.png"));
        Card cAC = new Card(14, 2, Image.FromFile("AC.png"));
        Card cAS = new Card(14, 1, Image.FromFile("AS.png"));
        Card cAD = new Card(14, 3, Image.FromFile("AD.png"));
        Card cAH = new Card(14, 4, Image.FromFile("AH.png"));
        #endregion
        #region global variable
        int turndau = 1;
        bool dangchoi = false;
        int[] boturn = new int[4];
        int[] isSelected = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Card[] player = new Card[13];//bộ bài của người chơi
        int playern = new int();//số bài còn lại trên tay
        int selectedn = new int();//số bài đã chọn
        int skipped = 0;//Biến để biết người chơi đã bỏ lượt chưa(trong 1 round)
        int time = 0;//Thời gian đợi đánh bài
        int timepicture = 5;// Thời gian xuất hiện ảnh
        CheckBox[] checkBoxes = new CheckBox[4];//check box biểu thị lượt
        bool anhdahien = false;
        //Bài trên bàn
        string Ttype; //Loại bài
        int Tvalue0;// Giá trị của lá bài lớn nhất
        int Tvalue1;// Kiểu của lá bài lớn nhất
        int cardsn;// Số bài
        int turn = 1;//lượt đi hiện tại
        int yourturn = 10;//lượt của mình
        bool bichat = false;
        bool isFirstTurn = false;
        #endregion
        #region My functions
        //Hàm chọn bài
        void Select(Card[] Cards, ref PictureBox a, int boxi)
        {
            if (isSelected[boxi - 1] == 0)
            {
                Soundef.Play();
                isSelected[boxi - 1] = 1;
                selectedn++;
                a.Top -= 10;
            }
            else if (isSelected[boxi - 1] == 1)
            {
                Soundef.Play();
                isSelected[boxi - 1] = 0;
                selectedn--;
                a.Top += 10;
            }
        }

        //Kiểm tra loại bài được chọn
        string KTLoaiBai(Card[] Player)
        {
            if (selectedn == 1)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (isSelected[i] == 1)
                    {
                        if (player[i].getValue() == 16 && ((player[i].getTypeid() == 1) || (player[i].getTypeid() == 2)))
                        {
                            return "2den";
                        }
                        else if (player[i].getValue() == 16 && ((player[i].getTypeid() == 3) || (player[i].getTypeid() == 4)))
                        {
                            return "2do";
                        }
                    }
                }
                return "Le";
            }
            int j = 0;
            int[] value = new int[selectedn];
            int[] loai = new int[selectedn];
            int temp;
            for (int i = 0; i < 13; i++)
            {
                if (isSelected[i] == 1)
                {
                    value[j] = player[i].getValue();
                    loai[j] = player[i].getTypeid();
                    j++;
                }
            }
            int equal = 1;
            for (int i = 0; i < selectedn - 1; i++)
            {
                if (value[i] != value[i + 1])
                {
                    equal = 0;
                }
            }
            if (equal == 1)
            {
                if (selectedn == 3)
                    return "Tam";
                if (selectedn == 4)
                    return "Tu Quy";
                if (selectedn == 2)
                {
                    if (value[0] == 16 && ((loai[0] == 1 && loai[1] == 2) || (loai[0] == 2 && loai[1] == 1)))
                    {
                        return "doi21";
                    }
                    else if (value[0] == 16 && ((loai[0] == 3 && loai[1] == 4) || (loai[0] == 4 && loai[1] == 3)))
                    {
                        return "doi23";
                    }
                    else if (value[0] == 16 && ((loai[0] == 1 && loai[1] == 4) || (loai[0] == 4 && loai[1] == 1) || (loai[0] == 1 && loai[1] == 3) || (loai[0] == 3 && loai[1] == 1)))
                    {
                        return "doi22";
                    }
                    else if (value[0] == 16 && ((loai[0] == 2 && loai[1] == 4) || (loai[0] == 4 && loai[1] == 2) || (loai[0] == 2 && loai[1] == 3) || (loai[0] == 3 && loai[1] == 2)))
                    {
                        return "doi22";
                    }
                    return "Doi";
                }
            }

            if (selectedn > 2)
            {
                int laSanh = 1;
                for (int i = 0; i < selectedn - 1; i++)
                {
                    if (value[i] != value[i + 1] - 1)
                        laSanh = 0;
                }
                if (laSanh == 1)
                {
                    return "Sanh";
                }
            }

            if (selectedn > 5)
            {
                int isdoithong = 0;
                for (int i = 0; i < selectedn - 1; i++)
                {
                    if (value[i] == value[i + 1])
                    {
                        isdoithong = 1;
                        if (i < selectedn - 2)
                        {
                            if (value[i] + 1 != value[i + 2])
                            {
                                isdoithong = 0;
                                break;
                            }
                        }
                        i++;
                    }
                    else
                    {
                        isdoithong = 0;
                        break;
                    }
                }
                if (isdoithong == 1)
                {
                    if (selectedn == 6)
                        return "Doi Thong_3";
                    else if (selectedn == 8)
                        return "Doi Thong_4";
                    else if (selectedn == 10)
                        return "Doi Thong_5";
                }
            }

            return "Khong Danh Duoc";
        }

        //Kiểm bài đã chọn có phù hợp để được đánh không
        int Chophepdanh(string s)
        {
            if (s == "Ăn Lăng")
            {
                return 1;
            }
            if (skipped == 1)
                if (KTLoaiBai(player) != "Doi Thong_4" && KTLoaiBai(player) != "Tu Quy")
                {
                    MessageBox.Show("Bạn đã bỏ lượt trong round này, bạn chỉ được chặt -HEO,ĐÔI THÔNG, TỨ QUÝ- bằng -TỨ QUÝ HOẶC 4 ĐÔI THÔNG-", "Đã bỏ lượt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
            if (turn != yourturn)
                return 0;
            if (isFirstTurn && isSelected[0] == 0)
            {
                MessageBox.Show("LƯỢT ĐẦU: Bạn phải đánh con bài nhỏ nhất");
                return 0;
            }
            if (selectedn == 0)
                return 0;
            if (KTLoaiBai(player).CompareTo("Khong Danh Duoc") == 0)
                return 0;
            if (Ttype == "0")
                return 1;
            int[] values = new int[selectedn];
            int[] types = new int[selectedn];
            int j = 0;
            for (int i = 0; i < 13; i++)
            {
                if (isSelected[i] == 1)
                {
                    values[j] = player[i].getValue();
                    types[j] = player[i].getTypeid();
                    j++;
                }
            }
            j--;
            //Tu quy
            if (KTLoaiBai(player).CompareTo("Tu Quy") == 0)
            {
                var validTypes = new HashSet<string> { "2do", "2den", "doi23", "doi21", "doi22", "Doi Thong_3", "Tu Quy" };

                if (validTypes.Contains(Ttype) || (values[0] > Tvalue0 && Ttype == "Tu Quy"))
                {
                    return 1;
                }
            }
            //doi thong
            if (KTLoaiBai(player).Contains("Doi Thong"))
            {
                if (types[j] < types[j - 1])
                    types[j] = types[j - 1];

                if (Ttype == "2do" || Ttype == "2den" || (Ttype == "doi23" && selectedn == 8) || (Ttype == "doi21" && selectedn == 8) || (Ttype == "doi22" && selectedn == 8))
                    return 1;

                if (Ttype == "Tu quy" && selectedn > 7)
                    return 1;

                if (Ttype.Contains("Doi Thong"))
                {
                    if (selectedn < cardsn || (selectedn > cardsn) || (Tvalue0 == values[j] && Tvalue1 < types[j]) || (Tvalue0 < values[j]))
                        return 1;
                }

                return 0;
            }
            //          
            if (cardsn != selectedn)
                return 0;
            // Le          
            if (Ttype.CompareTo("Le") == 0 || Ttype.CompareTo("2den") == 0 || Ttype.CompareTo("2do") == 0)
            {
                if (types[0] > Tvalue1 && values[0] == Tvalue0)
                    return 1;
                if (Tvalue0 < values[0])
                    return 1;
                return 0;
            }
            // Doi
            if (Ttype.CompareTo("Doi") == 0 || Ttype.CompareTo("doi21") == 0 || Ttype.CompareTo("doi22") == 0 || Ttype.CompareTo("doi23") == 0)
            {
                if (types[0] < types[1])
                    types[0] = types[1];
                if (types[0] > Tvalue1 && values[0] == Tvalue0)
                    return 1;
                if (Tvalue0 < values[0])
                    return 1;
                return 0;
            }
            //Tam
            if (Ttype.CompareTo("Tam") == 0)
            {
                if (values[0] > Tvalue0)
                    return 1;
                return 0;
            }
            // Sanh
            if (Ttype.CompareTo("Sanh") == 0)
            {
                if (values[j] > Tvalue0)
                    return 1;
                else if (values[j] == Tvalue0 && types[j] > Tvalue1)
                    return 1;
                return 0;
            }
            return 0;
        }

        //Hàm xếp bài
        void xepBai(Card[] Player)
        {
            Card temp = new Card();
            for (int i = 0; i < 13 - 1; i++)
            {
                for (int j = i + 1; j < 13; j++)
                    if (Player[i].getValue() > player[j].getValue())
                    {
                        temp = player[i];
                        Player[i] = Player[j];
                        Player[j] = temp;
                    }
                    else if (Player[i].getValue() == player[j].getValue())
                    {
                        if (player[i].getTypeid() > player[j].getTypeid())
                        {
                            temp = player[i];
                            Player[i] = Player[j];
                            Player[j] = temp;
                        }
                    }
            }

        }

        //Hàm bỏ lượt
        void BoLuot()
        {
            if (turn == yourturn && Ttype.CompareTo("0") != 0 && dangchoi)
            {
                string str = "Skip-" + skipped.ToString();
                skipped = 1;
                client.Send(serialize(str));
                turn_1.Hide();
            }
        }
        //Hàm đánh bài
        void DanhBai(string s)
        {
            if (Chophepdanh(s) == 1)
            {
                string str = string.Empty;
                PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13 };
                str = "ThongTinBai-" + stt.ToString() + "-" + KTLoaiBai(player) + "-";
                for (int i = 0; i < 13; i++)
                {
                    if (isSelected[i] == 1)
                        pictureBoxes[i].Hide();
                    if (isSelected[i] == 1)
                    {
                        isSelected[i] = -1;
                        str = str + player[i].getValue().ToString() + "_" + player[i].getTypeid().ToString() + ",";
                    }
                }
                playern -= selectedn;
                playcard.SoundLocation = "playcard.wav";
                playcard.Play();
                turn_1.Hide();
                selectedn = 0;
                client.Send(serialize(str));
                Thread thread = new Thread(() =>
                {
                    Thread.Sleep(500);
                    str = "Win-" + stt + "-" + yourturn;
                    client.Send(serialize(str));
                });
                if (playern == 0 && s != "Ăn Lăng")
                {
                    thread.Start();
                }
            }
            else
            {
                return;
            }
        }

        #endregion
        #region event handle

        private void Game_Load(object sender, EventArgs e)
        {
            win1.Hide();
            win2.Hide();
            win3.Hide();
            win4.Hide();
            pictureBox18.Hide();
            Soundef.SoundLocation = "clickef.wav";
            label5.Text = Player.dangtien;
            label9.Text = Player.hoten;
            string tiendung = Player.dangtien.Remove(Player.dangtien.Length - 1, 1);
            setava14(tiendung);
            label10.Hide();
            label11.Hide();
            label12.Hide();
            label6.Hide();
            label7.Hide();
            label8.Hide();
            pictureBox17.Hide();
            pictureBox16.Hide();
            pictureBox15.Hide();
            player4.Image = Image.FromFile("b1fv.png");
            player3.Image = Image.FromFile("b1fv.png");
            player2.Image = Image.FromFile("b1fv.png");
            button1.Hide();
            turn_3.Hide();
            turn_4.Hide();
            turn_2.Hide();
            turn_1.Hide();
            label1.Hide();
            button4.Hide();
            PictureBox[] pictureBoxes = { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13
                                        ,pictureBox1,pictureBox2,pictureBox3,pictureBox4,pictureBox5, pictureBox6, pictureBox7,
                                        pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13 ,
                                        pictureBox16,pictureBox17,pictureBox15,
                                        player4,player3,player2};
            for (int i = 0; i < 32; i++)
                pictureBoxes[i].Hide();
            timer1.Stop();
            timer.Hide();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox1, 1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox4, 4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox5, 5);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox13, 13);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox2, 2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox3, 3);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox6, 6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox7, 7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox8, 8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox9, 9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox10, 10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox11, 11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox12, 12);
        }


        //
        // Nút Đánh
        private void button1_Click(object sender, EventArgs e)
        {
            DanhBai(string.Empty);


        }

        //Nút sẵng sàng
        private void button2_Click(object sender, EventArgs e)
        {
            client.Send(serialize("Ready"));
            button2.Hide();
        }
        //Nút menu
        private void button3_Click(object sender, EventArgs e)
        {
            if (!dangchoi)
            {
                client.Send(serialize("Thoat-" + stt));
                Thread.Sleep(1000);
                client.Close();
                Thread thread = new Thread(() =>
                {
                    Form1 f = new Form1(Player.taitk, Player.tien);
                    // Hiển thị Form1
                    f.ShowDialog();
                });
                this.Close();
                thread.Start();
            }
            else
            {
                MessageBox.Show("Đang chơi không thể về menu!");

            }



        }
        //Nút bỏ lượt
        private void button4_Click(object sender, EventArgs e)
        {
            BoLuot();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            time--;

            timer.Text = "00:" + time.ToString();
            if (time == 10)
            {
                tickef.SoundLocation = "ticking10s.wav";
                tickef.Play();
            }
            if (time == 0)
            {
                tickef.Stop();
                if (turn == yourturn)
                {
                    if (Ttype.CompareTo("0") != 0)
                    {
                        BoLuot();
                        return;
                    }
                    else
                    {
                        PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13 };
                        for (int i = 0; i < 13; i++)
                        {
                            if (isSelected[i] == 1)
                            {
                                Select(player, ref pictureBoxes[i], i + 1);
                            }

                        }
                        for (int i = 0; i < 13; i++)
                        {
                            if (isSelected[i] == 0)
                            {
                                Select(player, ref pictureBoxes[i], i + 1);
                                break;
                            }
                        }
                        DanhBai(string.Empty);
                        return;
                    }
                }
                Invoke((MethodInvoker)delegate { timer1.Stop(); });
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string a = "Msg-" + textBox1.Text;
            client.Send(serialize(a));
            textBox1.Clear();
        }
        #endregion
        #region client
        IPEndPoint IP;
        Socket client;
        Thread listen;
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
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
        void Recieve()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string str = (string)deserialize(data);
                    string[] strings = str.Split('-');
                    Card[] deck = { c3S, c3C, c3D, c3H, c4S, c4C, c4D, c4H, c5S, c5C, c5D, c5H, c6S, c6C, c6D, c6H, c7S, c7C, c7D, c7H, c8S, c8C, c8D, c8H, c9S, c9C, c9D, c9H, c10S, c10C, c10D, c10H, cJS, cJC, cJD, cJH, cQS, cQC, cQD, cQH, cKS, cKC, cKD, cKH, cAS, cAC, cAD, cAH, c2S, c2C, c2D, c2H };
                    //                  
                    if (strings[0].CompareTo("STT") == 0)
                    {
                        stt = int.Parse(strings[1]);
                        doithu.Add(stt);
                        count = int.Parse(strings[2]);
                        numberplayer.Text = "Số người chơi:" + strings[2];
                        client.Send(serialize("Capnhat-Tatca-" + stt + "-" + Player.hoten + "," + Player.tien + "," + Player.taitk + "-" + strings[2]));
                        #region capnhat
                        for (int i = 5; i < strings.Length; i++)
                        {

                            string[] tt = strings[i].Split(',');
                            doithu.Add(int.Parse(tt[2]));
                            string tiendung = tt[1].Remove(tt[1].Length - 1, 1);
                            if (tt[3] == "Ko")
                                switch (stt)
                                {
                                    case 1:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 2:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();

                                                break;
                                            case 3:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                            case 4:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                        }
                                        break;
                                    case 2:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 1:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                            case 3:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();
                                                break;
                                            case 4:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                        }
                                        break;
                                    case 3:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 1:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                            case 2:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                            case 4:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();
                                                break;
                                        }
                                        break;
                                    case 4:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 1:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();
                                                break;
                                            case 2:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                            case 3:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                        }
                                        break;
                                }
                            else
                            {
                                switch (stt)
                                {
                                    case 1:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 2:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                player2.Show();
                                                turn_4.Show();
                                                label6.Show();
                                                label10.Show();

                                                pictureBox15.Show();

                                                break;
                                            case 3:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                player3.Show();
                                                turn_3.Show();
                                                label7.Show();
                                                label11.Show();

                                                pictureBox16.Show();
                                                break;
                                            case 4:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                player4.Show();
                                                turn_2.Show();
                                                label8.Show();
                                                label12.Show();

                                                pictureBox17.Show();
                                                break;
                                        }
                                        break;
                                    case 2:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 1:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                player4.Show();
                                                turn_2.Show();
                                                label8.Show();
                                                label12.Show();

                                                pictureBox17.Show();
                                                break;
                                            case 3:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                player4.Show();
                                                turn_4.Show();
                                                label6.Show();
                                                label10.Show();

                                                pictureBox15.Show();
                                                break;
                                            case 4:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                player2.Show();
                                                turn_3.Show();
                                                label7.Show();
                                                label11.Show();

                                                pictureBox16.Show();
                                                break;
                                        }
                                        break;
                                    case 3:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 1:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                player3.Show();
                                                turn_3.Show();
                                                label7.Show();
                                                label11.Show();

                                                pictureBox16.Show();
                                                break;
                                            case 2:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                player4.Show();
                                                turn_2.Show();
                                                label8.Show();
                                                label12.Show();

                                                pictureBox17.Show();
                                                break;
                                            case 4:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                player2.Show();
                                                turn_4.Show();
                                                label6.Show();
                                                label10.Show();

                                                pictureBox15.Show();
                                                break;
                                        }
                                        break;
                                    case 4:
                                        switch (int.Parse(tt[2]))
                                        {
                                            case 1:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                player2.Show();
                                                turn_4.Show();
                                                label6.Show();
                                                label10.Show();

                                                pictureBox15.Show();
                                                break;
                                            case 2:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                player3.Show();
                                                turn_3.Show();
                                                label7.Show();
                                                label11.Show();

                                                pictureBox16.Show();
                                                break;
                                            case 3:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                player4.Show();
                                                turn_2.Show();
                                                label8.Show();
                                                label12.Show();

                                                pictureBox17.Show();
                                                break;
                                        }
                                        break;
                                }
                            }
                            #endregion
                        }
                    }
                    else if (strings[0].CompareTo("Khongduocsansang") == 0)
                    {
                        MessageBox.Show("Không thể sẵn sàng khi phòng đang chơi!");
                        button2.Show();
                    }
                    else if (strings[0].CompareTo("Capnhat") == 0)
                    {

                        if (stt != int.Parse(strings[2]))
                        {
                            if (strings[1].CompareTo("Tatca") == 0)
                            {
                                string[] tt = strings[3].Split(',');

                                doithu.Add(int.Parse(strings[2]));
                                numberplayer.Text = "Số người chơi:" + strings[4];
                                string tiendung = tt[1].Remove(tt[1].Length - 1, 1);
                                switch (stt)
                                {
                                    case 1:
                                        switch (int.Parse(strings[2]))
                                        {
                                            case 2:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();



                                                setava15(tiendung);

                                                pictureBox15.Show();

                                                break;
                                            case 3:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();

                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                            case 4:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                        }
                                        break;
                                    case 2:
                                        switch (int.Parse(strings[2]))
                                        {
                                            case 1:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                            case 3:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();
                                                break;
                                            case 4:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                        }
                                        break;
                                    case 3:
                                        switch (int.Parse(strings[2]))
                                        {
                                            case 1:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                            case 2:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                            case 4:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();
                                                break;
                                        }
                                        break;
                                    case 4:
                                        switch (int.Parse(strings[2]))
                                        {
                                            case 1:
                                                label6.Text = tt[0];
                                                label10.Text = tt[1];
                                                label6.Show();
                                                label10.Show();
                                                setava15(tiendung);
                                                pictureBox15.Show();
                                                break;
                                            case 2:
                                                label7.Text = tt[0];
                                                label11.Text = tt[1];
                                                label7.Show();
                                                label11.Show();
                                                setava16(tiendung);
                                                pictureBox16.Show();
                                                break;
                                            case 3:
                                                label8.Text = tt[0];
                                                label12.Text = tt[1];
                                                label8.Show();
                                                label12.Show();
                                                setava17(tiendung);
                                                pictureBox17.Show();
                                                break;
                                        }
                                        break;
                                }
                            }
                            else if (strings[1].CompareTo("Guilai") == 0)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    string[] tach = strings[i + 2].Split(',');
                                    xulytien(tach[0], tach[1], tach[2]);
                                }
                            }
                        }

                    }
                    //Server phát bài
                    else if (strings[0].CompareTo("PhatBai") == 0)
                    {

                        time = 31;
                        dangchoi = true;
                        for (int i = 0; i < 4; i++)
                        {
                            boturn[i] = 0;
                        }
                        playern = 13;
                        PictureBox[] pictureBoxes2 = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, player4, player3, player2 };
                        PictureBox[] pictureBoxes = { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13 };
                        string[] strings2 = strings[1].Split(",");
                        //
                        for (int i = 0; i < 13; i++)
                        {
                            isSelected[i] = 0;
                            pictureBoxes[i].Hide();
                        }
                        //
                        for (int i = 0; i < 13; i++)
                        {
                            player[i] = deck[int.Parse(strings2[i])];
                        }
                        xepBai(player);
                        //
                        int x = 214;
                        for (int i = 0; i < 13; i++)
                        {
                            pictureBoxes2[i].Image = player[i].getimg();
                            pictureBoxes2[i].Location = new Point(x, 345);
                            x += 24;
                            pictureBoxes2[i].Show();
                        }
                        yourturn = int.Parse(strings[3]);
                        Ttype = strings[4];
                        button1.Show();
                        label1.Text = yourturn.ToString();
                        label1.Show();
                        button4.Show();
                        isFirstTurn = true;
                        int Np = int.Parse(strings[2]);//số người chơi                       
                        PictureBox[] pictures = { player2, player4, player3 }; //picture box tượng trung cho người chơi khác 
                        //checkbox hiển thị lượt
                        switch (yourturn)
                        {
                            case 1:
                                {
                                    checkBoxes = new[] { turn_1, turn_2, turn_3, turn_4 };
                                    pictures = new[] { player2, player3, player4 };
                                    break;
                                }
                            case 2:
                                {
                                    checkBoxes = new[] { turn_4, turn_1, turn_2, turn_3 };
                                    pictures = new[] { player4, player2, player3 };
                                    break;
                                }
                            case 3:
                                {
                                    checkBoxes = new[] { turn_3, turn_4, turn_1, turn_2 };
                                    pictures = new[] { player3, player4, player2 };
                                    break;
                                }
                            case 4:
                                {
                                    checkBoxes = new[] { turn_2, turn_3, turn_4, turn_1 };
                                    break;
                                }
                        }
                        for (int i = 0; i < Np - 1; i++)
                        {
                            pictures[i].Show();
                        }
                    }
                    // Sever gửi thông tin bài vừa được đánh ra
                    else if (strings[0].CompareTo("ThongTinBai") == 0)
                    {
                        string[] ThongTinBai = strings[3].Split(',');
                        string[] LaBai = new string[2];
                        Ttype = strings[2];
                        turn = int.Parse(strings.Last());
                        cardsn = ThongTinBai.Length - 1;
                        int[] values = new int[cardsn];
                        int[] types = new int[cardsn];
                        isFirstTurn = false;
                        PictureBox[] pictureBoxes = { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13 };
                        for (int i = 0; i < 13; i++)
                        {
                            pictureBoxes[i].Hide();
                        }
                        for (int i = 0; i < cardsn; i++)
                        {
                            LaBai = ThongTinBai[i].Split("_");
                            values[i] = int.Parse(LaBai[0]);
                            types[i] = int.Parse(LaBai[1]);
                        }
                        // Lấy ảnh
                        int a;
                        for (int i = 0; i < cardsn; i++)
                        {
                            if (values[i] != 16)
                            {
                                a = (values[i] - 3) * 4 + types[i] - 1;
                                pictureBoxes[i].Image = deck[a].getimg();
                            }
                            else
                            {
                                a = (15 - 3) * 4 + types[i] - 1;
                                pictureBoxes[i].Image = deck[a].getimg();
                            }
                            pictureBoxes[i].Show();
                        }
                        //lấy value0 va value1
                        Tvalue0 = values[cardsn - 1];
                        if (Ttype == "Le" || Ttype == "2den" || Ttype == "2do")
                        {
                            Tvalue1 = types[0];
                        }
                        else if (Ttype == "Doi" || Ttype.Contains("doi"))
                        {
                            if (types[1] > types[0])
                                Tvalue1 = types[1];
                            else Tvalue1 = types[0];

                        }
                        else if (Ttype.Contains("Doi Thong"))
                        {
                            if (types[cardsn - 1] > types[cardsn - 2])
                                Tvalue1 = types[cardsn - 1];
                            else Tvalue1 = types[cardsn - 2];
                            anhdahien = true;
                            pictureBox18.Image = Image.FromFile("OMG.gif");
                            pictureBox18.Show();
                            Invoke((MethodInvoker)delegate { timer2.Start(); });

                        }
                        else if (Ttype == "Sanh")
                        {
                            Tvalue1 = types[cardsn - 1];

                        }
                        else if (Ttype == "Tu Quy")
                        {
                            anhdahien = true;
                            pictureBox18.Image = Image.FromFile("OMG.gif");
                            pictureBox18.Show();
                            Invoke((MethodInvoker)delegate { timer2.Start(); });
                        }
                        time = 31;
                        if (Ttype != "Khong Danh Duoc")
                        {
                            Invoke((MethodInvoker)delegate { timer1.Start(); });
                            tickef.Stop();
                        }
                    }
                    else if (strings[0].CompareTo("Trutien") == 0)
                    {
                        if (Player.Tien - decimal.Parse(strings[1]) >= 0)
                            Player.capnhat(Player.Tien - decimal.Parse(strings[1]));
                        else Player.capnhat(0);
                        client.Send(serialize("Capnhat-" + yourturn + "-" + Player.Tien));
                    }
                    else if (strings[0].CompareTo("Congtien") == 0)
                    {
                        Player.capnhat(Player.Tien + decimal.Parse(strings[1]) * 0.9m);
                        client.Send(serialize("Capnhat-" + yourturn + "-" + Player.Tien));
                    }
                    else if (strings[0] == "Thoat")
                    {
                        numberplayer.Text = "Số người chơi:" + strings[2];
                        Thread thread = new Thread(() => { xulythoat(strings[1]); });
                        thread.Start();
                    }
                    else if (strings[0] == "Tien")
                    {

                    }
                    //
                    //Game kết thúc
                    else if (strings[0].CompareTo("GameEnd") == 0)
                    {
                        Invoke((MethodInvoker)delegate { timer1.Stop(); });
                        this.Enabled = false;
                        Thread Endgame = new Thread(() =>
                        {
                            int choi = int.Parse(strings[3]);
                            if (strings[strings.Length - 2] == "Ăn Lăng")
                            {
                                for (int i = choi + 5; i < strings.Length - 2; i++)
                                    xulythoat(strings[i]);
                                if (int.Parse(strings[2]) == yourturn)
                                {
                                    selectedn = 13;
                                    for (int i = 0; i < 13; i++)
                                    {
                                        isSelected[i] = 1;
                                    }
                                    DanhBai("Ăn Lăng");
                                }
                                Image currentBackgroundImage = this.BackgroundImage;
                                this.BackgroundImage = Properties.Resources.thantai;
                                MessageBox.Show(strings[1], "Ăn Lăng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.BackgroundImage = currentBackgroundImage;
                                winsound.SoundLocation = "win.wav";
                                winsound.Play();
                            }
                            else
                            {
                                for (int i = choi + 5; i < strings.Length; i++)
                                    xulythoat(strings[i]);

                                if (int.Parse(strings[2]) == yourturn)
                                {
                                    winsound.SoundLocation = "win.wav";
                                    winsound.Play();
                                   
                                    anhdahien = true;
                                    win1.Image = Image.FromFile("win.gif");
                                    win1.Show();
                                    Invoke((MethodInvoker)delegate { timer2.Start(); });

                                }
                                else
                                {
                                    losesound.SoundLocation = "lose.wav";
                                    losesound.Play();
                                    anhdahien = true;
                                    Invoke((MethodInvoker)delegate { timer2.Start(); });
                                    switch (yourturn)
                                    {
                                        case 1:
                                            switch (int.Parse(strings[2]))
                                            {
                                                case 2:
                                                    setwin2();
                                                    break;
                                                case 3:
                                                    setwin3();
                                                    break;
                                                case 4:
                                                    setwin4();
                                                    break;

                                            }
                                            break;
                                        case 2:
                                            switch (int.Parse(strings[2]))
                                            {
                                                case 1:
                                                    setwin4();
                                                    break;
                                                case 3:
                                                    setwin2();
                                                    break;
                                                case 4:
                                                    setwin3();
                                                    break;

                                            }
                                            break;
                                        case 3:
                                            switch (int.Parse(strings[2]))
                                            {
                                                case 1:
                                                    setwin3();
                                                    break;
                                                case 2:
                                                    setwin4();
                                                    break;
                                                case 4:
                                                    setwin2();
                                                    break;

                                            }
                                            break;
                                        case 4:
                                            switch (int.Parse(strings[2]))
                                            {
                                                case 1:
                                                    setwin2();
                                                    break;
                                                case 2:
                                                    setwin3();
                                                    break;
                                                case 3:
                                                    setwin4();
                                                    break;

                                            }
                                            break;

                                    }
                                }
                            }
                            phongdangchoi = false;
                            turndau = 1;
                            label4.Text = "Sẵn sàng: 0";
                            dangchoi = false;
                            button2.Show();
                            yourturn = -1;
                            label1.Hide();
                            button4.Hide();
                            button1.Hide();
                            player4.Hide();
                            player3.Hide();
                            player2.Hide();
                            turn_1.Hide();
                            turn_4.Hide();
                            turn_3.Hide();
                            turn_2.Hide();
                            label2.Hide();
                            timer.Hide();
                            PictureBox[] pictures = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13 };
                            for (int i = 0; i < pictures.Length; i++) { pictures[i].Hide(); }
                            for (int i = 4; i < choi + 4 - 1; i++)
                            {
                                string[] tach = strings[i].Split(',');
                                xulytien(tach[0], tach[1], tach[2]);
                            }
                            string[] tach1 = strings[choi + 4 - 1].Split(',');
                            xulytien(tach1[0], tach1[1], tach1[2]);
                            this.Enabled = true;
                        });
                        Endgame.Start();
                    }
                    //Có người chơi bỏ lượt
                    else if (strings[0].CompareTo("Skip") == 0)
                    {
                        if (strings.Length > 2)
                        {
                            Ttype = strings[1];
                            if (skipped == 0)
                            {
                                string s = "SetTurn-" + yourturn.ToString();
                                turn = yourturn;
                                client.Send(serialize(s));
                            }
                            else
                            {
                                skipped = 0;
                            }
                        }
                        else
                        {
                            turn = int.Parse(strings[1]);
                            for (int index = 0; index < boturn.Length; index++)
                            {
                                if (boturn[index] == 0)
                                {
                                    boturn[index] = turn;
                                    break;
                                }
                            }
                        }
                        time = 31;
                        Invoke((MethodInvoker)delegate { timer1.Start(); });
                        tickef.Stop();
                    }
                    // Đặt lại lượt theo lệnh của sever                  
                    else if (strings[0].CompareTo("SetTurn") == 0)
                    {
                        if (strings.Length > 2)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                string[] tach = strings[i + 2].Split(',');
                                xulytien(tach[0], tach[1], tach[2]);
                            }
                        }
                        turn = int.Parse(strings[1]);
                        turndau = int.Parse(strings[1]);
                        skipped = 0;
                        time = 31;
                        Invoke((MethodInvoker)delegate { timer1.Start(); });
                        timer.Show();
                    }

                    //Sever gửi số người chơi                
                    else if (strings[0].CompareTo("PlayerNumber") == 0)
                    {
                        numberplayer.Text = "Số người chơi:";
                        numberplayer.Text += strings[1];
                    }
                    //Server gửi số người đã sẵng sàng
                    else if (strings[0].CompareTo("ReadyP") == 0)
                    {
                        label4.Text = "Sẵn sàng: " + strings[1];
                        if (strings[2] == "Thoat")
                        {
                            for (int i = 3; i < strings.Length; i++)
                            {
                                xulythoat(strings[i]);
                            }
                        }
                    }
                    else if (strings[0].CompareTo("Msg") == 0)
                    {
                        richTextBox1.Focus();
                        richTextBox1.Text += strings[1] + "\n";

                    }
                    //
                    //                                    
                    if (turn == yourturn)
                    {
                        //tự bỏ lượt nếu đã bỏ lượt trong round
                        if (skipped == 1)
                            if (Ttype == "Le" || Ttype == "Sanh" || Ttype == "Doi")
                                BoLuot();
                    }
                    //
                    if (playern != 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            checkBoxes[i].Hide();
                        }
                        checkBoxes[turn - 1].Show();
                    }
                }
            }
            catch { }
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
        #region xu ly
        void xulythoat(string thoat)
        {
            switch (stt)
            {
                case 1:
                    switch (int.Parse(thoat))
                    {
                        case 2:
                            player4.Hide();
                            turn_4.Hide();
                            label6.Hide();
                            label10.Hide();
                            pictureBox15.Hide();
                            break;
                        case 3:
                            player3.Hide();
                            turn_3.Hide();
                            label7.Hide();
                            label11.Hide();
                            pictureBox16.Hide();
                            break;
                        case 4:
                            player2.Hide();
                            turn_2.Hide();
                            label8.Hide();
                            label12.Hide();
                            pictureBox17.Hide();
                            break;
                    }
                    break;
                case 2:
                    switch (int.Parse(thoat))
                    {
                        case 1:
                            player2.Hide();
                            turn_2.Hide();
                            label8.Hide();
                            label12.Hide();
                            pictureBox17.Hide();
                            break;
                        case 3:
                            player4.Hide();
                            turn_4.Hide();
                            label6.Hide();
                            label10.Hide();
                            pictureBox15.Hide();
                            break;
                        case 4:
                            player3.Hide();
                            turn_3.Hide();
                            label7.Hide();
                            label11.Hide();
                            pictureBox16.Hide();
                            break;
                    }
                    break;
                case 3:
                    switch (int.Parse(thoat))
                    {
                        case 1:
                            player3.Hide();
                            turn_3.Hide();
                            label7.Hide();
                            label11.Hide();
                            pictureBox16.Hide();

                            break;
                        case 2:
                            player2.Hide();
                            turn_2.Hide();
                            label8.Hide();
                            label12.Hide();
                            pictureBox17.Hide();
                            break;
                        case 4:
                            player4.Hide();
                            turn_4.Hide();
                            label6.Hide();
                            label10.Hide();
                            pictureBox15.Hide();
                            break;
                    }
                    break;
                case 4:
                    switch (int.Parse(thoat))
                    {
                        case 1:
                            player4.Hide();
                            turn_4.Hide();
                            label6.Hide();
                            label10.Hide();
                            pictureBox15.Hide();
                            break;
                        case 2:
                            player3.Hide();
                            turn_3.Hide();
                            label7.Hide();
                            label11.Hide();
                            pictureBox16.Hide();
                            break;
                        case 3:
                            player2.Hide();
                            turn_2.Hide();
                            label8.Hide();
                            label12.Hide();
                            pictureBox17.Hide();
                            break;
                    }
                    break;
            }
        }
        void xulytien(string play, string hoten, string tien)
        {
            switch (stt)
            {
                case 1:
                    switch (int.Parse(play))
                    {
                        case 1:
                            if (label9.Text == hoten)
                            {
                                label5.Text = tien;
                            }
                            break;
                        case 2:
                            if (label6.Text == hoten)
                            {
                                label10.Text = tien;
                            }
                            break;
                        case 3:
                            if (label7.Text == hoten)
                            {
                                label11.Text = tien;
                            }
                            break;
                        case 4:
                            if (label8.Text == hoten)
                            {
                                label12.Text = tien;
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (int.Parse(play))
                    {
                        case 1:
                            if (label8.Text == hoten)
                            {
                                label12.Text = tien;
                            }
                            break;
                        case 2:
                            if (label9.Text == hoten)
                            {
                                label5.Text = tien;
                            }
                            break;
                        case 3:
                            if (label6.Text == hoten)
                            {
                                label10.Text = tien;
                            }
                            break;
                        case 4:
                            if (label7.Text == hoten)
                            {
                                label11.Text = tien;
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (int.Parse(play))
                    {
                        case 1:
                            if (label7.Text == hoten)
                            {
                                label11.Text = tien;
                            }

                            break;
                        case 2:
                            if (label8.Text == hoten)
                            {
                                label12.Text = tien;
                            }
                            break;
                        case 3:
                            if (label9.Text == hoten)
                            {
                                label5.Text = tien;
                            }
                            break;
                        case 4:
                            if (label6.Text == hoten)
                            {
                                label10.Text = tien;
                            }
                            break;
                    }
                    break;
                case 4:
                    switch (int.Parse(play))
                    {
                        case 1:
                            if (label6.Text == hoten)
                            {
                                label10.Text = tien;
                            }
                            break;
                        case 2:
                            if (label7.Text == hoten)
                            {
                                label11.Text = tien;
                            }
                            break;
                        case 3:
                            if (label8.Text == hoten)
                            {
                                label12.Text = tien;
                            }
                            break;
                        case 4:
                            if (label9.Text == hoten)
                            {
                                label5.Text = tien;
                            }
                            break;
                    }
                    break;
            }
        }
        #endregion

        private void textBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.Location = new Point(600, 240);
            richTextBox1.Size = new Size(200, 150);
            richTextBox1.Visible = true;
            richTextBox1.Enabled = true;
        }

        private void Game_Click(object sender, EventArgs e)
        {
            richTextBox1.Visible = false;
            richTextBox1.Enabled = false;
        }
        private void setava14(string tt)
        {
            if (decimal.Parse(tt) <= 65)
            {
                pictureBox14.Image = Image.FromFile("anxin.png");

            }
            if (decimal.Parse(tt) > 65 && decimal.Parse(tt) <= 500)
            {
                pictureBox14.Image = Image.FromFile("thuongdan.png");


            }
            if (decimal.Parse(tt) > 500 && decimal.Parse(tt) <= 2000)
            {
                pictureBox14.Image = Image.FromFile("doanhnhan.png");
            }
            if (decimal.Parse(tt) > 2000 && decimal.Parse(tt) <= 5000)
            {
                pictureBox14.Image = Image.FromFile("nguoigiau.png");
            }
            if (decimal.Parse(tt) > 5000)
            {
                pictureBox14.Image = Image.FromFile("typhu.png");
            }
        }
        private void setava15(string tt)
        {
            if (decimal.Parse(tt) <= 65)
            {
                pictureBox15.Image = Image.FromFile("anxin.png");

            }
            if (decimal.Parse(tt) > 65 && decimal.Parse(tt) <= 500)
            {
                pictureBox15.Image = Image.FromFile("thuongdan.png");


            }
            if (decimal.Parse(tt) > 500 && decimal.Parse(tt) <= 2000)
            {
                pictureBox15.Image = Image.FromFile("doanhnhan.png");
            }
            if (decimal.Parse(tt) > 2000 && decimal.Parse(tt) <= 5000)
            {
                pictureBox15.Image = Image.FromFile("nguoigiau.png");
            }
            if (decimal.Parse(tt) > 5000)
            {
                pictureBox15.Image = Image.FromFile("typhu.png");
            }
        }
        private void setava16(string tt)
        {
            if (decimal.Parse(tt) <= 65)
            {
                pictureBox16.Image = Image.FromFile("anxin.png");

            }
            if (decimal.Parse(tt) > 65 && decimal.Parse(tt) <= 500)
            {
                pictureBox16.Image = Image.FromFile("thuongdan.png");


            }
            if (decimal.Parse(tt) > 500 && decimal.Parse(tt) <= 2000)
            {
                pictureBox16.Image = Image.FromFile("doanhnhan.png");
            }
            if (decimal.Parse(tt) > 2000 && decimal.Parse(tt) <= 5000)
            {
                pictureBox16.Image = Image.FromFile("nguoigiau.png");
            }
            if (decimal.Parse(tt) > 5000)
            {
                pictureBox16.Image = Image.FromFile("typhu.png");
            }
        }
        private void setava17(string tt)
        {
            if (decimal.Parse(tt) <= 65)
            {
                pictureBox17.Image = Image.FromFile("anxin.png");

            }
            if (decimal.Parse(tt) > 65 && decimal.Parse(tt) <= 500)
            {
                pictureBox17.Image = Image.FromFile("thuongdan.png");


            }
            if (decimal.Parse(tt) > 500 && decimal.Parse(tt) <= 2000)
            {
                pictureBox17.Image = Image.FromFile("doanhnhan.png");
            }
            if (decimal.Parse(tt) > 2000 && decimal.Parse(tt) <= 5000)
            {
                pictureBox17.Image = Image.FromFile("nguoigiau.png");
            }
            if (decimal.Parse(tt) > 5000)
            {
                pictureBox17.Image = Image.FromFile("typhu.png");
            }
        }
        private void setwin2()
        {

            win2.Image = Image.FromFile("win.gif");
            win2.Show();
        }
        private void setwin3()
        {
            win3.Image = Image.FromFile("win.gif");
            win3.Show();
        }
        private void setwin4()
        {
            win4.Image = Image.FromFile("win.gif");
            win4.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (anhdahien)
            {
                timepicture--;
            }
            if (timepicture == 0)
            {
                pictureBox18.Visible = false;
                timepicture = 5;
                anhdahien = false;
                win1.Visible = false;
                win2.Visible = false;
                win3.Visible = false;
                win4.Visible = false;
                Invoke((MethodInvoker)delegate { timer2.Stop(); });
            }
        }
    }
}
