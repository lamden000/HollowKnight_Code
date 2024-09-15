using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Net.Mime.MediaTypeNames;

namespace ServerChinh
{
    public partial class SERVERCHINH : Form
    {
        List<phong> listport;
        List<Server> a;
        List<Thread> b;
        List<(string, int)> thongtinngchoi;
        Modify modify=new Modify();
        public SERVERCHINH()
        {
            InitializeComponent();
            listport = new List<phong>();
            a = new List<Server>();
            b = new List<Thread>();
            thongtinngchoi = new List<(string, int)>();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        class phong
        {
            public int maphong;
            public int songuoi;
            public int dangchoi;
            public string tienphong;
            public string mk;
            public string chuPhong;
            public phong(int maphong, int songuoi)
            {
                this.maphong = maphong;
                this.songuoi = songuoi;
                this.dangchoi = 0;
            }
        }
        int laychiso2(int stt)
        {
            int count = 0;
            foreach (var i in thongtinngchoi)
            {
                if (i.Item2 == stt)
                {
                    return count;
                }
                else count++;
            }
            return -1;
        }
        bool ktmk(int port,string matkhau)
        {
            foreach(var i in a)
            {
                if(i.port == port)
                {
                    if (matkhau == i.getmk())
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        bool ktport(int stt)
        {
            foreach (var i in a)
            {
                if (i.getport() == stt)
                {
                    return true;
                }
            }
            return false;
        }
        int indexktport(int stt)
        {
            for (int i = 0; i < a.Count(); i++)
            {
                if (a[i].getport() == stt)
                {
                    return i;
                }
            }
            return -1;
        }

        int laystt(Socket s)
        {
            foreach (var i in clients)
            {
                if (s == i.Item1)
                {
                    return i.Item2;
                }
            }
            return -1;
        }
        int laychiso(int stt)
        {
            for (int i = 0; i < clients.Count(); i++)
            {
                if (stt == clients[i].Item2)
                {
                    return i;
                }
            }
            return -1;
        }

        bool ktstt(int stt)
        {
            foreach (var i in clients)
            {
                if (i.Item2 == stt)
                {
                    return true;
                }
            }
            return false;
        }
        #region Server
        IPEndPoint IP;
        Socket server;
        List<(Socket, int)> clients;
        void Connect()
        {
            clients = new List<(Socket, int)>();
            IP = new IPEndPoint(IPAddress.Any, 8888);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);
            Thread listen = new Thread
                (() =>
                {
                    try
                    {
                        while (true)
                        {
                            server.Listen(4);
                            Socket client = server.Accept();
                            int stt = 1;
                            while (true)
                            {
                                if (ktstt(stt))
                                {
                                    stt++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            client.Send(serialize("STT-" + stt.ToString()));
                            clients.Add((client, stt));

                            Thread recieve = new Thread(Recieve);
                            recieve.IsBackground = true;
                            recieve.Start(client);
                        }
                    }
                    catch
                    {
                        IP = new IPEndPoint(IPAddress.Any, 8888);
                        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    }
                });
            listen.IsBackground = true;
            listen.Start();
        }
        public int songuoi()
        {
            return clients.Count();
        }
        void Recieve(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string str = (string)deserialize(data);
                    string[] s = str.Split('-');
                    //
                    if (s[0].CompareTo("Form1") == 0)
                    {
                        if (s[1].CompareTo("Choingay") == 0)
                        {
                            choingayserver(s[3], int.Parse(s[4]),int.Parse(s[2]));
                        }
                        //
                        //
                        else if (s[1].CompareTo("Taoban") == 0)
                        {
                            string[] k = s[3].Split(',');
                            Taoban(s[2],k[0],k[1],int.Parse(s[4]));
                        }
                        else if (s[1].CompareTo("Chonban") == 0)
                        {
                            dsban(int.Parse(s[2]));
                        }
                        //
                        else if (s[1].CompareTo("Ketnoi") == 0)
                        {
                            string[] k = s[2].Split(',');
                            xinphepketnoi(int.Parse(k[1]), int.Parse(k[0]), k[2], k[3]);
                        }
                        else if (s[1].CompareTo("Laythongtin") == 0)
                        {
                            thongtin(s[3], s[2]);
                        }
                        else if (s[1].CompareTo("Thoat") == 0)
                        {
                            string query = "UPDATE TaiKhoan SET DN = 0 WHERE TenTaiKhoan = '" + s[2] + "'";
                            modify.TaiKhoans(query);
                        }
                    }
                    else if (s[0].CompareTo("Login") == 0)
                    {
                        if (s[2].CompareTo("DN") == 0)
                        {
                            ktdn(s[3], s[4], s[1]);
                        }
                    }
                    else if (s[0].CompareTo("DK") == 0)
                    {
                        ktdy(s[2], s[3], s[4], s[5], s[1]);
                    }
                    else if (s[0].CompareTo("Laylaimatkhau")==0)
                    {
                        laylaimk(s[2], s[1]);
                    }
                    else if (s[0].CompareTo("Msg") == 0)
                    {
                        string k = "Msg-" + s[1];
                        foreach (var item in clients)
                        {
                            item.Item1.Send(serialize(k));
                        }
                    }
                    else if (s[0].CompareTo("Naptien") == 0)
                    {
                        string tentk = s[2];
                        string query = "Update table Taikhoan set Tien=Tien+150 where TenTaiKhoan='" + tentk + "'";
                        string k = "Napxong-" + s[1];
                        foreach (var item in clients)
                        {
                            item.Item1.Send(serialize(k));
                        }
                    }
                }

            }
            catch
            {
                if (laystt(client) == -1)
                {
                    MessageBox.Show("Khong ton tai");
                }
                else
                {
                    if (laychiso2(laystt(client)) != -1)
                    {
                        string query = "UPDATE TaiKhoan SET DN = 0 WHERE TenTaiKhoan = '" + thongtinngchoi[laychiso2(laystt(client))].Item1 + "'";
                        modify.TaiKhoans(query);
                        thongtinngchoi.Remove(thongtinngchoi[laychiso2(laystt(client))]);
                    }
                    clients.Remove((client, laystt(client)));
                    client.Close();
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
        void Taoban(string tentk,string mk,string tien,int client)
        {
            string query = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "'";
            decimal tientht = xulytien(tien);
            if(modify.TaiKhoans(query)[0].Tien<tientht*13)
            {
                clients[laychiso(client)].Item1.Send(serialize("Không đủ tiền"));
                return;
            }
            int stt = 1;
            while (true)
            {
                if (ktport(stt))
                {
                    stt++;
                }
                else
                {
                    Server form = new Server(stt, mk, tien);
                    a.Add(form);
                    phong phong = new phong(stt, 1);
                    listport.Add(phong);
                    Thread formThread = new Thread(() =>
                    {
                        a[a.Count() - 1].ShowDialog();
                    });
                    b.Add(formThread);
                    b[b.Count() - 1].Start();

                    break;
                }
            }
            string s = "Taoban-" + stt.ToString()+"-"+tientht;
            clients[laychiso(client)].Item1.Send(serialize(s));
        }
        void dsban(int client)
        {
            string s = "modanhsach-";
            //Cập nhật trạng thái các phòng
            for (int i = 0; i < listport.Count(); i++)
            {
                listport[i].songuoi = a[i].songuoi();
                listport[i].dangchoi = a[i].dangchoi;
                listport[i].maphong = a[i].getport();
                listport[i].tienphong = a[i].tienphong;
                listport[i].mk = a[i].getmk();
                listport[i].chuPhong = a[i].chuPhong;
                
            }
            //gửi thông tin các phòng
            for (int i = 0; i < listport.Count(); i++)
            {
                s = s + listport[i].maphong + "," + listport[i].songuoi + "," + listport[i].dangchoi + "," + listport[i].tienphong + ","+listport[i].mk+","+listport[i].chuPhong+"-";
            }
            clients[laychiso(client)].Item1.Send(serialize(s));
        }
        void xinphepketnoi(int stt, int client,string tentk,string tien)
        {
            if (a[indexktport(stt)].songuoi() < 4 && !a[indexktport(stt)].kttkthoat(tentk) && xulytien(a[indexktport(stt)].tienphong)*13<=decimal.Parse(tien))
            {
                string s;
                if (a[indexktport(stt)].dangchoi == 0)
                    s = "0";
                else s = "1";
                clients[laychiso(client)].Item1.Send(serialize("Chophep-" + a[indexktport(stt)].getport().ToString()+"-"+a[indexktport(stt)].tienphong+"-" + s));
                listport[stt - 1].songuoi++;
            }
            else
            {
                clients[laychiso(client)].Item1.Send(serialize("Phòng đã đầy"));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = clients.Count().ToString();
        }
        void choingayserver(string tentk,int banvuavao,int client)
        {
            int port = -1;
            decimal tienb=5;
            bool dangchoi = false;
            for (int i = 0; i < a.Count(); i++)
            {
                if (a[i].songuoi() < 4 && a[i].dangchoi == 0 && int.Parse(a[i].maphong) != banvuavao && a[i].getmk() == "" && !a[i].kttkthoat(tentk))
                {
                    port = a[i].getport();
                    tienb = xulytien(a[i].tienphong);
                    break;
                }
            }
            if (port == -1)
            {
                for (int i = 0; i < a.Count(); i++)
                {
                    if (a[i].songuoi() < 4 && int.Parse(a[i].maphong) != banvuavao && a[i].getmk() == "" && !a[i].kttkthoat(tentk))
                    {
                        port = a[i].getport();
                        dangchoi = true;
                        tienb = xulytien(a[i].tienphong);
                        break;
                    }
                }
            }
            if (port == -1)
            {
                string query = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "'";
                if (modify.TaiKhoans(query)[0].Tien / 13 >= 5) {  Taoban(tentk, "", "5k", client); return; }
                else
                {
                    clients[laychiso(client)].Item1.Send(serialize("Không đủ tiền!"));
                }
                
            }
            else
            {

                string s = "choingay-" + port.ToString()+"-"+tienb.ToString();
                if (dangchoi)
                {
                    s += "-1";
                }
                else s += "-0";

                clients[laychiso(client)].Item1.Send(serialize(s));
            }
        }

        void ktdn(string tentk, string mk, string stt)
        {
            string k;
            if (tentk.Trim() == "") { k = "1"; clients[laychiso(int.Parse(stt))].Item1.Send(serialize(k)); return; }
            else if (mk.Trim() == "") { k = "2"; clients[laychiso(int.Parse(stt))].Item1.Send(serialize(k)); return; }
            else
            {
                string query = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "' and MatKhau = '" + mk + "'";
                if (modify.TaiKhoans(query).Count != 0)
                {
                    query = "Select * from TaiKhoan where DN=0 AND TenTaiKhoan = '" + tentk + "'";
                if (modify.TaiKhoans(query).Count != 0)
                {
                        decimal tien = modify.TaiKhoans(query)[0].Tien;
                        k = "3-" + tien; clients[laychiso(int.Parse(stt))].Item1.Send(serialize(k));
                }
                else
                {
                    k = "5"; clients[laychiso(int.Parse(stt))].Item1.Send(serialize(k));
                }
                return;
                }
                else
                {
                    k = "4"; clients[laychiso(int.Parse(stt))].Item1.Send(serialize(k));
                    return;
                }
            }
        }
        void ktdy(string tentk, string mk, string hoten, string email,string stt)
        {
            if (modify.TaiKhoans("Select * from TaiKhoan where Email = '" + email + "'").Count != 0) { clients[laychiso(int.Parse(stt))].Item1.Send(serialize("5")); return; }
            if (modify.TaiKhoans("Select * from TaiKhoan where Hoten = '" + hoten + "'").Count != 0) { clients[laychiso(int.Parse(stt))].Item1.Send(serialize("6")); return; }
            if (modify.TaiKhoans("Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "'").Count != 0) { clients[laychiso(int.Parse(stt))].Item1.Send(serialize("7")); return; }
            try
            {
                string query = "Insert into TaiKhoan (TenTaiKhoan, MatKhau, Email, Hoten) values ('" + tentk + "','" + mk + "','" + email + "','" + hoten + "')";
                modify.Command(query);
                clients[laychiso(int.Parse(stt))].Item1.Send(serialize("8"));
            }
            catch
            {
                clients[laychiso(int.Parse(stt))].Item1.Send(serialize("9"));
            }
        }
        void laylaimk(string email,string stt)
        {
            string query = "Select * from TaiKhoan where Email = '" + email + "'";
            if (modify.TaiKhoans(query).Count != 0)
            {
                string k = "11-"+modify.TaiKhoans(query)[0].MatKhau;
                clients[laychiso(int.Parse(stt))].Item1.Send(serialize(k));
            }
            else
            {
                clients[laychiso(int.Parse(stt))].Item1.Send(serialize("12"));
            }
        }
        void thongtin(string tentk,string stt)
        {
            string query = "UPDATE TaiKhoan SET DN = 1 WHERE TenTaiKhoan = '" + tentk + "'";
            modify.TaiKhoans(query);
            thongtinngchoi.Add((tentk,int.Parse(stt)));
            query = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "'";
            string s = "Thongtin-"; 
            s = s+ modify.TaiKhoans(query)[0].Hoten1 + "-" + modify.TaiKhoans(query)[0].Tien.ToString();
            clients[laychiso(int.Parse(stt))].Item1.Send(serialize(s));
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
        decimal xulytien(string s)
        {
            if (s == "5k")
            {
                return 5;
            }
            else if (s == "10k")
            {
                return 10;
            }
            else if (s == "20k")
            {
                return 20;
            }
            else if (s == "50k")
            {
                return 50;
            }
            else if (s == "100k")
            {
                return 100;
            }
            else if (s == "200k")
            {
                return 200;
            }
            else if (s == "500k")
            {
                return 500;
            }
            else if (s == "1M")
            {
                return 1000;
            }
            else if (s == "2M")
            {
                return 2000;
            }
            else if(s == "5M")
            {
                return 5000;
            }
            else if (s == "10M")
            {
                return 10000;
            }
            return -1;
        }
        #endregion
    }
}