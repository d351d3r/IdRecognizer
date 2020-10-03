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
			//return text.Split("\n").Where(x => x.Length > 25).TakeLast(4);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			throw e;
		}
	}
}
