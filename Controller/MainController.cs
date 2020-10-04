using Microsoft.Win32;
using Newtonsoft.Json;
using RecognizerDLL.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Controller
{
	public class MainController : ControllerBase
	{
		private const string ScanFileName = "scan123456789987654321.png";
		private const string InvalidFormatErrorMessage = "Возникла ошибка во время выбора файла";
		private const string SaveErrorMessage = "Нечего сохранять!";
		private const string AccessErrorMessage = "Невозможно получить доступ к файлу!";

		private const string RussionPostURL = "https://www.pochta.ru";
		private const string JsonExtension = ".json";
		private const bool DebugMode = false;

		public delegate void Notifier(string message);

		private readonly Notifier notifier;
		private readonly Dispatcher dispatcher;
		private readonly Recognizer recognizer;

		public event Action IdDropped;
		public event Action<BitmapImage> ImageSelected;

		public bool IsDataValid => isNameValid && isSecondNameValid && isMiddleNameValid
											&& isSeriesNameValid && isExpirationValid
											&& isCodeValid && isNumberValid && isBirthValid;
		private bool isNameValid = false;
		private bool isSecondNameValid = false;
		private bool isMiddleNameValid = false;
		private bool isSeriesNameValid = false;
		private bool isCodeValid = false;
		private bool isNumberValid = false;
		private bool isExpirationValid = false;
		private bool isBirthValid = false;

		public Visibility AnimationVisibility
		{
			get => animationVisibility;

			private set
			{
				animationVisibility = value;

				NotifyPropertyChanged("AnimationVisibility");
			}
		}

		private PersonId person;
		private string tempPath;
		private Visibility animationVisibility = Visibility.Hidden;

		public PersonId Person
		{
			get => person;
			private set
			{
				person = value;

				NotifyPropertyChanged("Person");
			}
		}

		public MainController(Notifier notifier, Dispatcher dispatcher)
		{
			this.notifier = notifier;
			this.dispatcher = dispatcher;
			recognizer = new Recognizer();
			recognizer.RecognitionFinished += Recognizer_RecognitionFinished;

			Task.Run(() => RemoveTempData());
		}

		public void ChooseScan()
		{
			Anumate(Visibility.Visible);

			DropFields();

			var path = @"D:\repos\IdRecognizer\IdRecognizer\PythonScripts\untitled0.exe";
			var scanProcess = Process.Start(new ProcessStartInfo(path)
			{
				UseShellExecute = false,
				CreateNoWindow = !DebugMode
			});

			Task.Run(() => WaitCompletionTask(scanProcess));
		}

		private void WaitCompletionTask(Process scanProcess)
		{
			scanProcess.WaitForExit();

			tempPath = GetPath();

			ImageSelected?.Invoke(StartRecognision(tempPath));
		}

		private string GetPath()
		{
			return Path.GetTempPath() + ScanFileName;
		}

		private void Recognizer_RecognitionFinished(string jsonPath)
		{
			if(jsonPath is null == false)
			{
				var jsonStr = File.ReadAllText(jsonPath);

				Person = JsonConvert.DeserializeObject<PersonId>(jsonStr);
				Person.ValidationChaged += Person_ValidationChaged;
			}

			Anumate(Visibility.Hidden);
		}

		private void RemoveTempData()
		{
			if (tempPath is null)
				return;

			try
			{
				File.Delete(tempPath);
			}
			catch (IOException ioe)
			{
				Notify(AccessErrorMessage);
				Console.WriteLine(ioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			tempPath = null;
		}

		private void Person_ValidationChaged(string propName, bool isValidFlag)
		{
			switch (propName)
			{
				case PersonId.NameProp:
					isNameValid = isValidFlag;
					break;

				case PersonId.SecondNameProp:
					isSecondNameValid = isValidFlag;
					break;

				case PersonId.MiddleNameProp:
					isMiddleNameValid = isValidFlag;
					break;

				case PersonId.SeriesProp:
					isSeriesNameValid = isValidFlag;
					break;

				case PersonId.CodeProp:
					isCodeValid = isValidFlag;
					break;

				case PersonId.NumberProp:
					isNumberValid = isValidFlag;
					break;

				case PersonId.ExpirationProp:
					isExpirationValid = isValidFlag;
					break;

				case PersonId.BirthProp:
					isBirthValid = isValidFlag;
					break;

				default:
					throw new ArgumentException($"Property name was {propName}");
			}

			NotifyPropertyChanged("IsDataValid");
		}

		public BitmapImage ChooseImage()
		{
			var path = ChoosePath();

			if (path is null)
				return null;

			Anumate(Visibility.Visible);

			return StartRecognision(path);
		}

		private void Anumate(Visibility visibility)
		{
			dispatcher?.Invoke(() => AnimationVisibility = visibility);
		}

		private BitmapImage StartRecognision(string path)
		{
			Task.Run(() => TryRun(path));

			return dispatcher?.Invoke(() => new BitmapImage(new Uri(path)).Clone());
		}

		private void TryRun(string path)
		{
			try
			{
				recognizer.RecognizeId(path);
			}
			catch (Exception e)
			{
				Notify(e.Message);
			}
		}

		public void OutError(ValidationErrorEventArgs e)
		{
			if (e.Error.Exception is null == false)
				Notify(e.Error.Exception.Message);
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

			DropFields();
		}

		private void DropFields()
		{
			isNameValid = false;
			isSecondNameValid = false;
			isMiddleNameValid = false;
			isSeriesNameValid = false;
			isCodeValid = false;
			isNumberValid = false;
			isExpirationValid = false;
			isBirthValid = false;
			NotifyPropertyChanged("IsDataValid");

			Person = null;

			RemoveTempData();

			IdDropped?.Invoke();
		}

		private void SaveJson(string fileName)
		{
			var jsonStr = JsonConvert.SerializeObject(Person);

			File.WriteAllText(fileName, jsonStr);
		}

		private void Notify(string message)
		{
			dispatcher?.Invoke(() => notifier?.Invoke(message));
		}

		public void Navigate()
		{
			Process.Start(new ProcessStartInfo(RussionPostURL)
			{
				UseShellExecute = true,
			});
		}
	}
}
