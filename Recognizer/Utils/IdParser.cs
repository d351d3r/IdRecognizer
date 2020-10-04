using System.Linq;

namespace RecognizerDLL.Utils
{
	internal static class IdParser
	{
		public static PersonId Parse(string firstLine, string secondLine)
		{
			var p = Split(firstLine, secondLine);

			return p;
		}

		private static PersonId Split(string firstLine, string secondLine)
		{
			var namesInput = firstLine.Split("<").Where(x => x != "");
			var secondName = namesInput.ElementAt(0).Skip(5).ToArray();
			var name = namesInput.ElementAt(1);
			var middleName = namesInput.ElementAt(2);

			var secondPhraseParts = secondLine.Split("<").Where(x => x != "");

			var firstPart = secondPhraseParts.ElementAt(0);
			var series = firstPart.Take(3).ToArray();
			var number = firstPart.Skip(3).Take(6).ToArray();
			var birth = firstPart.Skip(3 + 6 + 4).Take(6).ToArray();
			var gender = firstPart.Skip(3 + 6 + 4 + 6 + 1).Take(1).ToArray();

			var secongPart = secondPhraseParts.ElementAt(1).ToArray();
			var expiration = secongPart.Skip(1).Take(6).ToArray();
			var code = secongPart.Skip(1 + 6).Take(6).ToArray();

			return new PersonId
			{
				Name = name.ToRus(),
				SecondName = new string(secondName).ToRus(),
				MiddleName = middleName.ToRus(),
				Series = new string(series),
				Number = new string(number),
				Birth = ToDateFormat(birth),
				Gender = new string(gender),
				Code = new string(code),
				Expiration = ToDateFormat(expiration),
			};
		}

		private static string ToDateFormat(char[] charArr)
		{
			//YYMMDD
			//YYYY.MM.DD
			var res = string.Empty;
			for (int i = 0; i < charArr.Length; i++)
			{
				if (i == 0)
				{
					if (charArr[i] >= '0' && charArr[i] <= '5')
						res += "20";
					else
						res += "19";
				}

				if (i == 2 || i == 4)
					res += ".";

				res += charArr[i];
			}

			return res;
		}
	}
}
