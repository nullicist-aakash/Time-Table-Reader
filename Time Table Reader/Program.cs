using System.IO;
using System;
using Newtonsoft.Json;

namespace Time_Table_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileloc = @"D:\G - Projects\Time Table Khurapat\Hyderabad.xlsx";

            TimeTable t;

            using (var x = new ExcelToTimeTable { FileLoc = fileloc })
            {
                x.Load();
                t = x.GenerateTimeTable();
            }
            Console.WriteLine("Import Completed");

            string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);



            Console.WriteLine("JSON Conversion Completed");

            File.WriteAllText(@"D:\G - Projects\Time Table Khurapat\Hyderabad.Json", JSON);
        }
    }
}