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
		MySqlConnection connection = new MySqlConnection("server=localhost;username=root;port=3306;password=;database=mydb");

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
