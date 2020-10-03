using RecognizerDLL.Utils;
using System;
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
   //         var strs = recognize.RecognizeId(testIMG);

			//foreach (var item in strs)
			//{
			//	Console.WriteLine(item);
			//}
        }
    }
}
