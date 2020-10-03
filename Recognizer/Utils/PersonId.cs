using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace RecognizerDLL.Utils
{
	public partial class PersonId
	{
		private string code;
		private string expiration;
		private string birth;
		private string gender;
		private string number;
		private string series;
		private string nation;
		private string middleName;
		private string secondName;
		private string name;

		/// <summary>
		/// Имя
		/// </summary>
		public string Name
		{
			get => name;
			set
			{
				name = value;
				NotifyPropertyChanged("Name");
			}
		}

		/// <summary>
		/// Фамилия
		/// </summary>
		public string SecondName
		{
			get => secondName; set
			{
				secondName = value;
				NotifyPropertyChanged("SecondName");
			}
		}

		/// <summary>
		/// Отчетво
		/// </summary>
		public string MiddleName
		{
			get => middleName; set
			{
				middleName = value;
				NotifyPropertyChanged("MiddleName");
			}
		}

		/// <summary>
		/// Национальность
		/// </summary>
		public string Nation
		{
			get => nation;
			set
			{
				nation = value;
				NotifyPropertyChanged("Nation");
			}
		}

		/// <summary>
		/// Номер паспорта
		/// </summary>
		public string Series
		{
			get => series;

			set
			{
				series = value;

				NotifyPropertyChanged("Series");
			}
		}

		/// <summary>
		/// Номер паспорта
		/// </summary>
		public string Number
		{
			get => number;

			set
			{
				number = value;

				NotifyPropertyChanged("Number");
			}
		}

		/// <summary>
		/// Пол
		/// </summary>
		public string Gender
		{
			get => gender;

			set
			{
				gender = value;

				NotifyPropertyChanged("Gender");
			}
		}

		/// <summary>
		/// In format YY/MM/DD
		/// Дата рождения
		/// </summary>
		public string Birth
		{
			get => birth;

			set
			{
				birth = value;

				NotifyPropertyChanged("Birth");
			}
		}

		/// <summary>
		/// In format YY/MM/DD
		/// Дата вылачи паспорта
		/// </summary>
		public string Expiration
		{
			get => expiration;
			set
			{
				expiration = value;

				NotifyPropertyChanged("Expiration");
			}
		}

		/// <summary>
		/// Код подразделения выдачи
		/// </summary>
		public string Code
		{
			get => code;
			set
			{
				code = value;

				NotifyPropertyChanged("Code");
			}
		}
	}

	public partial class PersonId : IDataErrorInfo
	{
		private const string InvalidNameMessage = "Неверное имя! Имя не может содержать цифр!";
		private const string InvalidSeriesMessage = "Неверная серия паспорта! Это должно быть 4х значеное число!";
		private const string InvalidNumberMessage = "Неверный номер паспорта! Это должно быть 6ти значное число!";

		public string this[string columnName]
		{
			get
			{
				string error = string.Empty;
				switch (columnName)
				{
					case "Name":
						if (DataValidation.IsNameValid(Name) == false)
							error = InvalidNameMessage;
						break;

					case "SecondName":
						if (DataValidation.IsNameValid(SecondName) == false)
							error = InvalidNameMessage;
						break;

					case "MiddleName":
						if (DataValidation.IsNameValid(MiddleName) == false)
							error = InvalidNameMessage;
						break;

					case "Series":
						if (DataValidation.IsSeriesValid(Series) == false)
							error = InvalidSeriesMessage;
						break;

					case "Code":
					case "Number":
						if (DataValidation.IsNumberValid(Number) == false)
							error = InvalidNumberMessage;
						break;

					case "Expiration":
					case "Birth":
						break;

					default:
						throw new ArgumentException($"Column name was {columnName}");

				}

				Error = error;
				return error;
			}
		}

		[JsonProperty("Error", NullValueHandling = NullValueHandling.Ignore)]
		public string Error { get; private set; }

	}

	public partial class PersonId : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}
}
