
//Multiple face detection and recognition in real time
//Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
//Writed by Sergio Andrés Guitérrez Rojas
//"Serg3ant" for the delveloper comunity
// Sergiogut1805@hotmail.com
//Regards from Bucaramanga-Colombia ;)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using VisioForge.Libs.AForge.Video;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System.Drawing.Imaging;
using Accord.Video.FFMPEG;
using Microsoft.Office.Interop.Excel;

namespace MultiFaceRec
{
	public partial class FrmPrincipal : Form
    {
        
		System.DateTime td = System.DateTime.UtcNow;
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Bitmap originalImage;
        Capture grabber;
        string group = "";
        string time;
		HaarCascade face;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_SIMPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Rgb, byte> Face = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
		List<string> time_to_enter = new List<string>();
		List<string> time_to_exit = new List<string>();
		List<string> fio_time = new List<string>();
		List<bool> pers_warning = new List<bool>();

        Image<Bgr, Byte> ImageFrame;
        Bitmap bpm;
        MemoryStream s = new MemoryStream(); // камера
        MemoryStream z = new MemoryStream(); // считывание
        IntPtr hwnd;
        bool writ;
        bool Running = true;
        MJPEGStream stream;
        bd bd = new bd();

        String perms = null;


        List<String> person = new List<String>();


        public FrmPrincipal()
        {
            InitializeComponent();
			LoadVideo();
			axWindowsMediaPlayer1.Visible = false;
			// путь к модели обнаружения лиц
			face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            byte[] byteImage = null;
            bd.openConnection();
            string sql = "SELECT COUNT(*) as count FROM (SELECT count(person.face_pic) as count FROM `person` UNION SELECT count(visiter.face_pic) as count FROM `visiter`) as _count";
            MySqlCommand count = new MySqlCommand(sql, bd.getConnection());
            MySqlDataReader r_count = count.ExecuteReader();
            while (r_count.Read())
            {
                NumLabels = Convert.ToInt16(r_count[0]);
            }
            r_count.Close();
            string Labelsinfo = null;
            string sql3 = "SELECT fio FROM `person` UNION SELECT fio FROM `visiter`";
            MySqlCommand name = new MySqlCommand(sql3, bd.getConnection());
            MySqlDataReader r_name = name.ExecuteReader();
            while (r_name.Read())
            {
                labels.Add(r_name[0].ToString());
            }
            r_name.Close();
            ContTrain = NumLabels;
            string LoadFaces;
            string sql2 = "SELECT face_pic FROM `person` WHERE CHAR_LENGTH(face_pic) > 0 UNION SELECT face_pic FROM `visiter` WHERE CHAR_LENGTH(face_pic) > 0";
            MySqlCommand pict = new MySqlCommand(sql2, bd.getConnection());
            MySqlDataReader r_pict = pict.ExecuteReader();
            int tf = 0;
            while (r_pict.Read())
            {
                    byteImage = (byte[])r_pict[0];
                    trainingImages.Add(new Image<Gray, byte>((Bitmap)byteArrayToImage(byteImage)));
            }
            r_pict.Close();

			string sql4 = "SELECT fio, enter_time, exit_time FROM `time_to_work`, `person` WHERE person.id_person = time_to_work.id_person";
			MySqlCommand status4 = new MySqlCommand(sql4, bd.getConnection());
			MySqlDataReader stat4 = status4.ExecuteReader();
			while (stat4.Read())
			{
                time_to_enter.Add(stat4[1].ToString());
                time_to_exit.Add(stat4[2].ToString());
                fio_time.Add(stat4[0].ToString());
                pers_warning.Add(false);
			}
			stat4.Close();

			bd.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			axWindowsMediaPlayer1.Visible = false;
            //Обязательно сделать подключение к камере через rtsp
            grabber = new Capture(0);
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            System.Windows.Forms.Application.Idle += new EventHandler(FrameGrabber);

            button1.Enabled = false;
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = ExcelApp.Workbooks.Add();
			workbook.Title = "Отчет о посетителях";
			
            // Получение первой страницы книги
			Worksheet worksheet = workbook.Sheets[1];
			worksheet.Name = "Список прошедших";
			worksheet.Columns.ColumnWidth = 15;

			worksheet.Cells[1, 1] = "Время прихода";
			worksheet.Cells[1, 2] = "ФИО";
			worksheet.Cells[1, 3] = "Группа";
			worksheet.Cells[1, 4] = "Фото";
			worksheet.Cells[1, 5] = "Опаздание";
			worksheet.Cells[1, 6] = "Время ухода";


            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    if (j == 3) worksheet.Cells[j + 2, i + 1] = dataGridView1[i, j].Value;
                    else worksheet.Cells[j + 2, i + 1] = dataGridView1[i, j].Value;
                }
            }
			// Получение второй страницы книги
			Worksheet worksheet2 = workbook.Sheets.Add();
			worksheet2.Name = "Список отсутствующих";

			worksheet2.Columns.ColumnWidth = 15;

			worksheet2.Cells[1, 1] = "ФИО";
			worksheet2.Cells[1, 2] = "Рабочее время";
			worksheet2.Cells[1, 3] = "Причина отсутствия";
			for (int i = 0; i < time_to_enter.Count; i++)
			{
				worksheet2.Cells[i + 2, 1] = fio_time[i];
				worksheet2.Cells[i + 2, 2] = time_to_enter[i] + "-" + time_to_exit[i];
			}

			ExcelApp.Visible = true;

			

		}

		void LoadVideo()
		{
            comboBox1.Items.Clear();
			string host = "192.168.1.5";
			string username = "pi";
			string password = "root";
			int port = 22;

			using (SftpClient client = new SftpClient(host, port, username, password))
			{
				try
				{
					client.Connect();

					List<string> fileList = GetFileList(client, "Video/"); // Укажите путь к папке на удаленном сервере

					// Вывод списка файлов в ComboBox
					foreach (string file in fileList)
					{
						comboBox1.Items.Add(file);
					}
				}
				catch (Exception ex)
				{
					// Обработка ошибок подключения или выполнения операций по SSH
					Console.WriteLine("Ошибка: " + ex.Message);
				}
				finally
				{
					client.Disconnect();
				}
			}
		}

		static List<string> GetFileList(SftpClient client, string path)
		{
			List<string> fileList = new List<string>();

			var files = client.ListDirectory(path);
			foreach (var file in files)
			{
				if (!file.IsDirectory)
				{
					fileList.Add(file.Name);
				}
			}

			return fileList;
		}

        public static void OpenFileOverSsh(string host, string username, string password, string remoteFilePath, string localFilePath)
        {
            using (var client = new SftpClient(host, username, password))
            {
                client.Connect();

                using (var fileStream = System.IO.File.OpenWrite(localFilePath))
                {
                    client.DownloadFile(remoteFilePath, fileStream);
                }

                client.Disconnect();
            }
        }

		private void button5_Click(object sender, EventArgs e)
        {
			if (comboBox1.Text != "")
            {
                axWindowsMediaPlayer1.Visible = true;
                grabber = null;
                System.Windows.Forms.Application.Idle -= new EventHandler(FrameGrabber);
                imageBoxFrameGrabber.Image = null;
                button1.Enabled = true;

                string host = "192.168.1.5";
                string username = "pi";
                string password = "root";
                int port = 22;
                string remoteFilePath = $"Video/{comboBox1.Text}"; // Укажите путь к удаленному файлу на хосте
                string localFilePath = $"Video/{comboBox1.Text}"; // Укажите путь, куда сохранить файл на локальной машине
				OpenFileOverSsh(host, username, password, remoteFilePath, localFilePath);
				axWindowsMediaPlayer1.URL = localFilePath;
				axWindowsMediaPlayer1.Ctlcontrols.play();
			}
			else MessageBox.Show("Выберите файл для открытия");
		}

		public static void UploadSFTPFile(string host, string username, string password, string sourcefile, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                client.ChangeDirectory(destinationpath);
                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                {
                    client.BufferSize = 1073741824;
                    client.UploadFile(fs, Path.GetFileName(sourcefile));
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Stop();
			SaveVideo();
            LoadVideo();
			timer1.Start();
		}
		static void CreateVideoFromImages(string[] imagePaths, string outputVideoPath, int frameRate)
		{
			VideoFileWriter writer = new VideoFileWriter();

			writer.Open(outputVideoPath, GetImageWidth(imagePaths[0]), GetImageHeight(imagePaths[0]), frameRate, VideoCodec.MPEG4); // Установите нужный кодек

			foreach (string imagePath in imagePaths)
			{
				Bitmap image = new Bitmap(imagePath);
				writer.WriteVideoFrame(image);
				image.Dispose();
			}

			writer.Close();
		}

		static int GetImageWidth(string imagePath)
		{
			using (Bitmap image = new Bitmap(imagePath))
			{
				return image.Width;
			}
		}

        static int GetImageHeight(string imagePath)
        {
            using (Bitmap image = new Bitmap(imagePath))
            {
                return image.Height;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
			if (imageBoxFrameGrabber.Image != null)
            {
                var img = imageBoxFrameGrabber.Image.Bitmap;
                string[] files = Directory.GetFiles(@"Frame\");
                int counter = files.Length + 1;
                string directoryPath = $"img{counter}.jpg";
                img.Save(@"Frame\" + directoryPath, ImageFormat.Bmp);
			}
        }

		private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
		{
            SaveVideo();
		}

		public void SaveVideo()
        {
			string[] imagePaths = Directory.GetFiles(@"Frame\", "*.jpg"); // Путь к изображениям и формат файлов
			if (imagePaths.Length < 1)
			{
				return;
			}
			else
			{

				string[] files = Directory.GetFiles(@"Frame\");

				int counter = files.Length + 1;
				Array.Sort(imagePaths, new ImageFileNameComparer());
				FileInfo fi = new FileInfo(@"Frame/img1.jpg");
				time = fi.CreationTime.ToString("d.MM.yyyy HH_mm_ss");

				string outputVideoPath = $"Video/{time}.avi"; // Путь к выходному видеофайлу и его формат

				int frameRate = 15; // Частота кадров в видео

				CreateVideoFromImages(imagePaths, outputVideoPath, frameRate);

				for (int i = 1; i < counter; i++)
				{
                    System.IO.File.Delete($"Frame/img{i}.jpg");
				}

				if (time != null)
				{
					string source = $"Video/{time}.avi";
					string destination = @"Video";
					string host = "192.168.1.5";
					string username = "pi";
					string password = "root";
					int port = 22;

					UploadSFTPFile(host, username, password, source, destination, port);
                    System.IO.File.Delete(source);
				}
				else MessageBox.Show("Видео не сохранено");
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
            if(checkBox1.Checked == true) timer1.Enabled = true;
			else timer1.Enabled = false;
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
            for(int i = 0; i < time_to_enter.Count; i++) 
            {
                if(DateTime.Now > Convert.ToDateTime(time_to_enter[i]) && pers_warning[i] != true)
                {
                    pers_warning[i] = true;
					MessageBox.Show("Зафиксирована неявка: "+fio_time[i] + " должен был явиться к " + time_to_enter[i]);
				}
			}
		}

		public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        private void button2_Click(object sender, System.EventArgs e)
        {
            bd.openConnection();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Имя не введено");
            }
            else
            {

                bd.openConnection();
                string sql = "INSERT INTO `visiter` (`id_visiter`, `fio`, `face_pic`, `reason`) VALUES (NULL, @fio, @picture, @reson);";
                MySqlCommand command = new MySqlCommand(sql, bd.getConnection());
                command.Parameters.Add("@fio", MySqlDbType.VarChar, 45).Value = textBox1.Text;
                command.Parameters.Add("@picture", MySqlDbType.MediumBlob).Value = imageToByteArray(TrainedFace.Bitmap);
                command.Parameters.Add("@reson", MySqlDbType.String, 11).Value = textBox2.Text;

                int cmd = command.ExecuteNonQuery();
                bd.closeConnection();

            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {

			NamePersons.Add("");
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            gray = currentFrame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          // 10 качество обнаружения, ломается при большом числе
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                currentFrame.Draw(f.rect, new Bgr(System.Drawing.Color.Green), 2);
                // база лиц не пустая приписать имя к лицу
                if (trainingImages.ToArray().Length != 0)
                {
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);
                       name = recognizer.Recognize(result);


                    foreach (MCvAvgComp fc in facesDetected[0])
                    {
                        Face = currentFrame.Copy(fc.rect).Convert<Rgb, byte>();
                        break;
                    }
                    TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    var b = currentFrame.Bitmap;
                    Graphics g = Graphics.FromImage(b);
                    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 8);
                    SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Red);
                    PointF drawPoint = new PointF(f.rect.X - 2, f.rect.Y - 15);
                    g.DrawString(name, drawFont, drawBrush, drawPoint);
                    currentFrame.Bitmap = b;
                    // вывод в лебл имени из бд
                    NamePersons[t - 1] = name;
                    NamePersons.Add("");
				}
				t = 0;
                for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                {
                    names = names + NamePersons[nnn] + ", ";
                }


                if ("Неизвестный" != name)
                {
                    if (name != "")
                    {
                        string Warning = "";

						bd.openConnection();
                        string sql = "SELECT fio FROM `person` where fio LIKE '" + name + "' GROUP BY fio";
                        MySqlCommand count = new MySqlCommand(sql, bd.getConnection());
                        MySqlDataReader r_count = count.ExecuteReader();
                        while (r_count.Read())
                        {
                            if (r_count[0].ToString() != "") group = "Рабочий";
                        }
                        string sql2 = "SELECT fio FROM `visiter` where fio LIKE '" + name + "' GROUP BY fio";
                        MySqlCommand count2 = new MySqlCommand(sql2, bd.getConnection());
                        r_count.Close();
                        MySqlDataReader r_count2 = count2.ExecuteReader();
                        while (r_count2.Read())
                        {
                            if (r_count2[0].ToString() != "") group = "Посетитель";
                        }
						r_count2.Close();
						string time_to_work = $"SELECT person.fio, time_to_work.enter_time FROM person, time_to_work WHERE (person.id_person = time_to_work.id_person && person.fio = '{name}') && (enter_time < '{DateTime.Now.ToShortTimeString()}:00')";

						MySqlCommand _time_to_work = new MySqlCommand(time_to_work, bd.getConnection());
						MySqlDataReader reader = _time_to_work.ExecuteReader();
						while (reader.Read())
						{
							if (reader[0].ToString() != "") Warning = "Опаздал";
						}
                        reader.Close();
						string time_to_work2 = $"SELECT person.fio, time_to_work.enter_time FROM person, time_to_work WHERE (person.id_person = time_to_work.id_person && person.fio = '{name}') && (enter_time > '{DateTime.Now.ToShortTimeString()}:00')";
                        MySqlCommand _time_to_work2 = new MySqlCommand(time_to_work2, bd.getConnection());
                        MySqlDataReader reader2 = _time_to_work2.ExecuteReader();
						while (reader2.Read())
						{
							if (reader2[0].ToString() != "") Warning = "Нет";

						}
						reader2.Close();
						Person pers = new Person(name, System.DateTime.Now.ToShortTimeString(), group, Face.Bitmap, Warning);
                        bool onWork = false;
                        if (dataGridView1.RowCount == 0)
                        {
                            dataGridView1.Rows.Add(pers.time, pers.fio, pers.group, pers.face, pers.warn);
                            person.Add(name);
                        }
                        else
                        {
                            if (!person.Contains(name))
                            {
                                person.Add(name);
                                dataGridView1.Rows.Add(pers.time, pers.fio, pers.group, pers.face, pers.warn);
                            }
                        }
						string sql_name = "SELECT fio, recognition.status FROM person, recognition WHERE (fio = '" + name + "')" +
						" and (recognition.id_recognition = recognition_id_recognition);";
						MySqlCommand admin = new MySqlCommand(sql_name, bd.getConnection());
						MySqlDataReader r_name = admin.ExecuteReader();
						while (r_name.Read())
						{
							perms = r_name.GetString(1).ToString();
						}
						r_name.Close();
						bd.closeConnection();
                    }
                }


			}

			imageBox1.Image = TrainedFace;
            TrainedFace = null;
            NamePersons.Clear();
			imageBoxFrameGrabber.Image = currentFrame;

		}
		//  Детект объектов (ебейшая нагрузка, желательно найти альтернативу или убрать)
		//  private async void Detect()
		//  {
		//      using var image = System.Drawing.Image.FromStream(s);
		//      if (image != null)
		//      {
		//          using var scorer = new YoloScorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
		//
		//          List<YoloPrediction> predictions = scorer.Predict(image);
		//
		//          using var graphics = Graphics.FromImage(image);
		//
		//          foreach (var prediction in predictions)
		//          {
		//              double score = Math.Round(prediction.Score, 2);
		//
		//              graphics.DrawRectangles(new Pen(prediction.Label.Color, 1),
		//                  new[] { prediction.Rectangle });
		//
		//              var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);
		//
		//              graphics.DrawString($"{prediction.Label.Name} ({score})",
		//                  new Font("Arial", 16, GraphicsUnit.Pixel), new SolidBrush(prediction.Label.Color),
		//                  new PointF(x, y));
		//              switch (prediction.Label.Name)
		//              {
		//                
		//              }
		//              image.Save(z, System.Drawing.Imaging.ImageFormat.Png);
		//              //pictureBox1.Image = image;
		//          }
		//      }
		//  }






		private void button3_Click_1(object sender, EventArgs e)
        {
			bd.openConnection();
                if (imageBox1.Image != null)
                {
                    string name_pers = name;
                    string sql_name = "SELECT fio, recognition.status FROM person, recognition WHERE (fio = '" + name_pers + "')" +
                    " and (recognition.id_recognition = recognition_id_recognition);";
                    MySqlCommand admin = new MySqlCommand(sql_name, bd.getConnection());
                    bd.openConnection();
                    MySqlDataReader r_name = admin.ExecuteReader();
                    while (r_name.Read())
                    {
                        perms = r_name.GetString(1).ToString();
                    }
                    r_name.Close();
                    bd.closeConnection();
                    if (name_pers != null)
                    {
                        if ("Администратор" == perms)
                        {
                            MessageBox.Show(perms);
                            AdminPanel admins = new AdminPanel();
                            admins.ShowDialog();

                        }
                        else
                        {
                            MessageBox.Show("Недостаточно прав");
                        }
                    }
                    bd.closeConnection();
                    imageBox1.Image = null;
                    name_pers = null;
                    name = null;
                }
				else MessageBox.Show("Лицо не обноружено. Повторите попытку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);			
        }
    }
    class Person
    {
        public string fio;
        public string time;
        public string group;
        public System.Drawing.Image face;
        public string warn;
        public Person(String fio, string time, string group, System.Drawing.Image face, string warn)
        {
            this.fio = fio;
            this.time = time;
            this.group = group;
            this.face = face;
            this.warn = warn;
        }
    }
}
class ImageFileNameComparer : IComparer<string>
{
	public int Compare(string x, string y)
	{
		string fileNameX = Path.GetFileNameWithoutExtension(x);
		string fileNameY = Path.GetFileNameWithoutExtension(y);

		if (fileNameX == null || fileNameY == null)
			return 0;

		int numberX, numberY;
		bool successX = int.TryParse(fileNameX.Substring(3), out numberX);
		bool successY = int.TryParse(fileNameY.Substring(3), out numberY);

		if (successX && successY)
			return numberX.CompareTo(numberY);

		return 0;
	}
}


