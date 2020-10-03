using System;
namespace ConsoleDotNetTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var testIMG = @"D:\WorkSpace\kakaton\CP\ConsoleDotNetTest\bin\Debug\netcoreapp3.1\jopa.png";
            Recognize recognize = new Recognize();
            recognize.Rec2(testIMG);
        }
    }
}
