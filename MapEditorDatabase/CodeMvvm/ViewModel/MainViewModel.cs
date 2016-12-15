using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CodeMvvm.Model;
using System;
using System.Data.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace CodeMvvm.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : UserViewModel, INotifyPropertyChanged
	{
		#region Properties
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

        public Tile[,] TileMapDoubleArray
        {
            get
            {
                return _tileMapDoubleArray;
            }

            set
            {
                if (_tileMapDoubleArray != value)
                {
                    _tileMapDoubleArray = value;
                    RaisePropertyChanged("TileMapDoubleArray");
                }
            }
        }
		#endregion

		#region Commands

		public ICommand SaveCommand {
			get; private set;
		}

		public ICommand CancelCommand {
			get; private set;
		}

		#endregion

		#region Private fields

		private TileCollection _tileMap;
        private Tile[,] _tileMapDoubleArray;
		private LinqToSQLClassesDataContext _db;

        private int _tileMapNumberOfXNodes = 10;
        private int _tileMapNumberOfYNodes = 10;

        private int _tileColumnWidth = 50;
        private int _tileRowHeight = 50;


        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {
			_db = new LinqToSQLClassesDataContext();
			InitUserViewModel(_db);
			GetDataFromSQL();
			CreateCommands();
        }

		private void GetDataFromSQL()
		{
			TileMap = new TileCollection();
			TileMap.GetData(_db);
		}

		private void CreateCommands() {
			SaveCommand = new RelayCommand(Save, CanSave);
			CancelCommand = new RelayCommand(Load);
		}

		private bool CanSave() {
			return (TileMap != null);
		}

		public void Save() {
            Thread saveThread = new Thread(new ThreadStart(SaveThread));
            saveThread.SetApartmentState(ApartmentState.STA);
            saveThread.IsBackground = true;
            saveThread.Start();
        }

		public void Load() {
            Thread loadThread = new Thread(new ThreadStart(LoadThread));
            loadThread.SetApartmentState(ApartmentState.STA);
            loadThread.IsBackground = true;
            loadThread.Start();
		}

        public void LoadThread()
        {
            _db.Refresh(RefreshMode.OverwriteCurrentValues, TileMap);
            TileMap.GetData(_db);
            System.Windows.Threading.Dispatcher.Run();
        }

        public void SaveThread()
        {
            UpdateTileMap();
            TileMap.Save();
            System.Windows.Threading.Dispatcher.Run();
        }

        public void UpdateTileMap()
        {
            foreach (Tile t in _tileMap)
            {
                _tileMap.Remove(t);
            }
            for (int i = 0; i < _tileMapNumberOfXNodes; i++)
            {
                for (int j = 0; j < _tileMapNumberOfYNodes; j++)
                {
                    _tileMap.Add(_tileMapDoubleArray[i, j]);
                }
            }
        }
	}
}