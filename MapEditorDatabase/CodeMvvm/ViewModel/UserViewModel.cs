using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using CodeMvvm.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Linq;
using System.IO;
using System.ComponentModel;

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
		public User LoginUserMap
		{
			get
			{
				return User;
			}

			set
			{
				if (User != value)
				{
					User = value;
					RaisePropertyChanged("UserMap");
				}
			}
		}
		public string UserExist
		{
			get
			{
				return UserInFile;
			}

			set
			{

				if (UserInFile != value)
				{
					UserInFile = value;
					RaisePropertyChanged("UserExist");
				}
			}
		}
		#endregion
		#region Private fields
		private User User;
		private UserColletction _usersMap;
		private LinqToSQLClassesDataContext _db;
		private string UserInFile;
		#endregion
		#region Commands
		public ICommand ExitProgramCommand
		{
			get; private set;
		}
		public ICommand WindowClosing
		{
			get
			{
				return new RelayCommand<CancelEventArgs>(
					(args) => {
						Exit();
					});
			}
		}

		public ICommand CancelCommand
		{
			get; private set;
		}
		
		#endregion
		public UserViewModel()
		{
			
		
		}
		~UserViewModel()
		{
			LoginUserMap.IsUserOn = 0;
			_db.SubmitChanges();
		}
		public void InitUserViewModel(LinqToSQLClassesDataContext db)
		{
			User = new User();
			User.IsUserOn = 0;
			UsersMap = new UserColletction();
			_db = db;
			ReadFileUser();
			UsersMap = UsersMap.GetData(_db);

			
			CreateCommands();
			
			
		}
		private void ReadFileUser()
		{
			FileInfo file = new FileInfo("User.txt");
			StreamReader reader = file.OpenText();
			string text = reader.ReadLine();
			if (text != null)
			{
				
				UserExist = "Hidden";
				LoginUserMap.Name = text;
				UsersMap.Username = LoginUserMap.Name;
				LoginUserMap.Id = Int32.Parse(reader.ReadLine());
				_db.Users.Attach(LoginUserMap);
				
				LoginUserMap.IsUserOn = 1;
				_db.SubmitChanges();
				//_db.Refresh(RefreshMode.OverwriteCurrentValues, LoginUserMap);

			}
			else {
				
				UserExist = "visible";
			}
		}

		private void CreateCommands()
		{
			ExitProgramCommand = new RelayCommand(Exit);


			CancelCommand = new RelayCommand(UpdateList);

		}

		private void Exit()
		{
			LoginUserMap.IsUserOn = 0;
			_db.SubmitChanges();
		}
		private void MainWindowDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Exit();
		}
		private void UpdateList()
		{
			throw new NotImplementedException();
		}

		private void Save()
		{
		
			if (LoginUserMap.Name == null)
			{
				Console.WriteLine("feltet kan ikke være tomt");
				
			}
			else
			{
				Console.WriteLine(LoginUserMap.Name + LoginUserMap.Name.Length);
				LoginUserMap.IsUserOn = 1;
				_db.Users.InsertOnSubmit(LoginUserMap);
				try
				{
					
					_db.SubmitChanges();
				}
				catch (Exception e)
				{
					Console.WriteLine("Brukeren eksisterer allerede" + e);
				}
				finally
				{
					UserExist = "Hidden";
					//System.IO.StreamWriter file = new System.IO.StreamWriter("User.txt");
					File.AppendAllText("User.txt", string.Format("{0}{1}{2}", LoginUserMap.Name, Environment.NewLine, LoginUserMap.Id));
					_db.Users.Attach(LoginUserMap);
					//file.Write(LoginUserMap.Name + "/n" + LoginUserMap.Id);
					//file.Close();
				}
			}
		}
	}
}
