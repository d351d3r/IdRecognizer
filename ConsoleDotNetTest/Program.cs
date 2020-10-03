using Newtonsoft.Json;
using RecognizerDLL.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleDotNetTest
{
	class Program
	{

		static void Main(string[] args)
		{
			//var testIMG = @"D:\WorkSpace\kakaton\CP\ConsoleDotNetTest\bin\Debug\netcoreapp3.1\fedos.jpg";
			//Recognize recognize = new Recognize();
			//recognize.Rec(testIMG);

			// IdParser.Parse("");
			var testIMG = @"jopa.png";
			//var testIMG = @"D:\WorkSpace\kakaton\CP\ConsoleDotNetTest\bin\Debug\netcoreapp3.1\jopa.png";
			Recognizer recognize = new Recognizer();
			recognize.RecognitionFinished += Recognize_RecognitionFinished;
			recognize.RecognizeId(testIMG);

			//foreach (var item in strs)
			//{
			//	Console.WriteLine(item);
			//}
		}

		private static void Recognize_RecognitionFinished(string jsonPath)
		{
			var jsonStr = File.ReadAllText(jsonPath);
			Console.WriteLine(jsonStr);

			var p = JsonConvert.DeserializeObject<PersonId>(jsonStr);
		}
	}
}
