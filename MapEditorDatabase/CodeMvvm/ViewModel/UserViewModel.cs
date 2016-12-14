using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using CodeMvvm.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMvvm.ViewModel
{
	public class UserViewModel : ViewModelBase
	{
		#region Properties
		public UserColletction UsersMap
		{
			get
			{
				return _usersMap;
			}

			set
			{
				if (_usersMap != value)
				{
					_usersMap = value;
					RaisePropertyChanged("UsersMap");
				}
			}
		}
		#endregion
		#region Private fields
		private User User;
		private UserColletction _usersMap;
		private LinqToSQLClassesDataContext _db;
		#endregion
		#region Commands

		public ICommand SaveUserCommand
		{
			get; private set;
		}

		public ICommand CancelCommand
		{
			get; private set;
		}

		#endregion
		public UserViewModel()
		{
			ReadFileUser();

			_db = new LinqToSQLClassesDataContext();
			UsersMap = new UserColletction();
			UsersMap = UsersMap.GetData(_db);
		}

		private void ReadFileUser()
		{
			bool UserExist = true;


			if (UserExist)
			{

			}
			else
			{

			}
		}

		private void CreateCommands()
		{
			SaveUserCommand = new RelayCommand(Save);

			CancelCommand = new RelayCommand(UpdateList);
		}

		private void UpdateList()
		{
			throw new NotImplementedException();
		}

		private void Save()
		{
			throw new NotImplementedException();
		}
	}
}
