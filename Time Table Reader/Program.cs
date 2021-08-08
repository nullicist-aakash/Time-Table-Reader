using System.IO;
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Time_Table_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileloc = @"E:\Time Table Reader\Timetable_4thAug_2021.xlsx";

            TimeTable t;

            using (var x = new Pilani_Parser { FileLoc = fileloc })
            {
                Console.WriteLine("1");
                x.Load();
                Console.WriteLine("{0} : Loaded", DateTime.Now);
                t = x.GenerateTimeTable();
            }

            Console.WriteLine("{0} : Import Completed", DateTime.Now);

            string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);


            Console.WriteLine("JSON Conversion Completed");

            File.WriteAllText(@"E:\Time Table Reader\Pilani.Json", JSON);
        }
    }
}