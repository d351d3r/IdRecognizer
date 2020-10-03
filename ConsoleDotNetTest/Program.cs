using System;
namespace ConsoleDotNetTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var testIMG = @"D:\WorkSpace\какатон\ЦП\ConsoleDotNetTest\bin\Debug\netcoreapp3.1\4.tif";
            Recognize recognize = new Recognize();
            recognize.Rec(testIMG);
        }
    }
}
