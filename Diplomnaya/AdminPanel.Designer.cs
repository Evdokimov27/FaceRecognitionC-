namespace MultiFaceRec
{
	partial class AdminPanel
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
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.addGroup = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.groupPerson = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.agePerson = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.genderPerson = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.namePerson = new System.Windows.Forms.TextBox();
			this.facePerson = new Emgu.CV.UI.ImageBox();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.facePerson)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.addGroup);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.groupPerson);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.agePerson);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.genderPerson);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.namePerson);
			this.groupBox1.Controls.Add(this.facePerson);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(504, 242);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Добавление:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(226, 168);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 13);
			this.label5.TabIndex = 17;
			this.label5.Text = "Права доступа: ";
			// 
			// addGroup
			// 
			this.addGroup.Location = new System.Drawing.Point(381, 161);
			this.addGroup.Name = "addGroup";
			this.addGroup.Size = new System.Drawing.Size(93, 20);
			this.addGroup.TabIndex = 16;
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(313, 193);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(98, 31);
			this.button1.TabIndex = 15;
			this.button1.Text = "2. Добавить права";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(226, 101);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 13);
			this.label4.TabIndex = 14;
			this.label4.Text = "Группа доступа:";
			// 
			// groupPerson
			// 
			this.groupPerson.FormattingEnabled = true;
			this.groupPerson.Location = new System.Drawing.Point(320, 97);
			this.groupPerson.Name = "groupPerson";
			this.groupPerson.Size = new System.Drawing.Size(154, 21);
			this.groupPerson.TabIndex = 13;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(226, 74);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Возраст: ";
			// 
			// agePerson
			// 
			this.agePerson.Location = new System.Drawing.Point(410, 71);
			this.agePerson.Name = "agePerson";
			this.agePerson.Size = new System.Drawing.Size(64, 20);
			this.agePerson.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(226, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Пол: ";
			// 
			// genderPerson
			// 
			this.genderPerson.Location = new System.Drawing.Point(390, 45);
			this.genderPerson.Name = "genderPerson";
			this.genderPerson.Size = new System.Drawing.Size(84, 20);
			this.genderPerson.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(226, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Имя: ";
			// 
			// namePerson
			// 
			this.namePerson.Location = new System.Drawing.Point(313, 22);
			this.namePerson.Name = "namePerson";
			this.namePerson.Size = new System.Drawing.Size(161, 20);
			this.namePerson.TabIndex = 7;
			// 
			// facePerson
			// 
			this.facePerson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.facePerson.Location = new System.Drawing.Point(8, 19);
			this.facePerson.Name = "facePerson";
			this.facePerson.Size = new System.Drawing.Size(198, 159);
			this.facePerson.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.facePerson.TabIndex = 5;
			this.facePerson.TabStop = false;
			// 
			// button2
			// 
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(8, 193);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(98, 31);
			this.button2.TabIndex = 3;
			this.button2.Text = "1. Добавить пользоватля";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// AdminPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(528, 268);
			this.Controls.Add(this.groupBox1);
			this.Name = "AdminPanel";
			this.Text = "Admin Panel";
			this.Load += new System.EventHandler(this.AdminPanel_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.facePerson)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox addGroup;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox groupPerson;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox agePerson;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox genderPerson;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox namePerson;
		private Emgu.CV.UI.ImageBox facePerson;
		private System.Windows.Forms.Button button2;
	}
}