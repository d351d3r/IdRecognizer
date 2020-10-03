using System;
namespace ConsoleDotNetTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var testIMG = @"D:\WorkSpace\kakaton\CP\ConsoleDotNetTest\bin\Debug\netcoreapp3.1\fedos.jpg";
            Recognize recognize = new Recognize();
            recognize.Rec(testIMG);
        }
    }
}
