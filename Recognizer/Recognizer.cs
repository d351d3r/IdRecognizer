using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tesseract;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text.Json;
using System.Linq;
using RecognizerDLL.Utils;

public class Recognizer
{
	public delegate void RecognitionFinishedDelegate(string jsonPath);

	public event RecognitionFinishedDelegate RecognitionFinished;

	public void RecognizeId(string path)
	{
		try
		{
			var p = System.IO.Path.GetFullPath(@".\tessdata\");
			using var engine = new TesseractEngine(p, "mrz", EngineMode.Default);

			using var img = Pix.LoadFromFile(path);

			using var page = engine.Process(img);

			var text = page.GetText();
			var res = text.Split("\n").Where(x => x.Length > 25).TakeLast(2);
			var pData = IdParser.Parse(res.ElementAt(0), res.ElementAt(1));
			//return ;

			//RecognitionFinished?.Invoke(path);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			throw e;
		}
	}
}
