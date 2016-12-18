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
		public List<float> RotationAngle;
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
		public void GetDataFromSQL()
		{
			Id = new List<int>();
			PositionX = new List<int>();
			PositionY = new List<int>();
			Path = new List<string>();
			RotationAngle = new List<float>();
			TopLeft = new List<bool>();
			TopMiddle = new List<bool>();
			TopRight = new List<bool>();
			RightMiddle = new List<bool>();
			BottomRight = new List<bool>();
			BottomMiddle = new List<bool>();
			BottomLeft = new List<bool>();
			BottomRight = new List<bool>();

			SqlConnection sqlConnection = new SqlConnection("Data Source=mapeditortools.database.windows.net,1433; Network Library=DBMSSOCN; Initial Catalog=MapEditor; User=Tools; Password=Root123456789;");
			sqlConnection.Open();

			SqlCommand cmd = sqlConnection.CreateCommand();

			cmd.CommandText = "Select * From Tiles";

			SqlDataReader read = cmd.ExecuteReader();

			while (read.Read())
			{
				Id.Add((int)read["Id"]);
				PositionX.Add((int)read["PositionX"]);
				PositionY.Add((int)read["PositionY"]);
				Path.Add((string)read["Path"]);
				//RotationAngle.Add((float)read["RotationAngle"]);

			}
		}
	}
}
