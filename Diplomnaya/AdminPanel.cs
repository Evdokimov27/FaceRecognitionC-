using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using VisioForge.Libs.AForge.Imaging;
using MySql.Data.MySqlClient;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using System.Data.SqlClient;
using Gst;

namespace MultiFaceRec
{
	public partial class AdminPanel : Form
	{
		public AdminPanel()
		{
			InitializeComponent();
			string sql = "SELECT status FROM recognition";
			foreach (DataRow row in bd.ExecuteSqlOverSSH(sql).Rows)
			{
				groupPerson.Items.Add(row[0].ToString());
			}
			face = new HaarCascade("haarcascade_frontalface_default.xml");
		}

		Image<Bgr, Byte> currentFrame;
		Capture grabber;
		HaarCascade face;
		Image<Gray, byte> result, TrainedFace = null;
		MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
		Image<Gray, byte> gray = null;
		List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
		List<string> labels = new List<string>();
		List<string> NamePersons = new List<string>();
		int ContTrain, t;
		string name, names = null;



		public byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();
		}

		private void AdminPanel_Load(object sender, EventArgs e)
		{
			grabber = new Capture(0);
			grabber.QueryFrame();
			System.Windows.Forms.Application.Idle += new EventHandler(FrameGrabber);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string sql = "INSERT INTO `recognition` (`id_recognition`, `status`) " +
											   "VALUES (NULL, @status)";
			bd.NonExecuteSqlOverSSH(sql, null);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string id = "";
			if (string.IsNullOrEmpty(namePerson.Text))
			{
				MessageBox.Show("Имя не введено");
			}
			else
			{
				if (string.IsNullOrEmpty(comboBox1.Text))
				{
					MessageBox.Show("Пол не введен");
				}
				else
				{
					if (string.IsNullOrEmpty(agePerson.Text))
					{
						MessageBox.Show("Возраст не введен");
					}
					else
					{
						if (string.IsNullOrEmpty(groupPerson.Text))
						{
							MessageBox.Show("Группа не выбрана");
						}
						else
						{
							ContTrain = ContTrain + 1;
							gray = grabber.QueryGrayFrame().Resize(facePerson.Width, facePerson.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
							MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
							face,
							1.1,
							10,
							Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
							new Size(20, 20));
							foreach (MCvAvgComp f in facesDetected[0])
							{
								if (facesDetected[0].Length > 0)
								{
									TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();


									TrainedFace = result.Resize(facePerson.Width, facePerson.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
									trainingImages.Add(TrainedFace);
									labels.Add(namePerson.Text);
									facePerson.Image = TrainedFace;

									string sql = $"INSERT INTO `person` (`id_person`, `fio`, `age`, `gender`, `face_pic`, `recognition_id_recognition`) VALUES (NULL, '{namePerson.Text}', {Convert.ToInt32(agePerson.Text)}, '{comboBox1.Text}', @img, '{groupPerson.SelectedIndex}')";
									MySqlParameter param = new MySqlParameter("@img", MySqlDbType.MediumBlob);
									param.Value = imageToByteArray(TrainedFace.Bitmap);
									int cmd = bd.NonExecuteSqlOverSSH(sql, param);

									string sql2 = $"SELECT id_person FROM `person`where fio = '{namePerson.Text}'";
									foreach (DataRow row in bd.ExecuteSqlOverSSH(sql2).Rows)
									{
										id = row[0].ToString();
									}
									string sql3 = $"INSERT INTO `time_to_work` (`id_time`, `enter_time`, `exit_time`, `id_person`) VALUES (NULL, '{dateTimePicker1.Value.ToShortTimeString()}:00', '{dateTimePicker2.Value.ToShortTimeString()}:00', '{id}')";
									int cmd3 = bd.NonExecuteSqlOverSSH(sql3, null);
									MessageBox.Show(namePerson.Text + " добавлен(а)", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
								}
							}
						}
					}
				}
			}
		}
	

		bd bd = new bd();
		void FrameGrabber(object sender, EventArgs e)
		{
			NamePersons.Add("");
			currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
			gray = currentFrame.Convert<Gray, Byte>();
			MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
		  face,
		  1.2,
		  10,
		  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
		  new Size(20, 20));
			foreach (MCvAvgComp f in facesDetected[0])
			{
				t = t + 1;
				result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
				currentFrame.Draw(f.rect, new Bgr(System.Drawing.Color.Green), 2);
				if (trainingImages.ToArray().Length != 0)
				{
					MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
					EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
					   trainingImages.ToArray(),
					   labels.ToArray(),
					   3000,
					   ref termCrit);
					//name = recognizer.Recognize(result);
					currentFrame.Draw(name, ref font, new System.Drawing.Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(System.Drawing.Color.Red));
				}
				NamePersons[t - 1] = name;
				NamePersons.Add("");
			}
			t = 0;
			for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
			{
				names = names + NamePersons[nnn] + ", ";
			}
			facePerson.Image = currentFrame;
			name = "";
			NamePersons.Clear();
		}
	}
}
