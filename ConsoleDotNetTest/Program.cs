using System;
using Recognizer;
namespace ConsoleDotNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testIMG = @"./4.jpg";
            Recognize recognize = new Recognize();
            recognize.MRZ(testIMG);
        }
    }
}
