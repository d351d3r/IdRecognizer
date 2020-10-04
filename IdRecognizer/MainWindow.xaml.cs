using Controller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IdRecognizer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MainController mainController;

		public MainWindow()
		{
			InitializeComponent();

			mainController = new MainController((message) => MessageBox.Show(message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Error), 
												Dispatcher);

			mainController.ImageSelected += MainController_ImageSelected;
			mainController.IdDropped += MainController_IdDropped;
			DataContext = mainController;
		}

		private void MainController_ImageSelected(System.Windows.Media.Imaging.BitmapImage obj)
		{
			Dispatcher.Invoke(() => IdScanImage.Source = obj);
		}

		private void MainController_IdDropped()
		{
			IdScanImage.Source = null;
		}

		private void OpenFileButton_Click(object sender, RoutedEventArgs e)
		{
			IdScanImage.Source = mainController.ChooseImage();
		}

		private void ScanButton_Click(object sender, RoutedEventArgs e)
		{
			mainController.ChooseScan();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			mainController.SaveFile();
		}

		private void PostLogo_MouseDown(object sender, MouseButtonEventArgs e)
		{
			mainController.Navigate();
		}

		private void MainFocusGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			MainFocusGrid.Focus();
		}

		private void TextBox_Error(object sender, ValidationErrorEventArgs e)
		{
			if (sender is TextBox)
			{
				mainController.OutError(e);
			}
		}
	}
}
