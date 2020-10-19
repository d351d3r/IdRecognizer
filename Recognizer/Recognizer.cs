using System;
using Tesseract;
using Newtonsoft.Json;
using System.Linq;
using RecognizerDLL.Utils;
using System.IO;

public class Recognizer
{
	private const string ParseErrorMessage = "Невозможно распознать текст!";
	private const string TemporatyFileExtension = ".tmp";

	public delegate void RecognitionFinishedDelegate(string jsonPath);

	public event RecognitionFinishedDelegate RecognitionFinished;

	public void RecognizeId(string path)
	{
		try
		{
			var text = Recognize(path);
			var res = text.Split("\n").Where(x => x.Length > 25).TakeLast(2);

			var pData = IdParser.Parse(res.ElementAt(0), res.ElementAt(1));
			var jsonStr = JsonConvert.SerializeObject(pData);

			var jsonPath = SaveToTemp(jsonStr);

			RecognitionFinished?.Invoke(jsonPath);
		}
		catch (ArgumentOutOfRangeException aOrE)
		{
			Console.WriteLine(aOrE.ToString());
			RecognitionFinished?.Invoke(null);
			throw new ArgumentException(ParseErrorMessage, aOrE);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			RecognitionFinished?.Invoke(null);
			throw e;
		}
	}

	private string SaveToTemp(string jsonStr)
	{
		string path = Path.GetTempPath() + Guid.NewGuid().ToString() + TemporatyFileExtension;
		File.WriteAllText(path, jsonStr);
		return path;
	}

	private string Recognize(string path)
	{
		var p = Path.GetFullPath(@".\tessdata\");
		using var engine = new TesseractEngine(p, "mrz", EngineMode.Default);

		using var img = Pix.LoadFromFile(path);

		using var page = engine.Process(img);

		return page.GetText();
	}
}
