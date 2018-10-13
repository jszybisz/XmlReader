using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ConsoleApp1.Extensions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var busList = new List<Bus>();
            DataUpdaterService dus = new DataUpdaterService();

          
             dus.UpdateFromXml(@"C:\Users\jacek.szybisz\Desktop\ConsoleApp1\Test.xml", busList);
         
            //busList.ForEach(b=>Console.WriteLine(b.ToJson(true)));
      
            Task.Delay(10000);
            Console.WriteLine($"Number of busses:{busList.Count}");
            Console.ReadLine();
        }
    }
}
