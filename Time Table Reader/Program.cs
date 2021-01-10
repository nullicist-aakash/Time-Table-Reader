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
            string fileloc = @"D:\G - Projects\Time Table Khurapat\Timetable_IISem_2020_21_10thJan.xlsx";

            TimeTable t;

            using (var x = new Pilani_Parser { FileLoc = fileloc })
            {
                x.Load();
                Console.WriteLine("{0} : Loaded", DateTime.Now);
                t = x.GenerateTimeTable();
            }

            Console.WriteLine("{0} : Import Completed", DateTime.Now);

            string JSON = JsonConvert.SerializeObject(t, Formatting.Indented);


            Console.WriteLine("JSON Conversion Completed");

            File.WriteAllText(@"D:\G - Projects\Time Table Khurapat\Pilani.Json", JSON);
        }

        static void Temp()
        {
            string daytime = Console.ReadLine();
            var splitted = daytime.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var split_indices = new List<int>();

            #region SplitIndices
            split_indices.Add(0);

            for (int i = 1; i < splitted.Length; ++i)
            {
                bool prev_time = int.TryParse(splitted[i - 1], out _);
                bool cur_time = int.TryParse(splitted[i], out _);

                if ((prev_time && !cur_time) || (!prev_time && cur_time))
                    split_indices.Add(i);
            }
            split_indices.Add(splitted.Length);

            #endregion

            var entries = new List<string>[2]
            {
                new List<string>(),
                new List<string>()
            };

            for (int i = 0; i < split_indices.Count - 1; ++i)
            {
                int start_index = split_indices[i];
                int end_index = split_indices[i + 1];
                var entrs = new List<string>();
                for (int j = start_index; j < end_index; ++j)
                    entrs.Add(splitted[j]);

                entries[i % 2].Add(string.Join(" ", entrs));
            }
            var lst = new List<(string days, string hours)>();


            for (int i = 0; i < entries[0].Count; ++i)
                lst.Add((entries[0][i], entries[1][i]));

            var time = new Timing(lst);

        }
    }
}