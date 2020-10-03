using Controller;
using System.Windows;
using System.Windows.Input;

namespace IdRecognizer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainController mainController;

		public MainWindow()
		{
			InitializeComponent();

			mainController = new MainController((message) => MessageBox.Show(message));
		}

		private void OpenFileButton_Click(object sender, RoutedEventArgs e)
		{
			IdScanImage.Source =  mainController.ChooseImage();
		}

		private void ScanButton_Click(object sender, RoutedEventArgs e)
		{

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
	}
}
