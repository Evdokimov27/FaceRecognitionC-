using Gst;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
	internal class bd
	{
		MySqlConnection connection = new MySqlConnection("server=192.168.1.3;port=80;username=root;password=root;database=camera;");
		//MySqlConnection connection = new MySqlConnection("server=localhost;userid=root;password=;database=mydb");
		private static SshClient client = new SshClient("192.168.1.3", "pi", "root");
		private static MySqlConnectionStringBuilder connBuilderIMSoverSSH = new MySqlConnectionStringBuilder();
		public void openConnection()
		{
			connBuilderIMSoverSSH.Server = "localhost";     //Текуший ПК
			connBuilderIMSoverSSH.Port = 3306;               //Порт, который пробрасываем (Не обязательно такой. Можно пробросить любой свободный)
			connBuilderIMSoverSSH.UserID = "root";
			connBuilderIMSoverSSH.Password = "root";
			connBuilderIMSoverSSH.Database = "camera";

			if (!client.IsConnected)
			{
				client.Connect();
				//Объявляем и инициализируем пробрасываемый порт
				var portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
				client.AddForwardedPort(portForwarded);
				//Открываем порт
				portForwarded.Start();
				var sql = new MySqlConnection(connBuilderIMSoverSSH.ConnectionString);
			}

		}
		public MySqlConnection getConnection()
		{
			return connection;
		}

		public DataTable ExecuteSqlOverSSH(string query)
		{
			openConnection();
			DataTable table = new DataTable();
			//соединение с MySQL
			using (var sql = new MySqlConnection(connBuilderIMSoverSSH.ConnectionString))
			{
				sql.Open();
				var cmd = sql.CreateCommand();
				cmd.CommandTimeout = 600;
				cmd.CommandText = query;
				var rdr = cmd.ExecuteReader();
				table.Load(rdr);
				sql.Close();
			}
			return table;
		}

		public int NonExecuteSqlOverSSH(string query, MySqlParameter param)
		{
			
			openConnection();
			//соединение с MySQL
			using (var sql = new MySqlConnection(connBuilderIMSoverSSH.ConnectionString))
			{
				sql.Open();
				var cmd = sql.CreateCommand();
				cmd.CommandTimeout = 600;
				cmd.CommandText = query;
				if(param != null) cmd.Parameters.Add(param);
				var rdr = cmd.ExecuteNonQuery();
				sql.Close();
			}
			return 0;
		}

	}
}
