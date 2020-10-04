using Microsoft.Win32;
using Newtonsoft.Json;
using RecognizerDLL.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Controller
{
	public class MainController : ControllerBase
	{
		private const string InvalidFormatErrorMessage = "Возникла ошибка во время выбора файла";
		private const string SaveErrorMessage = "Нечего сохранять!";
		private const string RussionPostURL = "https://www.pochta.ru";
		private const string GoogleURL = "http://www.google.com";
		private const string JsonExtension = ".json";

		public delegate void Notifier(string message);

		private readonly Notifier notifier;
		private readonly Recognizer recognizer;

		public event Action IdDropped;

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

		public void ChooseScan()
		{
			throw new NotImplementedException();
		}

		private void Recognizer_RecognitionFinished(string jsonPath)
		{
			var jsonStr = File.ReadAllText(jsonPath);

			Person = JsonConvert.DeserializeObject<PersonId>(jsonStr);
			Person.ValidationChaged += Person_ValidationChaged; ;
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

			Task.Run(() => recognizer.RecognizeId(path));

			return new BitmapImage(new Uri(path));
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

			IdDropped?.Invoke();
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
			Process.Start(new ProcessStartInfo(RussionPostURL)
			{
				UseShellExecute = true,
			});
		}
	}
}
