using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using System.Collections;

namespace ServerChinh
{
    public partial class Server : Form
    {
        public string maphong;
        public int port;
        private string mk;
        private decimal tienthua = 0;
        public string tienphong;
        private decimal tienchat = 0;
        private decimal tienchatdt = 0;
        int[] nhanthongtin = new int[4];
        string[] thongtinguilai = new string[4];
        List<(string, int)> listchat;
        List<int> thoatkhichoi;
        List<int> ngdangchoi;
        bool phongdangchoi = false;
        ThongtinPlayer[] players;
        Modify modify = new Modify();
        string Ttype;
        List<(int, int)>[] card = new List<(int, int)>[4];
        public string chuPhong;
        public Server(int prt = 0, string mk ="", string tienphong = "5k")
        {
            InitializeComponent();
            port = prt;
            maphong = prt.ToString();
            this.tienphong = tienphong;
            ngdangchoi = new List<int>();
            thoatkhichoi = new List<int>();
            this.Mk = mk;
            listchat = new List<(string, int)>();
            players = new ThongtinPlayer[4];
            for (int i = 0; i < 4; i++)
                nhanthongtin[i] = 0;
            for (int i = 0; i < 4; i++)
            {
                ThongtinPlayer p = new ThongtinPlayer("", "", 0);
                players[i] = p;
            }
            tienthua = 0;
            phongdangchoi = false;
            CheckForIllegalCrossThreadCalls = false;
            Connect();

        }
        #region maphong
        public string getmk()
        {
            return Mk;
        }
        public void Maphong(string s)
        {
            maphong = s;
        }

        public int getport()
        {
            return port;
        }

        bool ktsttnaycodangchoikhong(int stt)
        {
            foreach (var i in ngdangchoi)
            {
                if (stt == i)
                {
                    return true;
                }
            }
            return false;
        }
        bool sttthoat(int stt)
        {
            foreach (var i in thoatkhichoi)
                if (i == stt)
                    return true;
            return false;
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
        int laychiso1(Socket s)
        {
            for (int i = 0; i < clients.Count(); i++)
            {
                if (s == clients[i].Item1)
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
        #endregion
        #region Server
        IPEndPoint IP;
        Socket server;
        List<(Socket, int)> clients;
        //
        int ReadyP = 0;//Số người đã sẵn sàng
        int turn = 0;//Lượt đi
        List<int> exitedplayer=new List<int>(); //Lưu lại lượi của người đã thoát
        int maxj; // Số người chơi trong game lúc phát bài
        public int dangchoi = 0;//biến để bết phòng đã chơi chưa
        public int songdangchoi = 0;
        List<int> skippedPlayer=new List<int>();
        public string Mk { get => mk; set => mk = value; }

        void Connect()
        {
            clients = new List<(Socket, int)>();
            IP = new IPEndPoint(IPAddress.Any, port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);
            string str;
            Thread listen = new Thread
             (() =>
             {
                 try
                 {
                     while ( clients.Count() + thoatkhichoi.Count() <= 4)
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
                         str = "STT-" + stt.ToString() + "-" + (clients.Count() + 1 + thoatkhichoi.Count()).ToString();
                         str += "-Capnhat-Guilai";
                         if (clients.Count > 0)
                         {
                             for (int i = 0; i < 4; i++)
                             {

                                 if (players[i].taitk != "")
                                 {
                                     if (ktsttnaycodangchoikhong(i + 1))
                                         str += "-" + players[i].hoten + "," + players[i].dangtien + "," + (i + 1).ToString() + ",Co";
                                     else
                                         str += "-" + players[i].hoten + "," + players[i].dangtien + "," + (i + 1).ToString() + ",Ko";
                                 }
                             }
                         }
                         client.Send(serialize(str));
                         clients.Add((client, stt));
                         Thread recieve = new Thread(Recieve);
                         recieve.IsBackground = true;
                         recieve.Start(client);                         
                     }
                 }
                 catch
                 {
                     IP = new IPEndPoint(IPAddress.Any, port);
                     server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                 }
             });
            listen.IsBackground = true;
            listen.Start();
        }
        //
        //Nhận dữ liệu từ người chơi
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
                    string[] strings = str.Split("-");
                    //Sẵn sàng
                    if (strings[0].CompareTo("Ready") == 0)
                    {
                        if (!phongdangchoi)
                        {

                            ReadyP++;
                            str = "ReadyP-" + ReadyP + "-Kothoat";
                            foreach (var socket in clients)
                                socket.Item1.Send(serialize(str));

                            if (ReadyP >= clients.Count() && clients.Count() > 1)
                            {
                                songdangchoi = clients.Count();
                                PhatBai();
                                ReadyP = 0;
                                dangchoi = 1;
                            }
                        }
                        else
                        {
                            client.Send(serialize("Khongduocsansang"));
                        }
                    }

                    else if (strings[0] == "Tienthua")
                    {


                    }
                    else if (strings[0] == "Thoat")
                    {
                        str += "-" + clients.Count();
                        string hh = "UPDATE TaiKhoan SET DN = 0 WHERE TenTaiKhoan = '" + players[laystt(client) - 1].taitk + "'";
                        modify.TaiKhoans(hh);
                        clients.Remove(clients[laychiso(int.Parse(strings[1]))]);
                        client.Close();
                        foreach (var i in clients)
                        {

                            i.Item1.Send(serialize(str));

                        }
                    }
                    else if (strings[0] == "Thoatko")
                    {
                        xulythoat(ref client);
                    }
                    else if (strings[0] == "Cobaonhieu")
                    {

                    }
                    else if (strings[0] == "Capnhat")
                    {
                        Thread thread = new Thread(() =>
                        {

                            if (strings[1] == "Tatca")
                            {
                                string[] s = strings[3].Split(',');
                                ThongtinPlayer p1 = new ThongtinPlayer(s[2], s[0], decimal.Parse(s[1]));
                                players[int.Parse(strings[2]) - 1] = p1;
                                chuPhong = players[0].hoten.TrimEnd();
                                Thread.Sleep(500);
                                string hh = "UPDATE TaiKhoan SET DN = 1 WHERE TenTaiKhoan = '" + players[laystt(client) - 1].taitk + "'";
                                modify.TaiKhoans(hh);
                                str = "Capnhat-Tatca-" + strings[2] + "-" + s[0] + "," + p1.dangtien + "-" + strings[4];
                                if (clients.Count() > 1)
                                {
                                    foreach (var i in clients)
                                    {
                                        if (i.Item2 != int.Parse(strings[2]))
                                            i.Item1.Send(serialize(str));
                                    }
                                }
                            }
                            else if (strings[1] == "Guilai")
                            {

                                string k = "Capnhat-Guilai-" + strings[2] + "-" + strings[4] + "-" + strings[5] + "-" + (clients.Count + thoatkhichoi.Count - 1).ToString();
                                thongtinguilai[int.Parse(strings[2]) - 1] = k;

                                clients[laychiso(int.Parse(strings[3]))].Item1.Send(serialize(k));

                            }
                        });
                        thread.Start();
                    }
                    else
                    {
                        //Thông tin bài được đánh
                        if (strings[0].CompareTo("ThongTinBai") == 0)
                        {
                            int bonusMultiplier = 0;
                            Ttype = strings[2];

                            if (Ttype == "Tu Quy" || Ttype == "Doi Thong_3" || Ttype == "Doi Thong_4")
                            {
                                bonusMultiplier = 10;
                            }
                            else if (Ttype == "2den")
                            {
                                bonusMultiplier = 7;
                            }
                            else if (Ttype == "2do")
                            {
                                bonusMultiplier = 10;
                            }
                            else if (Ttype == "doi21")
                            {
                                bonusMultiplier = 14;
                            }
                            else if (Ttype == "doi22")
                            {
                                bonusMultiplier = 17;
                            }
                            else if (Ttype == "doi23")
                            {
                                bonusMultiplier = 20;
                            }

                            tienchat += xulytien(tienphong) * bonusMultiplier;

                            if (listchat.Count > 0)
                            {
                                listchat.Add(("Chat", int.Parse(strings[1])));
                            }
                            else
                            {
                                listchat.Add(("", int.Parse(strings[1])));
                            }
                            int stt = int.Parse(strings[1]);
                            xulybai(strings);
                            SetTurn();
                            str = str + "-" + turn.ToString();
                        }

                        // Có người thắng
                        else if (strings[0].CompareTo("Win") == 0)
                        {                                                         
                            string k = tinhtien(int.Parse(strings[1]));
                            turn= int.Parse(strings[2]);
                            str = "GameEnd-Người chơi " + turn.ToString() + " thắng-" + turn.ToString() + "-" + ngdangchoi.Count().ToString() + k + "-Thoat";
                            Thread.Sleep(200);
                            Resetgame();
                        }
                        // Bỏ lượt
                        else if (strings[0].CompareTo("Skip") == 0)
                        {                          
                            if (strings[1].CompareTo("0") == 0)
                                skippedPlayer.Add(turn);
                            SetTurn();
                            if (skippedPlayer.Count == ngdangchoi.Count - 1-exitedplayer.Count&& (Ttype == "Sanh" || Ttype == "Le" || Ttype == "Doi" || !skippedPlayer.Contains(turn)))
                            {
                                 str = "Skip-0-" + turn.ToString();
                                 skippedPlayer.Clear();
                            }
                            else
                            {
                                str = "Skip-" + turn.ToString();
                            }
                        }
                        else if (strings[0].CompareTo("Taikhoan") == 0)
                        {

                        }
                        else if (strings[0].CompareTo("Conbaonhieu") == 0)
                        {
                            string s = "";
                            string s1 = "";
                            foreach (var i in clients)
                            {
                                if (!ktsttnaycodangchoikhong(i.Item2))
                                    s1 += "," + i;
                                else
                                    s += "," + i;
                            }
                            if (s1 == "")
                                s1 = ",";
                            if (s == "")
                                s = ",";
                            clients[laychiso(int.Parse(strings[1]))].Item1.Send(serialize("Danh" + s + "-Kodanh" + s1));
                        }

                        //Yêu cầu khởi tạo lại lượt
                        else if (strings[0].CompareTo("SetTurn") == 0)
                        {
                            if (listchat.Count() > 1)
                            {
                                if (listchat[listchat.Count() - 1].Item1 == "Chat")
                                {
                                    //Xử lý trừ tiền cộng tiền trong database gửi lại thông tin
                                    decimal n = (tienchatdt / xulytien(tienphong) - 1) * xulytien(tienphong);
                                    tienchat += n;
                                    int trutien = listchat[listchat.Count() - 2].Item2;
                                    int congtien = listchat[listchat.Count() - 1].Item2;
                                    players[trutien - 1].Tien = trutien1(players[trutien - 1].Tien, tienchat);
                                    players[congtien - 1].Tien += tienchat * 0.95m;
                                    string query = "Update TaiKhoan  SET Tien='" + players[trutien - 1].tien + "' where TenTaiKhoan = '" + players[trutien - 1].taitk + "'";
                                    modify.Command(query);
                                    query = "Update TaiKhoan  SET Tien='" + players[congtien - 1].tien + "' where TenTaiKhoan = '" + players[congtien - 1].taitk + "'";
                                    modify.Command(query);
                                    str += "-" + congtien + ',' + players[congtien - 1].hoten + ',' + players[congtien - 1].dangtien + '-' + trutien + ',' + players[trutien - 1].hoten + ',' + players[trutien - 1].dangtien;
                                }
                            }
                            listchat = new List<(string, int)>();
                            tienchat = 0;
                            tienchatdt = 0;
                            turn = int.Parse(strings[1]);
                        }
                        //
                        // Phản hồi cho người chơi
                        data = serialize(str);
                        foreach (var socket in clients)
                            socket.Item1.Send(data);
                    }
                }
            }
            catch
            {

                try
                {
                    string hh = "UPDATE TaiKhoan SET DN = 0 WHERE TenTaiKhoan = '" + players[laystt(client) - 1].taitk + "'";
                    modify.TaiKhoans(hh);
                    string str;
                    if (ktsttnaycodangchoikhong(laystt(client)))
                    {
                        players[laystt(client) - 1].taitk = "";
                        thongtinguilai[laystt(client) - 1] = "";
                        exitedplayer.Add(laystt(client));                       
                        thoatkhichoi.Add(clients[laychiso1(client)].Item2);
                        int sttclient = laystt(client);
                        clients.Remove(clients[laychiso1(client)]);
                        client.Close();
                        str = "Thoat-" + sttclient + "-" + clients.Count();
                        foreach (var socket in clients)
                            socket.Item1.Send(serialize(str));
                        Thread.Sleep(100);
                        if (exitedplayer.Last() == turn)
                        {
                            SetTurn();
                            str = "SetTurn-" + turn.ToString();
                            foreach (var socket in clients)
                            {
                                socket.Item1.Send(serialize(str));
                            }
                        }
                        //Nếu còn một người chơi người đó sẽ thắng
                        if (ngdangchoi.Count - thoatkhichoi.Count == 1)
                        {
                            Thread.Sleep(100);
                            string k = tinhtien(turn);
                            str = "GameEnd-Người chơi " + turn.ToString() + " thắng-" + turn.ToString() + "-" + ngdangchoi.Count.ToString() + k;
                            if (thoatkhichoi.Count() > 0)
                            {
                                str += "-Thoat";
                                foreach (var i in thoatkhichoi)
                                {

                                    players[i - 1].taitk = "";
                                    thongtinguilai[i - 1] = "";
                                    str += "-" + i.ToString();
                                }
                            }
                            else str += "-Kothoat";
                            foreach (var socket in clients)
                                socket.Item1.Send(serialize(str));

                            dangchoi = 0;
                            phongdangchoi = false;
                            ngdangchoi = new List<int>();
                            thoatkhichoi = new List<int>();
                        }
                        ReadyP = 0;

                    }
                    else
                    {

                        string sttt = laystt(client).ToString();
                        clients.Remove(clients[laychiso1(client)]);
                        players[int.Parse(sttt) - 1].taitk = "";
                        client.Close();
                        foreach (var i in clients)
                        {
                            i.Item1.Send(serialize("Thoat-" + sttt + "-" + clients.Count));
                        }
                    }
                }
                catch { }
            }
        }
        //
        //Serialize & Deserialize
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
        decimal trutien1(decimal hienco, decimal tien)
        {
            if (hienco >= tien)
                return hienco - tien;
            return 0;
        }


        //Hàm phát bài
        void PhatBai()
        {
            skippedPlayer.Clear();
            exitedplayer.Clear();            
            phongdangchoi = true;
            clients = clients.OrderBy(client => client.Item2).ToList();
            int[] cards = new int[53];
            int a;
            string str;
            Random random = new Random();
            int min = 53;//Lá bài nhỏ nhất được phát ra
            //
            //Làm mới bộ bài
            for (int i = 0; i < 53; i++)
                cards[i] = 1;
            //Phát bài    
            card = new List<(int, int)>[4];
            foreach (var j in clients)
            {
                ngdangchoi.Add(j.Item2);
                card[j.Item2 - 1] = new List<(int, int)>();
                str = "PhatBai-";

                for (int i = 0; i < 13; i++)
                {
                      a = random.Next(0, 52);
                      if (cards[a] == 1)
                      {
                         if (a < 48)
                             card[j.Item2 - 1].Add((a/4+3, a % 4 + 1));
                         else
                             card[j.Item2 - 1].Add((16, a % 4 + 1));
                        str = str + a.ToString() + ",";
                        cards[a] = 0;
                        //tìm lá bài nhỏ nhất và người nhận nó
                        if (a < min)
                        {
                            min = a;
                            turn = j.Item2;
                        }
                    }
                    else i--;
                }
                str = str + "-" + clients.Count.ToString() + "-" + (j.Item2).ToString() + "-0";
                clients[laychiso(j.Item2)].Item1.Send(serialize(str));
            }
            //Gửi lượt = lượt của người chơi có lá bài nhỏ nhất
            str = "SetTurn-" + turn.ToString();
            Thread.Sleep(200);
            foreach (var client in clients)
            {
                client.Item1.Send(serialize(str));
            }
            //Số người chơi lúc mới phát bài xong
            maxj = ngdangchoi.Count;
            //Kiểm tra ăn lăng
            Thread.Sleep(500);
            KTAnLang();
        }

        //Kiểm tra ăn lăng
        void KTAnLang()
        {
            string str=string.Empty;
            int[] point= new int[maxj];
            for (int i = 0; i < maxj; i++)
            {                                         
                //
                if (kt4heo(card[i])) 
                    point[i] = 1;
                //
                if (KT6Doi(card[i]))     
                    point[i] = 2;
                //
                if (kt5doithong(card[i]))
                    point[i] = 3;
                //
                if (KT3ToiA(card[i]))                  
                    point[i] = 4;              
            }
            int maxpoint = 0;
            int nguoiAnLang = -1;
            //
            for(int i = 0;i<maxj;i++)
            {
                if (point[i]>maxpoint)
                {
                    maxpoint = point[i];
                    nguoiAnLang = i+1;
                }    
            }
            if(maxpoint!=0)
            {
                if(maxpoint==1)
                {
                    str = "GameEnd-Người chơi " + nguoiAnLang + " Ăn lăng 4 Heo-" + nguoiAnLang + "-" +
                       ngdangchoi.Count().ToString() + tinhtien(nguoiAnLang) + "-Thoat" + "-Ăn Lăng-4 Heo";
                }  
                else if(maxpoint==2)
                {
                    str = "GameEnd-Người chơi " + nguoiAnLang + " Ăn lăng 6 đôi-" + nguoiAnLang + "-" +
                          ngdangchoi.Count().ToString() + tinhtien(nguoiAnLang) + "-Thoat" + "-Ăn Lăng-6 Đôi";
                }    
                else if (maxpoint==3)
                {
                    str = "GameEnd-Người chơi " + nguoiAnLang + " Ăn lăng 5 Đôi thông-" + nguoiAnLang + "-" +
                    ngdangchoi.Count().ToString() + tinhtien(nguoiAnLang) + "-Thoat" + "-Ăn Lăng-5 Đôi thông";
                }    
                else if(maxpoint==4)
                {
                    str = "GameEnd-Người chơi " + nguoiAnLang + " Ăn lăng 3 tới A-" +nguoiAnLang + "-" +
                        ngdangchoi.Count().ToString() + tinhtien(nguoiAnLang) + "-Thoat" + "-Ăn Lăng-3 tới A";
                }
            }
            if (str != string.Empty)
            {
                foreach (var client in clients)
                    client.Item1.Send(serialize(str));
                Resetgame();
            }
        }

        bool KT6Doi(List<(int, int)> values)
        {
            int[] arr = new int[13];
            for (int i = 0; i < 13; i++)
                arr[i] = values[i].Item1;
            int[] duplicate;
            duplicate=FindDuplicates(arr);
            if(duplicate.Length<6)
                return false;
            return true;
        }
        bool KT3ToiA(List<(int,int)> values )
        {
            int[] arr=new int[13];
            for (int i = 0;i<13;i++)
            {            
                arr[i] = values[i].Item1;          
            }            
            Array.Sort(arr);          
            if (arr[0] > 3)
               return false;
            int[] sequence = new int[12];
            for (int i = 0; i < sequence.Length; i++)
                 sequence[i] = 0;
            for (int i = 0; i < arr.Length; i++)
             {
               if (arr[i] != 16)
               sequence[arr[i] - 3] = arr[i];
             }           
             for (int i = 0; i < sequence.Length; i++)
                  if (sequence[i] == 0)
                     return false;
                return true;
        }
        bool kt5doithong(List<(int, int)> values)
        {
            int[] array = new int[13];
            for (int i = 0; i < 13; i++)
            {
                array[i] = values[i].Item1;
            }
            int[] duplicates = FindDuplicates(array);
            if (duplicates.Length < 5)
            {
                return false;
            }
            Array.Sort(duplicates);
            int point = 1;
            for (int i = 0; i < duplicates.Length - 1; i++)
            {
                if (duplicates[i] + 1 != duplicates[i + 1])
                    point = 1;
                else point++;

                if (point == 5)
                    return true;
            }
            return false;
        }
        bool kt4heo(List<(int, int)> values)
        {
            int[] array = new int[13];
            for (int i = 0; i < 13; i++)
            {
                array[i] = values[i].Item1;
            }
            Array.Sort(array);
            if (array[9] != 16)
            {
                return false;
            }    
            return true;
        }
        int[] FindDuplicates(int[] array)
        {
            var duplicateNumbers = new HashSet<int>();
            var seenNumbers = new HashSet<int>();

            foreach (var number in array)
            {
                if (!seenNumbers.Add(number))
                {
                    duplicateNumbers.Add(number);
                }
            }
            return duplicateNumbers.ToArray();
        }
        //
        //Hàm tạo lượt
        void SetTurn()
        {
            turn++;
            if (turn > maxj)
                    turn = 1;
           if (exitedplayer.Contains(turn))
            {
              turn++;
              if (turn > maxj)
              turn = 1;
             }
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
            else if (s == "5M")
            {
                return 5000;
            }
            else if (s == "10M")
            {
                return 10000;
            }
            return -1;
        }
        void xulybai(string[] strings)
        {

            string[] ThongTinBai = strings[3].Split(',');
            string[] LaBai = new string[2];
            int cardsn = ThongTinBai.Length - 1;
            int[] values = new int[cardsn];
            int[] types = new int[cardsn];
            for (int i = 0; i < cardsn; i++)
            {
                LaBai = ThongTinBai[i].Split("_");
                values[i] = int.Parse(LaBai[0]);
                types[i] = int.Parse(LaBai[1]);
                card[int.Parse(strings[1]) - 1].Remove((values[i], types[i]));
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = clients.Count().ToString();
        }
        public bool kttkthoat(string s)
        {
            foreach (var i in thoatkhichoi)
            {
                if (players[i - 1].taitk == s)
                {
                    return true;
                }
            }
            return false;
        }
        string tinhtien(int win)
        {
            List<(int, decimal)> chiphi = new List<(int, decimal)>();
            decimal tt = 0;
            decimal tb = xulytien(tienphong);
            foreach (var i in ngdangchoi)
            {
                if (i != win)
                {
                    decimal sum = 0;
                    if (card[i - 1].Count < 13)
                    {
                        sum = card[i - 1].Count * tb;
                        foreach (var j in card[i - 1])
                        {
                            if (j.Item1 == 16)
                            {
                                if (j.Item2 == 1 || j.Item2 == 2)
                                {
                                    sum += tb * 5;
                                }
                                else
                                {
                                    sum += tb * 8;
                                }
                            }
                        }
                    }
                    else
                    {
                        sum = tb * 26;
                    }
                    tt += sum;
                    chiphi.Add((i, sum));

                }
            }
            players[win - 1].Tien += tt * 0.95m;
            string query;
            string s = "";
            foreach (var i in chiphi)
            {
                if (players[i.Item1 - 1].Tien >= i.Item2)
                    players[i.Item1 - 1].Tien -= i.Item2;
                else
                    players[i.Item1 + 1].Tien = 0;
                s += "-" + i.Item1.ToString() + ',' + players[i.Item1 - 1].hoten + ',' + players[i.Item1 - 1].dangtien;
                query = "Update TaiKhoan  SET Tien='" + players[i.Item1 - 1].tien + "' where TenTaiKhoan = '" + players[i.Item1 - 1].taitk + "'";
                modify.Command(query);
            }
            s += "-" + win.ToString() + ',' + players[win - 1].hoten + ',' + players[win - 1].dangtien;
            return s;

        }

        void Resetgame()
        {
            string str;
            if (thoatkhichoi.Count() > 0)
            {
                str = "ReadyP-" + ReadyP + "-Thoat";
                foreach (var i in thoatkhichoi)
                {

                    players[i - 1].taitk = "";
                    thongtinguilai[i - 1] = "";
                    str += "-" + i.ToString();
                }
            }
            else str = "ReadyP-" + ReadyP + "-Kothoat";
            foreach (var socket in clients)
                socket.Item1.Send(serialize(str));

            for (int i = 0; i < 4; i++)
            {
                if (players[i].taitk != "")
                {
                    if (players[i].tien < xulytien(tienphong) * 13)
                    {
                        str += "-" + (i + 1).ToString();
                    }
                }
            }
            ngdangchoi = new List<int>();
            thoatkhichoi = new List<int>();
            listchat = new List<(string, int)>();
            phongdangchoi = false;
            dangchoi = 0;
        }
        void xulythoat(ref Socket client)
        {
            if (phongdangchoi)
            {
                string str;
                if (ktsttnaycodangchoikhong(laystt(client)))
                {
                    exitedplayer.Add(laystt(client));
                    thoatkhichoi.Add(clients[laychiso1(client)].Item2);
                    clients.Remove(clients[laychiso1(client)]);

                    client.Close();
                    //Chia lại lượt
                    //
                    SetTurn();
                    str = "BoTurn-" + turn.ToString();
                    foreach (var socket in clients)
                        socket.Item1.Send(serialize(str));
                    //Gửi số lượng người chơi còn lại trong bàn                                                       
                    str = "PlayerNumber-" + clients.Count().ToString();

                    foreach (var socket in clients)
                    {
                        socket.Item1.Send(serialize(str));
                    }

                    //Nếu còn một người chơi người đó xẽ thắng

                    if (ngdangchoi.Count - thoatkhichoi.Count == 1 && dangchoi == 1)
                    {                       
                        str = "GameEnd-Bạn Thắng";
                        if (thoatkhichoi.Count() > 0)
                        {
                            str += "-Thoat";
                            foreach (var i in thoatkhichoi)
                            {
                                thongtinguilai[i - 1] = "";
                                players[i - 1].hoten = "";
                                str += "-" + i.ToString();
                            }
                        }
                        else str += "-Kothoat";
                        phongdangchoi = false;
                        foreach (var socket in clients)
                            socket.Item1.Send(serialize(str));
                        dangchoi = 0;
                        thoatkhichoi = new List<int>();
                        ngdangchoi = new List<int>();
                    }
                    ReadyP = 0;
                }
                else
                {
                    string sttt = laystt(client).ToString();
                    clients.Remove(clients[laychiso1(client)]);
                    players[int.Parse(sttt) - 1].hoten = "";
                    client.Close();
                    foreach (var i in clients)
                    {
                        i.Item1.Send(serialize("Thoat-" + sttt));
                    }
                }
            }
            else
            {

                string str = "Thoat-" + clients[laychiso1(client)].Item2;
                clients.Remove(clients[laychiso1(client)]);
                players[laystt(client) - 1].hoten = "";
                client.Close();
                foreach (var i in clients)
                {
                    i.Item1.Send(serialize(str));
                }
            }
        }
    }
}
    #endregion


