using Microsoft.Win32;
using Newtonsoft.Json;
using RecognizerDLL.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Controller
{
	public class MainController: ControllerBase
	{
		private const string InvalidFormatErrorMessage = "Возникла ошибка во время выбора файла";
		private const string SaveErrorMessage = "Нечего сохранять!";
		private const string RussionPostURL = @"https://www.pochta.ru/";
		private const string JsonExtension = ".json";

		public delegate void Notifier(string message);

		private readonly Notifier notifier;
		private readonly Recognizer recognizer;
		public bool IsValid = false;

		public SolidColorBrush InvalidBrush = new SolidColorBrush(Colors.Red);
		public SolidColorBrush ValidBrush = new SolidColorBrush(Colors.Transparent);

		private PersonId person;

		public PersonId Person
		{
			get => person;
			private set
			{
				person = value;

				NotifyPropertyChanged("Person");
			}
		}

		public MainController(Notifier notifier)
		{
			this.notifier = notifier;

			recognizer = new Recognizer();
			recognizer.RecognitionFinished += Recognizer_RecognitionFinished;
		}

		private void Recognizer_RecognitionFinished(string jsonPath)
		{
			var jsonStr = File.ReadAllText(jsonPath);

			Person = JsonConvert.DeserializeObject<PersonId>(jsonStr);
		}

		public BitmapImage ChooseImage()
		{
			var path = ChoosePath();

			if (path is null)
				return null;

			Task.Run(() => recognizer.RecognizeId(path));

			return new BitmapImage(new Uri(path));
		}

		public void OutError(TextBox currentTB, ValidationErrorEventArgs e)
		{
			if (e.Action == ValidationErrorEventAction.Added)
			{
				currentTB.BorderBrush = ValidBrush;
			}
			else
			{
				currentTB.BorderBrush = InvalidBrush;

				Notify(e.Error.Exception.Message);
			}
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
			if (Person is null)
			{
				Notify(SaveErrorMessage);
				return;
			}

			var dlg = new SaveFileDialog
			{
				DefaultExt = JsonExtension,
				FileName = Guid.NewGuid().ToString(),
			};

			var res = dlg.ShowDialog();
			if (res.HasValue && res.Value)
			{
				Task.Run(() => SaveJson(dlg.FileName));
			}
		}

		private void SaveJson(string fileName)
		{
			var jsonStr = JsonConvert.SerializeObject(Person);

			File.WriteAllText(fileName, jsonStr);
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
