using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System.Drawing.Imaging;
using Accord.Video.FFMPEG;
using Microsoft.Office.Interop.Excel;
using System.Data;
using VisioForge.Libs.MediaFoundation.OPM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MultiFaceRec
{
    public partial class Camera : Form
    {
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        string group = "";
        string time;
        HaarCascade face;
        Image<Gray, byte> result, TrainedFace = null;
        Image<Rgb, byte> Face = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        int ContTrain, NumLabels, t;
        string name = null;
        List<string> time_to_enter = new List<string>();
        List<string> time_to_exit = new List<string>();
        List<string> fio_time = new List<string>();
        List<bool> pers_warning = new List<bool>();
        bd bd = new bd();
		string host = "192.168.1.3";
		String perms = null;


        List<String> person = new List<String>();
		public Camera()
        {
			InitializeComponent();
			LoadVideo();
            axWindowsMediaPlayer1.Visible = false;
            // путь к модели обнаружения лиц
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");

            string sql3 = "SELECT fio FROM `person` WHERE CHAR_LENGTH(face_pic) > 0 UNION SELECT fio FROM `visiter` WHERE CHAR_LENGTH(face_pic) > 0";
            foreach (DataRow row in bd.ExecuteSqlOverSSH(sql3).Rows)
            {
                labels.Add(row[0].ToString());
            }
            ContTrain = 100;
            string LoadFaces;
            string sql2 = "SELECT face_pic FROM `person` WHERE CHAR_LENGTH(face_pic) > 0 UNION SELECT face_pic FROM `visiter` WHERE CHAR_LENGTH(face_pic) > 0";
            foreach (DataRow row in bd.ExecuteSqlOverSSH(sql2).Rows)
            {
                trainingImages.Add(new Image<Gray, byte>((Bitmap)byteArrayToImage((byte[])row[0])));
			}


            string sql4 = "SELECT fio, enter_time, exit_time FROM `time_to_work`, `person` WHERE person.id_person = time_to_work.id_person";
            foreach (DataRow row in bd.ExecuteSqlOverSSH(sql4).Rows)
            {
                time_to_enter.Add(row[1].ToString());
                time_to_exit.Add(row[2].ToString());
                fio_time.Add(row[0].ToString());
                pers_warning.Add(false);
            }
		}

        private void button1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Visible = false;
            //Обязательно сделать подключение к камере через rtsp
            grabber = new Capture("rtsp://192.168.1.3:8554/");
			grabber.QueryFrame();
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
            Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
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
            Worksheet worksheet2 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add();
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
            if (checkBox1.Checked == true) timer1.Enabled = true;
            else timer1.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < time_to_enter.Count; i++)
            {
                if (DateTime.Now > Convert.ToDateTime(time_to_enter[i]) && pers_warning[i] != true)
                {
                    pers_warning[i] = true;
                    MessageBox.Show("Зафиксирована неявка: " + fio_time[i] + ". Рабочее время с: " + time_to_enter[i] + " до " + time_to_exit[i]);

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

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Имя не введено");
            }
            else
            {
                string sql = $"INSERT INTO `visiter` (`id_visiter`, `fio`, `face_pic`, `reason`) VALUES (NULL, '{textBox1.Text}', '{imageToByteArray(TrainedFace.Bitmap)}', '{textBox2.Text}');";
				MessageBox.Show(textBox1.Text + " добавлен(а)", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
				int cmd = bd.NonExecuteSqlOverSSH(sql, null);
            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            currentFrame = grabber.QueryFrame().Resize(imageBoxFrameGrabber.Width, imageBoxFrameGrabber.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
			var time_img = currentFrame.Bitmap;
			Graphics time_graph = Graphics.FromImage(time_img);
			System.Drawing.Font timeFont = new System.Drawing.Font("Arial", 8);
			SolidBrush timeBrush = new SolidBrush(System.Drawing.Color.Red);
			PointF timePoint = new PointF(0, 0);
			time_graph.DrawString(DateTime.Now.ToString(), timeFont, timeBrush, timePoint);
			currentFrame.Bitmap = time_img;

			gray = currentFrame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.1,
          // 10 качество обнаружения, ломается при большом числе
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(imageBox1.Width, imageBox1.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                currentFrame.Draw(f.rect, new Bgr(System.Drawing.Color.Green), 2);

                if (trainingImages.ToArray().Length != 0)
                {
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       2500,
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
                }
                if ("Неизвестный" != name)
                {
                    if (name != "")
                    {
                        string Warning = "";
                        string sql = "SELECT fio FROM `person` where fio LIKE '" + name + "' GROUP BY fio";
                        foreach (DataRow row in bd.ExecuteSqlOverSSH(sql).Rows)
                        {
                            if (row[0].ToString() != "") group = "Рабочий";
                        }
                        string sql2 = "SELECT fio FROM `visiter` where fio LIKE '" + name + "' GROUP BY fio";
                        foreach (DataRow row in bd.ExecuteSqlOverSSH(sql2).Rows)
                        {
                            if (row[0].ToString() != "") group = "Посетитель";
                        }

                        string time_to_work = $"SELECT person.fio, time_to_work.enter_time FROM person, time_to_work WHERE (person.id_person = time_to_work.id_person && person.fio = '{name}') && (enter_time < '{DateTime.Now.ToShortTimeString()}:00')";

                        foreach (DataRow row in bd.ExecuteSqlOverSSH(time_to_work).Rows)
                        {
                            if (row[0].ToString() != "") Warning = "Опаздал";
                        }

                        string time_to_work2 = $"SELECT person.fio, time_to_work.enter_time FROM person, time_to_work WHERE (person.id_person = time_to_work.id_person && person.fio = '{name}') && (enter_time > '{DateTime.Now.ToShortTimeString()}:00')";
                        foreach (DataRow row in bd.ExecuteSqlOverSSH(time_to_work2).Rows)
                        {
                            if (row[0].ToString() != "") Warning = "Нет";

                        }
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
                        foreach (DataRow row in bd.ExecuteSqlOverSSH(sql_name).Rows)
                        {
                            perms = row[1].ToString();
                        }
                    }
                }


            }

            imageBox1.Image = TrainedFace;
            TrainedFace = null;
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
            if (imageBox1.Image != null)
            {
                string name_pers = name;
                string sql_name = "SELECT fio, recognition.status FROM person, recognition WHERE (fio = '" + name_pers + "')" +
                " and (recognition.id_recognition = recognition_id_recognition);";
                foreach (DataRow row in bd.ExecuteSqlOverSSH(sql_name).Rows)
                {
                    perms = row[1].ToString();
                }
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
                imageBox1.Image = null;
                name_pers = null;
                name = null;
            }
            else MessageBox.Show("Лицо не обноружено. Повторите попытку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
}


