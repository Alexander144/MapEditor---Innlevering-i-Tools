using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;

namespace CodeMvvm.Model
{
	public class TileCollection:ObservableCollection<Tile>
	{
		private LinqToSQLClassesDataContext l_db;
		public TileCollection() {
			
		}
		public void GetData(LinqToSQLClassesDataContext db)
		{
			l_db = db;
			
			foreach (Tile T in l_db.Tiles)
			{
				Add(T);
				
			}
			
		}

		public void AttachToDatabase()
		{
			foreach (Tile T in this.Items)
			{
				l_db.Tiles.Attach(T);
				
			}
			
		}
		public void Save()
		{
			l_db.SubmitChanges();
		}
	}
}
