using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;

namespace CodeMvvm.Model
{
	public class TileArray:ObservableCollection<Tile>
	{
		private MapEditorLinq l_db;
		public TileArray() {
			
		}
		public void GetData(MapEditorLinq db)
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
