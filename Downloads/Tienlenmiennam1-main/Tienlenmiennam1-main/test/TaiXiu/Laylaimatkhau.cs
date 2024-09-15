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
using System.Collections;

namespace TaiXiu
{
    public partial class Laylaimatkhau : Form
    {
        int stt;
        public Laylaimatkhau()
        {
            InitializeComponent();
            textBox2.Text = "";
            Connect();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string s = "Laylaimatkhau-" + stt.ToString() + "-" + email;
            if (email.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập email đăng ký!");
            }
            else
            {
                client.Send(serialize(s));
            }
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
                    else if (chuoi[0] == "11")
                    {

                        textBox2.ForeColor = Color.Blue;
                        textBox2.Text = "Mật khẩu: " + chuoi[1];
                    }
                    else if (chuoi[0] == "12")
                    {
                        textBox2.ForeColor = Color.Red;
                        textBox2.Text = "Email này chưa được đăng ký!";
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

        private void Laylaimatkhau_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }
    }
}
