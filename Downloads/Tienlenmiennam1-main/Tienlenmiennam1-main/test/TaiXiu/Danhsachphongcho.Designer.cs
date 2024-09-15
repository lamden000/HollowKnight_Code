using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TaiXiu
{
    partial class Danhsachphongcho
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            cChon = new ColumnHeader();
            cMaPhong = new ColumnHeader();
            cSoNguoi = new ColumnHeader();
            cDangChoi = new ColumnHeader();
            ctienphong = new ColumnHeader();
            cLoaiPhong = new ColumnHeader();
            textBox1 = new TextBox();
            label1 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.BackColor = Color.PaleTurquoise;
            listView1.BackgroundImageTiled = true;
            listView1.Columns.AddRange(new ColumnHeader[] { cChon, cMaPhong, cSoNguoi, cDangChoi, ctienphong, cLoaiPhong });
            listView1.Dock = DockStyle.Right;
            listView1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            listView1.Location = new Point(291, 0);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Scrollable = false;
            listView1.Size = new Size(950, 440);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.MouseClick += listView1_MouseClick;
            // 
            // cChon
            // 
            cChon.Text = "Chọn phòng";
            cChon.Width = 150;
            // 
            // cMaPhong
            // 
            cMaPhong.Text = "Mã Phòng";
            cMaPhong.Width = 150;
            // 
            // cSoNguoi
            // 
            cSoNguoi.Text = "Số Người";
            cSoNguoi.Width = 150;
            // 
            // cDangChoi
            // 
            cDangChoi.Text = "Tình trạng";
            cDangChoi.Width = 200;
            // 
            // ctienphong
            // 
            ctienphong.Text = "Tiền Phòng";
            ctienphong.Width = 150;
            // 
            // cLoaiPhong
            // 
            cLoaiPhong.Text = "Loại Phòng";
            cLoaiPhong.Width = 200;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.PaleTurquoise;
            textBox1.Location = new Point(8, 35);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(277, 27);
            textBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Rockwell", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Tomato;
            label1.Location = new Point(8, 9);
            label1.Name = "label1";
            label1.Size = new Size(283, 20);
            label1.TabIndex = 2;
            label1.Text = "Chọn phòng theo tên chủ phòng:";
            // 
            // button1
            // 
            button1.Location = new Point(175, 68);
            button1.Name = "button1";
            button1.Size = new Size(110, 29);
            button1.TabIndex = 3;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Danhsachphongcho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackgroundImage = Properties.Resources.anhtienlen;
            ClientSize = new Size(1228, 461);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(listView1);
            Name = "Danhsachphongcho";
            Text = "Danh Sách Phòng";
            FormClosing += Danhsachphongcho_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView1;
        private ColumnHeader cChon;
        private ColumnHeader cMaPhong;
        private ColumnHeader cSoPhong;
        private ColumnHeader cSoNguoi;
        private ColumnHeader cDangChoi;
        private ColumnHeader ctienphong;
        private ColumnHeader cLoaiPhong;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
    }
}