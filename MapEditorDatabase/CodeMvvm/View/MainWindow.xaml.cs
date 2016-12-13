using System.Windows;
using CodeMvvm.ViewModel;
using CodeMvvm;

namespace MVVM_Light_eksempel
{
	/// <summary>
	/// This application's main window.
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow() {
			InitializeComponent();
			Closing += (s, e) => ViewModelLocator.Cleanup();
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}