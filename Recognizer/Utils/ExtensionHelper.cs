using System.Collections.Generic;
using System.IO;

namespace RecognizerDLL.Utils
{
	static class ExtensionHelper
	{
		private const string UndetectedChar = "*";
		private static readonly Dictionary<char, char> Translit = new Dictionary<char, char>();
		private static readonly string SourcePath = Path.GetFullPath(@"D:\repos\IdRecognizer\Recognizer\Utils\translit_data.txt");

		static ExtensionHelper()
		{
			var pairs = File.ReadAllText(SourcePath).Split("\n");

			foreach (var pair in pairs)
			{
				var d = pair.Split("-");
				Translit.Add(d[0].ToCharArray()[0], d[1].ToCharArray()[0]);
			}
		}

		public static string ToRus(this string engData)
		{
			var result = string.Empty;
			foreach (var ch in engData)
			{
				if (Translit.ContainsKey(ch))
					result += Translit[ch];
				else
					result += UndetectedChar;
			}

			return result;
		}

	}
}
