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

namespace CodeMvvm.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : UserViewModel
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
					RaisePropertyChanged("_tileMap");
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
		private LinqToSQLClassesDataContext _db;
	

		#endregion

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel() {
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
			
			CancelCommand = new RelayCommand(Cancel);
		}

		private bool CanSave() {
			return (TileMap != null);
		}

		public void Save() {
			TileMap.Save();
		}

		public void Cancel() {
			_db.Refresh(RefreshMode.OverwriteCurrentValues, TileMap);
			TileMap.GetData(_db);
		}
	}
}