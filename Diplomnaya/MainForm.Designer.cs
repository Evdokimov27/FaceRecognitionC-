namespace MultiFaceRec
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
			this.label2 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
			this.button2 = new System.Windows.Forms.Button();
			this.imageBox1 = new Emgu.CV.UI.ImageBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button4 = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewImageColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 292);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(0, 13);
			this.label2.TabIndex = 9;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(6, 288);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(99, 26);
			this.button3.TabIndex = 10;
			this.button3.Text = "3. Админ-панель";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click_1);
			// 
			// imageBoxFrameGrabber
			// 
			this.imageBoxFrameGrabber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageBoxFrameGrabber.Location = new System.Drawing.Point(12, 12);
			this.imageBoxFrameGrabber.Name = "imageBoxFrameGrabber";
			this.imageBoxFrameGrabber.Size = new System.Drawing.Size(320, 230);
			this.imageBoxFrameGrabber.TabIndex = 4;
			this.imageBoxFrameGrabber.TabStop = false;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(130, 247);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(98, 31);
			this.button2.TabIndex = 3;
			this.button2.Text = "2. Добавление посетителя";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// imageBox1
			// 
			this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.imageBox1.Location = new System.Drawing.Point(38, 19);
			this.imageBox1.Name = "imageBox1";
			this.imageBox1.Size = new System.Drawing.Size(190, 134);
			this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imageBox1.TabIndex = 5;
			this.imageBox1.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(52, 170);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(161, 20);
			this.textBox1.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 173);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Имя: ";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(14, 247);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(98, 31);
			this.button1.TabIndex = 2;
			this.button1.Text = "1. Включить камеру";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.imageBox1);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Location = new System.Drawing.Point(342, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(255, 321);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Управление";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(76, 224);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(95, 17);
			this.checkBox1.TabIndex = 15;
			this.checkBox1.Text = "Вести запись";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(11, 199);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Цель: ";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(52, 196);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(161, 20);
			this.textBox2.TabIndex = 13;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(111, 288);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(138, 26);
			this.button4.TabIndex = 12;
			this.button4.Text = "4. Сформировать отчет";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
			this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5});
			this.dataGridView1.Location = new System.Drawing.Point(603, 18);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.dataGridView1.RowTemplate.Height = 100;
			this.dataGridView1.Size = new System.Drawing.Size(418, 315);
			this.dataGridView1.TabIndex = 11;
			// 
			// Column1
			// 
			this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Column1.HeaderText = "Время прихода";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "ФИО";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.Width = 59;
			// 
			// Column4
			// 
			this.Column4.HeaderText = "Группа";
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			this.Column4.Width = 67;
			// 
			// Column3
			// 
			this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Column3.HeaderText = "Фотография";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.Width = 78;
			// 
			// Column5
			// 
			this.Column5.HeaderText = "Опаздание";
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			this.Column5.Width = 88;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(36, 292);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(127, 37);
			this.button5.TabIndex = 12;
			this.button5.Text = "Открыть видео";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(36, 255);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(296, 28);
			this.button6.TabIndex = 13;
			this.button6.Text = "Сохранить видео";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// timer1
			// 
			this.timer1.Interval = 1;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(169, 292);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(163, 21);
			this.comboBox1.TabIndex = 14;
			// 
			// axWindowsMediaPlayer1
			// 
			this.axWindowsMediaPlayer1.Enabled = true;
			this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(12, 12);
			this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
			this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
			this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(320, 230);
			this.axWindowsMediaPlayer1.TabIndex = 15;
			// 
			// timer2
			// 
			this.timer2.Enabled = true;
			this.timer2.Interval = 1000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// FrmPrincipal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1043, 345);
			this.Controls.Add(this.axWindowsMediaPlayer1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.imageBoxFrameGrabber);
			this.Name = "FrmPrincipal";
			this.Text = "Camera Input & Face Detection";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPrincipal_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private Emgu.CV.UI.ImageBox imageBoxFrameGrabber;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private Emgu.CV.UI.ImageBox imageBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ComboBox comboBox1;
		private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewImageColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
		private System.Windows.Forms.Timer timer2;
	}
}

