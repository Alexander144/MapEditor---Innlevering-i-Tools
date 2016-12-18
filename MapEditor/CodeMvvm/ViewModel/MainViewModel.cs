using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CodeMvvm.Model;
using System;
using System.Data.Linq;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;

namespace CodeMvvm.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase, INotifyPropertyChanged
	{
		#region Properties

        //Tile properties
		public TileCollection TileMap
		{
			get
			{
				return _tileMap;
			}

			set
			{
				if (_tileMap != value)
				{
					_tileMap = value;
					RaisePropertyChanged("TileMap");
				}
			}
		}


        public Table<Tile> DBTiles
        {
            get
            {
                return _db.Tiles;
            }
        }

        //User properties
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

        #region Commands

        //General commands
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


        //Tile related commands
        public ICommand SaveTilesCommand {
			get
            {
                return new RelayCommand<CodeMvvm.View.Tile[,]>(SaveTiles);
            }
            set
            {

            }
        }

		public ICommand CancelCommand {
			get; private set;
		}

        //USer related commands
        public ICommand SaveUserCommand
        {
            get; private set;
        }

        public ICommand CancelUserCommand
        {
            get; private set;
        }

        #endregion

        #region Private fields

        //Tile related fields
        private TileCollection _tileMap;
		private LinqToSQLClassesDataContext _db;

        private int _tileMapNumberOfXNodes = 10;
        private int _tileMapNumberOfYNodes = 10;

        private int _tileColumnWidth = 50;
        private int _tileRowHeight = 50;

        //USer related fields
        private User User;
        private UserColletction _usersMap;
        private LinqToSQLClassesDataContext _usersDB;
        private string UserInFile;
		private DispatcherTimer dispatcherTimer;


		#endregion


		#region Methods


		//General methods
		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel() {
			_db = new LinqToSQLClassesDataContext();
			InitUserViewModel(_db);
			GetDataFromSQL();
			CreateCommands();
			dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler(UpdateData);
			dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
			dispatcherTimer.Start();
		}

		private void GetDataFromSQL()
		{
			TileMap = new TileCollection();
			TileMap.GetData(_db);
            bool tileMapIsEmpty = true;

            foreach (Tile t in TileMap)
            {
                
                if (t.Path != null)
                {
                    tileMapIsEmpty = false;
                    break;
                }
            }

            if (tileMapIsEmpty)
            {
                for (int i = 0; i < 100; i++)
                {
                    Tile t = new Tile();
                    t.Path = "pack://application:,,,/View/Sprites/defaultTile.png";
                    TileMap.Add(t);
                }
            }
		}

		private void CreateCommands() {
			SaveTilesCommand = new RelayCommand<CodeMvvm.View.Tile[,]>(SaveTiles);
            CancelCommand = new RelayCommand(LoadTiles);
            ExitProgramCommand = new RelayCommand(Exit);
            SaveUserCommand = new RelayCommand(SaveUsers);
            CancelUserCommand = new RelayCommand(UpdateList);

        }


        //Tile related methods
		private bool CanSave() {
			return (TileMap != null);
		}

		public void SaveTiles(CodeMvvm.View.Tile[,] tileDoubleArray) {

            Console.WriteLine("Saving tiles...");
            if (tileDoubleArray == null)
            {
                Console.WriteLine("Tile array is null...");
            }

            for (int i = 0; i < _tileMapNumberOfXNodes; i++)
            {
                for (int j = 0; j < _tileMapNumberOfYNodes; j++)
                {
                    Console.WriteLine(tileDoubleArray[i, j].Path);
                    Console.WriteLine("Hei");
                }
            }


            Thread saveThread = new Thread(() => SaveTilesThread(tileDoubleArray));
            saveThread.SetApartmentState(ApartmentState.STA);
            saveThread.IsBackground = true;
            saveThread.Start();
        }

		public void LoadTiles() {
            Thread loadThread = new Thread(new ThreadStart(LoadTilesThread));
            loadThread.SetApartmentState(ApartmentState.STA);
            loadThread.IsBackground = true;
            loadThread.Start();
		}

        public void LoadTilesThread()
        {
            _db.Refresh(RefreshMode.OverwriteCurrentValues, TileMap);
            TileMap.GetData(_db);
            System.Windows.Threading.Dispatcher.Run();
        }

        public void SaveTilesThread(CodeMvvm.View.Tile[,] tileDoubleArray)
        {
            UpdateTileMap(tileDoubleArray);
            TileMap.Save();
            System.Windows.Threading.Dispatcher.Run();
        }

		public void UpdateTileMap(CodeMvvm.View.Tile[,] tileDoubleArray)
		{



			foreach (Tile T in _db.Tiles)
			{
				TileMap.Add(T);
			}


			int i = 0;
			int j = 0;
			
            foreach(Tile U in TileMap)
            {
                U.Path = tileDoubleArray[i, j].Path;
                U.PositionX = tileDoubleArray[i, j].PositionX / _tileColumnWidth;
                U.PositionY = tileDoubleArray[i, j].PositionY /_tileRowHeight;
                U.RotationAngle = (int) tileDoubleArray[i, j].RotationAngle % -360;
                U.TopLeft = tileDoubleArray[i, j].TopLeft;
                U.TopMiddle = tileDoubleArray[i, j].TopMiddle;
                U.TopRight = tileDoubleArray[i, j].TopRight;
                U.LeftMiddle = tileDoubleArray[i, j].LeftMiddle;
                U.RightMiddle = tileDoubleArray[i, j].RightMiddle;
                U.BottomLeft = tileDoubleArray[i, j].BottomLeft;
                U.BottomMiddle = tileDoubleArray[i, j].BottomMiddle;
                U.BottomRight = tileDoubleArray[i, j].BottomRight;

                if (i == _tileMapNumberOfXNodes - 1 && j == _tileMapNumberOfYNodes - 1)
                {
                    break;
                }

                if (j < _tileMapNumberOfYNodes - 1)
                {
                    j++;
                } else
                {
                    i++;
                    j = 0;
                }
               
            }
			 _db.SubmitChanges();
			
		}


        //User related methods

        public void InitUserViewModel(LinqToSQLClassesDataContext db)
        {
            User = new User();
            User.IsUserOn = 0;
            UsersMap = new UserColletction();
            _usersDB = db;
            ReadFileUser();
			TilesHaveValues();
            UsersMap.GetData(_usersDB);


            CreateCommands();


        }

		private void TilesHaveValues()
		{
			var count = _db.Tiles.Where(me => me.Id == 1).Count();
			if (count == 0)
			{
				for (int d = 0; d < 100; d++)
				{
					Tile l = new Tile();
					l.Id = d;
					_db.Tiles.InsertOnSubmit(l);
					
				}
				_db.SubmitChanges();
			}
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

                }
            }
            else
            {

                UserExist = "visible";
            }
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

		
		private void UpdateData(object sender, EventArgs e)
		{

            Thread saveThread = new Thread(new ThreadStart(UpdateDataThread));
            saveThread.SetApartmentState(ApartmentState.STA);
            saveThread.IsBackground = true;
            saveThread.Start();
	
 		}

        private void UpdateDataThread()
        {
            CollectionViewSource.GetDefaultView(UsersMap).Refresh();
            UsersMap.RefreshData();
            UsersMap.AddNewData();
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
                        File.AppendAllText("User.txt", string.Format("{0}{1}{2}", LoginUserMap.Name, Environment.NewLine, LoginUserMap.Id));
                    }
                }
            }
        }
        #endregion


    }
}