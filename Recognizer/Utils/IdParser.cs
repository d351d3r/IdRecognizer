using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecognizerDLL.Utils
{
	public static class IdParser
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
				Birth = new string(birth),
				Gender = new string(gender),
				Code = new string(code), 
				Expiration = new string(expiration),
			};
		}
	}
}
