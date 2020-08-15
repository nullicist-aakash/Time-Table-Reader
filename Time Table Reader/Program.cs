using System.IO;
using System;
using Newtonsoft.Json;

namespace Time_Table_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileloc = @"D:\G - Projects\Time Table Khurapat\Goa.xlsx";

            TimeTable t;

            using (var x = new Goa_Parser { FileLoc = fileloc })
            {
                x.Load();
                Console.WriteLine("{0} : Loaded", DateTime.Now);
                t = x.GenerateTimeTable();
            }

            Console.WriteLine("{0} : Import Completed", DateTime.Now);

            string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);


            Console.WriteLine("JSON Conversion Completed");

            File.WriteAllText(@"D:\G - Projects\Time Table Khurapat\Goa.Json", JSON);
        }
    }
}