using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMvvm.Model
{
	public class UserColletction : ObservableCollection<User>
	{
		LinqToSQLClassesDataContext l_db;
		public string Username { get; set; }
		public UserColletction()
		{

		}
		public void GetData(LinqToSQLClassesDataContext db)
		{
			l_db = db;
			foreach (User T in l_db.Users)
			{
				if (T.Name != Username && T.IsUserOn == 1 && !Items.Contains(T))
				{
					Add(T);
				}
			}

		}
		public void RefreshData()
		{
			foreach (User T in Items)
			{
				if (T.IsUserOn == 0)
				{
					Remove(T);
					break;
				}
			}
		}
		public void AddNewData()
		{
		

		}
	}
}
