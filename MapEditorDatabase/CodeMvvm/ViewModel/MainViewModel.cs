using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CodeMvvm.Model;
using System;
using System.Data.Linq;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CodeMvvm.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		#region Properties
		public TileArray FrontTiless
		{
			get
			{
				return ArrayforTiles;
			}

			set
			{
				if (ArrayforTiles != value)
				{
				
					ArrayforTiles = value;
					RaisePropertyChanged("FrontTiless");
				}
			}
		}

		/*public int TileId {
	get {
		return ArrayforTiles[0].Id;
	}

	set {
		if (ArrayforTiles[0].Id != value) {
			ArrayforTiles[0].Id = value;
			RaisePropertyChanged();
		}
	}
}
public int TileposX
{
	get
	{
		return (int)ArrayforTiles[0].posX;
	}

	set
	{
		if (ArrayforTiles[0].posX != value)
		{
			ArrayforTiles[0].posX = value;
			RaisePropertyChanged();
		}
	}
}
public int TileposY
{
	get
	{
		return (int)ArrayforTiles[0].posY;
	}

	set
	{
		if (ArrayforTiles[0].posY != value)
		{
			ArrayforTiles[0].posY = value;
			RaisePropertyChanged();
		}
	}
}
*/
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

		private TileArray ArrayforTiles;
		private MapEditorLinq db;

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
			db = new MapEditorLinq();
			FrontTiless = new TileArray();
			FrontTiless.GetData(db);
		}

		private void CreateCommands() {
			SaveCommand = new RelayCommand(Save, CanSave);
			
			CancelCommand = new RelayCommand(Cancel);
		}

		private bool CanSave() {
			return (FrontTiless != null);
		}

		public void Save() {
			FrontTiless.Save();
		}

		public void Cancel() {
			db.Refresh(RefreshMode.OverwriteCurrentValues, FrontTiless);
			FrontTiless.GetData(db);

			//ArrayforTiles.Clear();
			//FrontTiless.posX = ArrayforTiles[0].posX;
			//var lastSavedCharacter = new Tile();
			
			//TileId = lastSavedCharacter.Id;
			//Race = lastSavedCharacter.Race;
			// TODO: Add more "reset fields" code here.
		}
	}
}