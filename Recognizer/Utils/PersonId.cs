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
		private readonly string InvalidNameMessage = $"Неверное имя! {StandartErrorMessage}\nИмя не может содержать цифр!";
		private readonly string InvalidSeriesMessage = $"Неверная серия паспорта! {StandartErrorMessage}\nЭто должно быть 4-х значеное число!";
		private readonly string InvalidNumberMessage = $"Неверный номер паспорта! {StandartErrorMessage}\nЭто должно быть 6-ти значное число!";
		private const string StandartErrorMessage = "Пожалуйста, проверьке правильность!";
		private readonly string InvalidDateMessage = $"Неверная дата! {StandartErrorMessage}";

		public const string NameProp = "Name";
		public const string SecondNameProp = "SecondName";
		public const string MiddleNameProp = "MiddleName";
		public const string SeriesProp = "Series";
		public const string CodeProp = "Code";
		public const string NumberProp = "Number";
		public const string ExpirationProp = "Expiration";
		public const string BirthProp = "Birth";

		public delegate void ValidationChangedDelegate(string propName, bool isValidFlag);

		public event ValidationChangedDelegate ValidationChaged;

		public string this[string columnName]
		{
			get
			{
				string error = string.Empty;
				switch (columnName)
				{
					case NameProp:
						{
							var isValid = DataValidation.IsNameValid(Name);
							if (isValid == false)
								error = InvalidNameMessage;

							ValidationChaged?.Invoke(NameProp, isValid);
						}
						break;

					case SecondNameProp:
						{
							var isValid = DataValidation.IsNameValid(SecondName);
							if (isValid == false)
								error = InvalidNameMessage;

							ValidationChaged?.Invoke(SecondNameProp, isValid);
						}
						break;

					case MiddleNameProp:
						{
							var isValid = DataValidation.IsNameValid(MiddleName);
							if (isValid == false)
								error = InvalidNameMessage;

							ValidationChaged?.Invoke(MiddleNameProp, isValid);
						}
						break;

					case SeriesProp:
						{
							var isValid = DataValidation.IsSeriesValid(Series);
							if (isValid == false)
								error = InvalidSeriesMessage;

							ValidationChaged?.Invoke(SeriesProp, isValid);
						}
						break;

					case CodeProp:
						{
							var isValid = DataValidation.IsNumberValid(Code);
							if (isValid == false)
								error = InvalidNumberMessage;

							ValidationChaged?.Invoke(CodeProp, isValid);
						}
						break;

					case NumberProp:
						{
							var isValid = DataValidation.IsNumberValid(Number);
							if (isValid == false)
								error = InvalidNumberMessage;

							ValidationChaged?.Invoke(NumberProp, isValid);
						}
						break;

					case ExpirationProp:
						{
							var isValid = DataValidation.IsDateValid(Expiration);
							if (isValid == false)
								error = InvalidDateMessage;

							ValidationChaged?.Invoke(ExpirationProp, isValid);
						}
						break;

					case BirthProp:
						{
							var isValid = DataValidation.IsDateValid(Birth);
							if (isValid == false)
								error = InvalidDateMessage;

							ValidationChaged?.Invoke(BirthProp, isValid);
						}
						break;

					default:
						throw new ArgumentException($"Column name was {columnName}");

				}

				Error = error;
				return error;
			}
		}

		[JsonIgnore]
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
