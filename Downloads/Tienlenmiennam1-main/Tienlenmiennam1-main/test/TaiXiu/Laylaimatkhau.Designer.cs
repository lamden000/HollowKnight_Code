namespace TaiXiu
{
    partial class Laylaimatkhau
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
            this.textBox1 = new Krypton.Toolkit.KryptonTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new Krypton.Toolkit.KryptonTextBox();
            this.button1 = new Krypton.Toolkit.KryptonButton();
            this.kryptonPictureBox1 = new Krypton.Toolkit.KryptonPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.CueHint.CueHintText = "Email xác nhận";
            this.textBox1.Location = new System.Drawing.Point(93, 264);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(363, 39);
            this.textBox1.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | Krypton.Toolkit.PaletteDrawBorders.Left)
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.textBox1.StateCommon.Border.Rounding = 18F;
            this.textBox1.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(93, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 28);
            this.label3.TabIndex = 19;
            this.label3.Text = "Reset Pasword";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(363, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "Nhập Email liên kết với tài khoản để nhận lại Pasword";
            // 
            // textBox2
            // 
            this.textBox2.CueHint.CueHintText = "Mật khẩu sẽ nhận";
            this.textBox2.Location = new System.Drawing.Point(93, 322);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(363, 39);
            this.textBox2.StateCommon.Back.Color1 = System.Drawing.Color.Silver;
            this.textBox2.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | Krypton.Toolkit.PaletteDrawBorders.Left)
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.textBox2.StateCommon.Border.Rounding = 18F;
            this.textBox2.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 418);
            this.button1.Name = "button1";
            this.button1.OverrideDefault.Back.Color1 = System.Drawing.Color.DarkOrchid;
            this.button1.OverrideDefault.Back.Color2 = System.Drawing.Color.DarkOrchid;
            this.button1.Size = new System.Drawing.Size(136, 48);
            this.button1.StateCommon.Back.Color1 = System.Drawing.Color.DarkOrchid;
            this.button1.StateCommon.Back.Color2 = System.Drawing.Color.DarkViolet;
            this.button1.StateCommon.Border.Color1 = System.Drawing.Color.DarkOrchid;
            this.button1.StateCommon.Border.Color2 = System.Drawing.Color.DarkOrchid;
            this.button1.StateCommon.Border.DrawBorders = ((Krypton.Toolkit.PaletteDrawBorders)((((Krypton.Toolkit.PaletteDrawBorders.Top | Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | Krypton.Toolkit.PaletteDrawBorders.Left)
            | Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.button1.StateCommon.Border.Rounding = 10F;
            this.button1.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.button1.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.button1.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.TabIndex = 22;
            this.button1.Values.Text = "Lấy lại";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // kryptonPictureBox1
            // 
            this.kryptonPictureBox1.Image = global::TaiXiu.Properties.Resources.sendemail;
            this.kryptonPictureBox1.Location = new System.Drawing.Point(96, 22);
            this.kryptonPictureBox1.Name = "kryptonPictureBox1";
            this.kryptonPictureBox1.Size = new System.Drawing.Size(147, 121);
            this.kryptonPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.kryptonPictureBox1.TabIndex = 23;
            this.kryptonPictureBox1.TabStop = false;
            // 
            // Laylaimatkhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImage = global::TaiXiu.Properties.Resources.reset;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(678, 542);
            this.Controls.Add(this.kryptonPictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Name = "Laylaimatkhau";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonTextBox textBox1;
        private Label label3;
        private Label label4;
        private Krypton.Toolkit.KryptonTextBox textBox2;
        private Krypton.Toolkit.KryptonButton button1;
        private Krypton.Toolkit.KryptonPictureBox kryptonPictureBox1;
    }
}