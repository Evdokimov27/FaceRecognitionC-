using Emgu.CV.Structure;
using Emgu.CV;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.CvEnum;

namespace MultiFaceRec
{
	public partial class AdminPanel : Form
	{
		public AdminPanel()
		{
			InitializeComponent();

			bd.openConnection();
			string sql = "SELECT status FROM recognition";
			MySqlCommand status = new MySqlCommand(sql, bd.getConnection());
			MySqlDataReader stat = status.ExecuteReader();
			while (stat.Read())
			{
				groupPerson.Items.Add(stat.GetString(0).ToString());
			}
			stat.Close();
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
			//Initialize the FrameGraber event
			System.Windows.Forms.Application.Idle += new EventHandler(FrameGrabber);

		}

		private void button1_Click(object sender, EventArgs e)
		{
			bd.openConnection();
			string sql = "INSERT INTO `recognition` (`id_recognition`, `status`) " +
											   "VALUES (NULL, @status)";
			MySqlCommand command = new MySqlCommand(sql, bd.getConnection());
			command.Parameters.Add("@status", MySqlDbType.VarChar, 45).Value = addGroup.Text;
			int cmd = command.ExecuteNonQuery();
		}


		private void button2_Click(object sender, EventArgs e)
		{
			bd.openConnection();
			if (string.IsNullOrEmpty(namePerson.Text))
			{
				MessageBox.Show("Имя не введено");
			}
			else
			{
				if (string.IsNullOrEmpty(genderPerson.Text))
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
							try
							{
								ContTrain = ContTrain + 1;
								gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
								//Face Detector
								MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
								face,
								1.2,
								10,
								Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
								new Size(20, 20));
								foreach (MCvAvgComp f in facesDetected[0])
								{
									TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
									break;
								}
								TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
								trainingImages.Add(TrainedFace);
								labels.Add(namePerson.Text);
								facePerson.Image = TrainedFace;

								MessageBox.Show(namePerson.Text + " добавлен", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							catch
							{
								MessageBox.Show("No face detected. Please check your camera or stand closer.", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							}
							string sql = "INSERT INTO `person` (`id_person`, `fio_person`, `age`, `gender`, `face_pic`, `recognition_id_recognition`) " +
											   "VALUES (NULL, @fio, @age, @gender, @picture, @id_rec)";
							MySqlCommand command = new MySqlCommand(sql, bd.getConnection());
							command.Parameters.Add("@fio", MySqlDbType.VarChar, 45).Value = namePerson.Text;
							command.Parameters.Add("@age", MySqlDbType.Int64, 11).Value = agePerson.Text;
							command.Parameters.Add("@gender", MySqlDbType.VarChar, 10).Value = genderPerson.Text;
							command.Parameters.Add("@picture", MySqlDbType.MediumBlob).Value = imageToByteArray(TrainedFace.Bitmap);
							command.Parameters.Add("@id_rec", MySqlDbType.Int64, 11).Value = groupPerson.SelectedIndex-1;
							int cmd = command.ExecuteNonQuery();
						}
					}
				}
			}
			bd.closeConnection();
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
					name = recognizer.Recognize(result);
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
