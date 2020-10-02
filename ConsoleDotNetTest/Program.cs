using System;
using Recognizer;
namespace ConsoleDotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testIMG = @"./4.tif";
            Recognize recognize = new Recognize();
            recognize.Rec(testIMG);
        }
    }
}
