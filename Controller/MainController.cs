using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Controller
{
	public class MainController
	{
		private const string InvalidFormatErrorMessage = "Error occured while choosing file";
		private const string SaveErrorMessage = "Please enter correct data";
		private const string RussionPostURL = @"https://www.pochta.ru/";

		public delegate void Notifier(string message);

		private readonly Notifier notifier;
		private readonly Recognizer recognizer;
		public bool IsValid = false;

		public MainController(Notifier notifier)
		{
			this.notifier = notifier;

			recognizer = new Recognizer();
			recognizer.RecognitionFinished += Recognizer_RecognitionFinished;
		}

		private void Recognizer_RecognitionFinished(string jsonPath)
		{
			throw new NotImplementedException();
		}

		public BitmapImage ChooseImage()
		{
			var path = ChoosePath();

			if (path is null)
				return null;

			Task.Run(() => recognizer.RecognizeId(path));

			return new BitmapImage(new Uri(path));
		}

		private string ChoosePath()
		{
			var fileDialog = new OpenFileDialog
			{
				Filter = "PNG|*.png| JGP|*.jpg"
			};

			if (fileDialog.ShowDialog() is true)
			{
				return fileDialog.FileName;
			}

			Notify(InvalidFormatErrorMessage);
			return null;
		}

		public void SaveFile()
		{
			if (IsValid == false)
			{
				Notify(SaveErrorMessage);
				return;
			}
		}

		private void Notify(string message)
		{
			notifier?.Invoke(message);
		}

		public void Navigate()
		{
			Task.Run(() => Process.Start(RussionPostURL));
		}
	}
}
