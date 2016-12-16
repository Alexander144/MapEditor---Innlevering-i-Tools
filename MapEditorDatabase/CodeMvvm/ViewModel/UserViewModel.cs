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
using System.Threading;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Collections.Specialized;

namespace CodeMvvm.ViewModel
{
	public class UserViewModel : ViewModelBase
	{
        /*
        
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
		private LinqToSQLClassesDataContext _usersDB;
		private string UserInFile;
		private DispatcherTimer dispatcherTimer;
		#endregion
		#region Commands
		public ICommand ExitProgramCommand
		{
			get; private set;
		}
		public ICommand SaveUserCommand
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

		public ICommand CancelUserCommand
		{
			get; private set;
		}

		#endregion

		
		public UserViewModel()
		{
			dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(UpdateData);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
			dispatcherTimer.Start();

		}

		private void UpdateData(object sender, EventArgs e)
		{
			
			_db.Refresh(RefreshMode.OverwriteCurrentValues, UsersMap);
			CollectionViewSource.GetDefaultView(UsersMap).Refresh();
			UsersMap.RefreshData();
			UsersMap.AddNewData();
		}

		public void InitUserViewModel(LinqToSQLClassesDataContext db)
		{
			User = new User();
			User.IsUserOn = 0;
			UsersMap = new UserColletction();
			_usersDB = db;
			ReadFileUser();
<<<<<<< HEAD
			UsersMap = UsersMap.GetData(_usersDB);

=======
			UsersMap.GetData(_db);
		
>>>>>>> b0d7646403abb3ebdd6bfac0ae6397591f30d018
			
			CreateCommands();
			
			
		}
		private void ReadFileUser()
		{

			FileInfo file = new FileInfo("User.txt");
			if (file.Exists)
			{
				StreamReader reader = file.OpenText();
				string text = reader.ReadLine();
				if (text != null)
				{

					UserExist = "Hidden";
					LoginUserMap.Name = text;
					UsersMap.Username = LoginUserMap.Name;
					LoginUserMap.Id = Int32.Parse(reader.ReadLine());
					_usersDB.Users.Attach(LoginUserMap);

					LoginUserMap.IsUserOn = 1;
					_db.SubmitChanges();



					//_db.Refresh(RefreshMode.OverwriteCurrentValues, LoginUserMap);

				}
			}
			else {

				UserExist = "visible";
			}
		}

		private void CreateCommands()
		{
			ExitProgramCommand = new RelayCommand(Exit);

			SaveUserCommand = new RelayCommand(SaveUsers);
			CancelUserCommand = new RelayCommand(UpdateList);

		}

		private void Exit()
		{
			LoginUserMap.IsUserOn = 0;
			_usersDB.SubmitChanges();
		}
		private void UpdateList()
		{
			throw new NotImplementedException();
		}

		private void SaveUsers()
		{

            Thread saveThread = new Thread(new ThreadStart(SaveUsersThread));
            saveThread.SetApartmentState(ApartmentState.STA);
            saveThread.IsBackground = true;
            saveThread.Start();

		}


        private void SaveUsersThread()
        {

            if (LoginUserMap.Name == null)
            {
                Console.WriteLine("feltet kan ikke være tomt");

            }
            else
            {
                Console.WriteLine(LoginUserMap.Name + LoginUserMap.Name.Length);
                bool ErrorSendDatabase = false;
                LoginUserMap.IsUserOn = 1;
                _usersDB.Users.InsertOnSubmit(LoginUserMap);
                try
                {

                    _usersDB.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Brukeren eksisterer allerede" + e);
                    ErrorSendDatabase = true;
                }
                finally
                {
                    if (!ErrorSendDatabase)
                    {
                        UserExist = "Hidden";
                        //System.IO.StreamWriter file = new System.IO.StreamWriter("User.txt");
                        File.AppendAllText("User.txt", string.Format("{0}{1}{2}", LoginUserMap.Name, Environment.NewLine, LoginUserMap.Id));
                        //_db.Users.Attach(LoginUserMap);
                        //file.Write(string.Format("{0}{1}{2}", LoginUserMap.Name, Environment.NewLine, LoginUserMap.Id));
                        //file.Close();
                    }
                }
            }
        }

    */
        
	}
}
