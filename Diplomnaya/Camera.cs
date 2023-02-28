
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using VisioForge.Libs.AForge.Video;
using System.Net;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;
using System.Net.NetworkInformation;
using Gst.Video;
using VisioForge.Libs.AForge.Imaging;
using MySql.Data.MySqlClient;
using VisioForge.Libs.ZXing.Common;
using Diplomnaya;
using System.Collections;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        System.DateTime td = DateTime.UtcNow;
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
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
        WorkList list = new WorkList();

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
            // путь к модели обнаружения лиц
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            byte[] byteImage = null;
            bd.openConnection();
            string sql = "SELECT count(face_pic) FROM `person` WHERE 1";
            MySqlCommand count = new MySqlCommand(sql, bd.getConnection());
            MySqlDataReader r_count = count.ExecuteReader();
            while (r_count.Read())
            {
                NumLabels = Convert.ToInt16(r_count[0]);
            }
            r_count.Close();
            string Labelsinfo = null;
            string sql3 = "SELECT fio_person FROM `person` WHERE 1";
            MySqlCommand name = new MySqlCommand(sql3, bd.getConnection());
            MySqlDataReader r_name = name.ExecuteReader();
            while (r_name.Read())
            {
                labels.Add(r_name[0].ToString());
            }
            r_name.Close();

            ContTrain = NumLabels;
            string LoadFaces;
            r_count.Close();
            string sql2 = "SELECT face_pic FROM `person` WHERE 1";
            MySqlCommand pict = new MySqlCommand(sql2, bd.getConnection());
            MySqlDataReader r_pict = pict.ExecuteReader();
            int tf = 0;
            while (r_pict.Read())
            {
                    byteImage = (byte[])r_pict[0];
                    trainingImages.Add(new Image<Gray, byte>((Bitmap)byteArrayToImage(byteImage)));
            }
            r_pict.Close();
            bd.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Обязательно сделать подключение к камере через rtsp
            //grabber = new Capture("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4");
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

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            list.ShowDialog();
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
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Имя не введено");
                }
                else
                {
                    //Обучение лицу
                    ContTrain = ContTrain + 1;

                    //Получение видео с камеры
                    gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Нейронка поиска лиц, доделать обработку
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face,
                    1.2,
                    10,
                    Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));

                    // Что делать с найденым лицом
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                        break;
                    }
                    TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    trainingImages.Add(TrainedFace);
                    labels.Add(textBox1.Text);

                    imageBox1.Image = TrainedFace;

                    MessageBox.Show(textBox1.Text + " добавлен", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("No face detected. Please check your camera or stand closer.", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            string sql = "INSERT INTO `person` (`id_person`, `fio_person`, `age`, `gender`, `face_pic`, `recognition_id_recognition`) " +
                               "VALUES (NULL, @fio, @age, @gender, @picture, @id_rec)";
            MySqlCommand command = new MySqlCommand(sql, bd.getConnection());
            command.Parameters.Add("@fio", MySqlDbType.VarChar, 45);
            command.Parameters.Add("@age", MySqlDbType.Int64, 11);
            command.Parameters.Add("@gender", MySqlDbType.VarChar, 10);
            command.Parameters.Add("@picture", MySqlDbType.MediumBlob);
            command.Parameters.Add("@id_rec", MySqlDbType.Int64, 11);

            command.Parameters["@fio"].Value = textBox1.Text;
            command.Parameters["@age"].Value = "25";
            command.Parameters["@gender"].Value = "женский";
            command.Parameters["@picture"].Value = imageToByteArray(TrainedFace.Bitmap);
            command.Parameters["@id_rec"].Value = 1;
            int cmd = command.ExecuteNonQuery();
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
                currentFrame.Draw(f.rect, new Bgr(Color.Green), 2);
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
                    Font drawFont = new Font("Arial", 8);
                    SolidBrush drawBrush = new SolidBrush(Color.Red);
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
                label2.Text = name;


				if ("Неизвестный" != perms)
				{
					if (name != "")
					{
						Person pers = new Person(name, DateTime.Now, Face.Bitmap);
						bool onWork = false;
						if (dataGridView1.RowCount == 0)
						{
							dataGridView1.Rows.Add(pers.time, pers.fio, pers.face);
							person.Add(name);
						}
						else
						{
							if (!person.Contains(name))
							{
								person.Add(name);
								dataGridView1.Rows.Add(pers.time, pers.fio, pers.face);
							}
						}
					}
				}
                
				name = "";
                NamePersons.Clear();
            }
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
            if (label2.Text != null)
            {
                string sql = "SELECT fio_person, recognition.status FROM person, recognition WHERE (fio_person = '" + label2.Text + "')" +
                    " and (recognition.id_recognition = recognition_id_recognition);";
                MySqlCommand admin = new MySqlCommand(sql, bd.getConnection());
                MySqlDataReader r_name = admin.ExecuteReader();
                while (r_name.Read())
                {
                    perms = r_name.GetString(1).ToString();
                }

                if ("Администратор" == perms)
                {
                    MessageBox.Show(perms);
                    AdminPanel admins = new AdminPanel();
                    admins.ShowDialog();

                }
                else if ("Рабочий" == perms)
                {
                    MessageBox.Show(perms);
                }
                else
                {
                    MessageBox.Show("Недостаточно прав");
                }

                r_name.Close();
            }
        }
    }
    class Person
    {
        public string fio;
        public DateTime time;
        public System.Drawing.Image face;
        public Person(String fio, DateTime time, System.Drawing.Image face)
        {
            this.fio = fio;
            this.time = time;
            this.face = face;
        }
    }
}
