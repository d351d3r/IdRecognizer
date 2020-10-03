using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
//using Emgu.CV;
//using Emgu.CV.ImgHash;
//using Emgu.CV.OCR;
//using Emgu.CV.Saliency;
//using Emgu.CV.Structure;
using Tesseract;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Web.Script.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text.Json;
using System.Linq;

public class Recognize
{
	const String CONFIG_DEBUG_LEVEL = "info";
	const bool CONFIG_DEBUG_WRITE_INPUT_IMAGE = false; // must be false unless you're debugging the code
	const String CONFIG_DEBUG_DEBUG_INTERNAL_DATA_PATH = ".";
	const int CONFIG_NUM_THREADS = -1;
	const bool CONFIG_GPGPU_ENABLED = true;
	const bool CONFIG_GPGPU_WORKLOAD_BALANCING_ENABLED = false;
	const String CONFIG_SEGMENTER_ACCURACY = "high";
	const String CONFIG_INTERPOLATION = "bilinear";
	const int CONFIG_MIN_NUM_LINES = 2;
	static readonly IList<float> CONFIG_ROI = new[] { 0f, 0f, 0f, 0f };
	const double CONFIG_MIN_SCORE = 0.0;

	//    public  void Rec(string path) 
	//    {
	//    var p = System.IO.Path.GetFullPath(@".\assets");
	//    UltMrzSdkResult result = CheckResult("Init", UltMrzSdkEngine.init(BuildJSON(@"D:\WorkSpace\kakaton\CP\Recognizer\bin\Debug\netcoreapp3.1\assets", String.Empty)));

	//    // Decode the JPEG/PNG/BMP file
	//    String file = path;
	//    if (!System.IO.File.Exists(file))
	//    {
	//        throw new System.IO.FileNotFoundException("File not found:" + file);
	//    }
	//    Bitmap image = new Bitmap(file);
	//    if (Image.GetPixelFormatSize(image.PixelFormat) == 24 && ((image.Width * 3) & 3) != 0)
	//    {

	//        Console.Error.WriteLine(String.Format("//!\\ The image width ({0}) not a multiple of DWORD.", image.Width));
	//        image = new Bitmap(image, new Size((image.Width + 3) & -4, image.Height));
	//    }
	//    int bytesPerPixel = Image.GetPixelFormatSize(image.PixelFormat) >> 3;
	//    if (bytesPerPixel != 1 && bytesPerPixel != 3 && bytesPerPixel != 4)
	//    {
	//        throw new System.Exception("Invalid BPP:" + bytesPerPixel);
	//    }

	//    const int ExifOrientationTagId = 0x112;
	//    int orientation = 1;
	//    if (Array.IndexOf(image.PropertyIdList, ExifOrientationTagId) > -1)
	//    {
	//        int orientation_ = image.GetPropertyItem(ExifOrientationTagId).Value[0];
	//        if (orientation_ >= 1 && orientation_ <= 8)
	//        {
	//            orientation = orientation_;
	//        }
	//    }

	//    BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
	//    try
	//    {
	//        // For packed formats (RGB-family): https://www.doubango.org/SDKs/mrz/docs/cpp-api.html#_CPPv4N14ultimateMrzSdk15UltMrzSdkEngine7processEK21ULTMRZ_SDK_IMAGE_TYPEPKvK6size_tK6size_tK6size_tKi
	//        // For YUV formats (data from camera): https://www.doubango.org/SDKs/mrz/docs/cpp-api.html#_CPPv4N14ultimateMrzSdk15UltMrzSdkEngine7processEK21ULTMRZ_SDK_IMAGE_TYPEPKvPKvPKvK6size_tK6size_tK6size_tK6size_tK6size_tK6size_tKi
	//        result = CheckResult("Process", UltMrzSdkEngine.process(
	//                 (bytesPerPixel == 1) ? ULTMRZ_SDK_IMAGE_TYPE.ULTMRZ_SDK_IMAGE_TYPE_Y : (bytesPerPixel == 4 ? ULTMRZ_SDK_IMAGE_TYPE.ULTMRZ_SDK_IMAGE_TYPE_BGRA32 : ULTMRZ_SDK_IMAGE_TYPE.ULTMRZ_SDK_IMAGE_TYPE_BGR24),
	//                imageData.Scan0,
	//                (uint)imageData.Width,
	//                (uint)imageData.Height,
	//                (uint)(imageData.Stride / bytesPerPixel),
	//                orientation
	//            ));
	//        // Print result to console
	//        Console.WriteLine("Result: {0}", result.json());
	//    }
	//    finally
	//    {
	//        image.UnlockBits(imageData);
	//    }

	//    // Write until user press a key
	//    Console.WriteLine("Press any key to terminate !!");
	//    Console.Read();

	//    // Now that you're done, deInit the engine before exiting
	//    CheckResult("DeInit", UltMrzSdkEngine.deInit());



	//    static UltMrzSdkResult CheckResult(String functionName, UltMrzSdkResult result)
	//    {
	//        if (!result.isOK())
	//        {
	//            String errMessage = String.Format("{0}: Execution failed: {1}", new String[] { functionName, result.json() });
	//            Console.Error.WriteLine(errMessage);
	//            throw new Exception(errMessage);
	//        }
	//        return result;
	//    }

	//    static String BuildJSON(String assetsFolder = "", String tokenDataBase64 = "")
	//    {
	//        return System.Text.Json.JsonSerializer.Serialize(new
	//        {
	//            debug_level = CONFIG_DEBUG_LEVEL,
	//            debug_write_input_image_enabled = CONFIG_DEBUG_WRITE_INPUT_IMAGE,
	//            debug_internal_data_path = CONFIG_DEBUG_DEBUG_INTERNAL_DATA_PATH,

	//            num_threads = CONFIG_NUM_THREADS,
	//            gpgpu_enabled = CONFIG_GPGPU_ENABLED,
	//            gpgpu_workload_balancing_enabled = CONFIG_GPGPU_WORKLOAD_BALANCING_ENABLED,

	//            segmenter_accuracy = CONFIG_SEGMENTER_ACCURACY,
	//            interpolation = CONFIG_INTERPOLATION,
	//            min_num_lines = CONFIG_MIN_NUM_LINES,
	//            roi = CONFIG_ROI,
	//            min_score = CONFIG_MIN_SCORE,

	//            // Value added using command line args
	//            assets_folder = assetsFolder,
	//            license_token_data = tokenDataBase64,
	//        });
	//    }
	//}


	public IEnumerable<string> Rec2(string path)
	{
		try
		{
			var p = System.IO.Path.GetFullPath(@".\tessdata\");
			using var engine = new TesseractEngine(p, "mrz", EngineMode.Default);

			using var img = Pix.LoadFromFile(path);

			using var page = engine.Process(img);

			var text = page.GetText();
			return text.Split("\n").Where(x => x.Length > 25).TakeLast(4);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			throw e;
		}
	}
}
