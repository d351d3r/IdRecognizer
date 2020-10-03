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
using org.doubango.ultimateMrz.Sdk;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;


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

        public  void Rec(string path) 
        {
            //UltMrzSdkResult result = CheckResult("Init");

            // Decode the JPEG/PNG/BMP file
            String file = path;
            if (!System.IO.File.Exists(file))
            {
                throw new System.IO.FileNotFoundException("File not found:" + file);
            }
        Bitmap image = new Bitmap(file);
            if (Image.GetPixelFormatSize(image.PixelFormat) == 24 && ((image.Width * 3) & 3) != 0)
            {
                //!\\ Not DWORD aligned -> the stride will be multiple of 4-bytes instead of 3-bytes
                // ultimateMICR requires stride to be in samples unit instead of in bytes
                Console.Error.WriteLine(String.Format("//!\\ The image width ({0}) not a multiple of DWORD.", image.Width));
                image = new Bitmap(image, new Size((image.Width + 3) & -4, image.Height));
            }
            int bytesPerPixel = Image.GetPixelFormatSize(image.PixelFormat) >> 3;
            if (bytesPerPixel != 1 && bytesPerPixel != 3 && bytesPerPixel != 4)
            {
                throw new System.Exception("Invalid BPP:" + bytesPerPixel);
            }

            // Extract Exif orientation
            const int ExifOrientationTagId = 0x112;
            int orientation = 1;
            if (Array.IndexOf(image.PropertyIdList, ExifOrientationTagId) > -1)
            {
                int orientation_ = image.GetPropertyItem(ExifOrientationTagId).Value[0];
                if (orientation_ >= 1 && orientation_ <= 8)
                {
                    orientation = orientation_;
                }
            }


           
            //static String BuildJSON(String assetsFolder = "", String tokenDataBase64 = "")
            //{
            //    return new JavaScriptSerializer().Serialize(new
            //    {
            //        debug_level = CONFIG_DEBUG_LEVEL,
            //        debug_write_input_image_enabled = CONFIG_DEBUG_WRITE_INPUT_IMAGE,
            //        debug_internal_data_path = CONFIG_DEBUG_DEBUG_INTERNAL_DATA_PATH,

            //        num_threads = CONFIG_NUM_THREADS,
            //        gpgpu_enabled = CONFIG_GPGPU_ENABLED,
            //        gpgpu_workload_balancing_enabled = CONFIG_GPGPU_WORKLOAD_BALANCING_ENABLED,

            //        segmenter_accuracy = CONFIG_SEGMENTER_ACCURACY,
            //        interpolation = CONFIG_INTERPOLATION,
            //        min_num_lines = CONFIG_MIN_NUM_LINES,
            //        roi = CONFIG_ROI,
            //        min_score = CONFIG_MIN_SCORE,

            //        // Value added using command line args
            //        assets_folder = assetsFolder,
            //        license_token_data = tokenDataBase64,
            //    });
            //}


            //public void MRZ(string path)
            //{

            //    try
            //    {
            //        using (var engine = new TesseractEngine(@"tessdata", "mrz", EngineMode.Default))
            //        {
            //            using (var img = Pix.LoadFromFile(path))
            //            {
            //                var processed = img.ConvertRGBToGray();

            //                using (var page = engine.Process(img))
            //                {

            //                    var text = page.GetText();
            //                    Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

            //                    Console.WriteLine("Text (GetText): \r\n{0}", text);
            //                    Console.WriteLine("Text (iterator):");
            //                    using (var iter = page.GetIterator())
            //                    {
            //                        iter.Begin();

            //                        do
            //                        {
            //                            do
            //                            {
            //                                do
            //                                {
            //                                    do
            //                                    {
            //                                        if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
            //                                        {
            //                                            Console.WriteLine("<BLOCK>");
            //                                        }

            //                                        Console.Write(iter.GetText(PageIteratorLevel.Word));
            //                                        Console.Write(" ");

            //                                        if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
            //                                        {
            //                                            Console.WriteLine();
            //                                        }
            //                                    } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

            //                                    if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
            //                                    {
            //                                        Console.WriteLine();
            //                                    }
            //                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
            //                            } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
            //                        } while (iter.Next(PageIteratorLevel.Block));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Trace.TraceError(e.ToString());
            //        Console.WriteLine("Unexpected Error: " + e.Message);
            //        Console.WriteLine("Details: ");
            //        Console.WriteLine(e.ToString());
            //    }
            //    Console.Write("Press any key to continue . . . ");
            //    Console.ReadKey(true);





            //Emgu.CV.OCR.Tesseract tesseract = new Emgu.CV.OCR.Tesseract("./tessdata", "eng", OcrEngineMode.Default);
            //var img = Tesseract.Pix.LoadFromFile(path);
            //var img = new Image<Gray, byte>(path);
            // img._EqualizeHist();
            //img._GammaCorrect(1.8d);

            //tesseract.SetImage(img.);
            //CvInvoke.NamedWindow("AAAAAAAAAAAA");
            //CvInvoke.Imshow("AAAAAAAAAAAA", img);

            //Console.WriteLine(tesseract.());
            //Console.ReadLine();
            //}

        }
}
