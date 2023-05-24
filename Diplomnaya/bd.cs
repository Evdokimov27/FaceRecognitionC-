using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFaceRec
{
	internal class bd
	{
		//MySqlConnection connection = new MySqlConnection("server=192.168.1.5;port=80;username=root;password=root;database=camera");
		MySqlConnection connection = new MySqlConnection("server=localhost;userid=root;password=;database=mydb");

		public void openConnection()
		{
			if (connection.State == System.Data.ConnectionState.Closed)
				connection.Open();
		}
		public void closeConnection()
		{
			if (connection.State == System.Data.ConnectionState.Open)
				connection.Close();
		}
		public MySqlConnection getConnection()
		{
			return connection;
		}

	}
}
