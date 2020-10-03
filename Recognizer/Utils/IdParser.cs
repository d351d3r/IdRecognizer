using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecognizerDLL.Utils
{
	public static class IdParser
	{
		private static string Sample = " Result: {\"duration\":117,\"frame_id\":0,\"zones\":[{\"lines\":[{\"confidence\":91.0,\"text\":\"PNRUSIWNKO<<FDOR<SRG**VI3<<<<<<<<<<<<<<<<\",\"warpedBox\":[-1.0,-1.0,-1.0,-1.0,-1.0,-1.0,-1.0,-1.0]},{\"confidence\":90.0,\"text\":\"02750277RUS000710M<<<<<<<0200*072\",\"warpedBox\":[-1.0,-1.0,-1.0,-1.0,-1.0,-1.0,-1.0,-1.0]}],\"warpedBox\":[283,2217,1765,2217,1765,2373,283,2373]}]}";

		public static void Parse(string jsonStr)
		{
			jsonStr = Sample;

			var p = Split(jsonStr);
		}

		private static PersonId Split(string jsonStr)
		{
			var ar = jsonStr.Split("\"text\":");
			var firstParse = ar[1].Split("\"warpedBox\"")[0].Replace("\"", "");
			var secondPhrase = ar[2].Split("\"warpedBox\"")[0].Replace("\"", "");

			//var ans = Regex.Match()

			var namesInput = firstParse.Split("<").Where(x => x != "");
			var secondName = namesInput.ElementAt(0).Skip(5);
			var name = namesInput.ElementAt(1);
			var middleName = namesInput.ElementAt(2);

			var secondPhraseParts = secondPhrase.Split("<").Where(x => x != "");

			var firstPart = secondPhraseParts.ElementAt(0);
			var series = firstPart.Take(3);
			var number = firstPart.Skip(3).Take(6);
			var birth = firstPart.Skip(3 + 6 + 4).Take(6);
			var gender = firstPart.Skip(3 + 6 + 4 + 6 + 1).Take(1);

			var secongPart = secondPhraseParts.ElementAt(1);
			var expiration = secongPart.Skip(1).Take(6);
			var code = secongPart.Skip(1 + 6).Take(6);

			return new PersonId
			{
				Name = name,
				SecondName = secondName.ToString(),
				MiddleName = middleName.ToString(),
				Series = series.ToString(),
				Number = number.ToString(),
				Birth = birth.ToString(),
				Gender = gender.ToString(),
				Code = code.ToString(), 
				Expiration = expiration.ToString(),
			};
		}
	}
}
