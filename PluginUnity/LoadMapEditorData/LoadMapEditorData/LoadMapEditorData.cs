using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Data.SqlClient;

namespace LoadMapEditorData
{
    public class LoadMapEditorData
    {
		public List<int> Id;
		public List<int> PositionX;
		public List<int> PositionY;
		public List<string> Path;
		public List<int> RotationAngle;
		public List<bool> TopLeft;
		public List<bool> TopMiddle;
		public List<bool> TopRight;
		public List<bool> RightMiddle;
		public List<bool> BottomRight;
		public List<bool> BottomMiddle;
		public List<bool> BottomLeft;
		public List<bool> LeftMiddle;

		public LoadMapEditorData()
		{
			
		}
		public string GetDataFromSQL()
		{

			SqlConnection sqlConnection = new SqlConnection("Data Source=192.168.1.242\\(localdb)\\MSSQLLocalDB; Initial Catalog=MapEditor; User=Admin; Password=123;");
			sqlConnection.Open();

			SqlCommand cmd = sqlConnection.CreateCommand();

			cmd.CommandText = "Select Path From Tiles where id = 1";

			SqlDataReader read = cmd.ExecuteReader();

			read.Read();

			string path = (string)read["Path"];

			return path;

		}
	}
}
