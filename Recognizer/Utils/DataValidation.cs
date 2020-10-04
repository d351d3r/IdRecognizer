using System;
using System.Text.RegularExpressions;

namespace RecognizerDLL.Utils
{
	internal static class DataValidation
	{
		private const int MaxAge = 120;

		public static bool IsNameValid(string input)
		{
			var regex = new Regex("[0-9]");

			return regex.IsMatch(input) == false && input.Length > 2;
		}

		public static bool IsSeriesValid(string input)
		{
			var regex = new Regex("^[0-9]{4}$");
			return regex.IsMatch(input);
		}

		public static bool IsNumberValid(string input)
		{
			var regex = new Regex("^[0-9]{6}$");
			return regex.IsMatch(input);
		}

		public static bool IsDateValid(string input)
		{
			var isVal = DateTime.TryParse(input, out var res);
			return isVal && res < DateTime.Today && res.Year >= DateTime.Today.Year - MaxAge;
		}
	}
}
