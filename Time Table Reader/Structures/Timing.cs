using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Time_Table_Generator
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Timing
    {
        [JsonProperty]
        readonly List<uint> Entries = new List<uint>();
        static uint DayMap => 0xFFFF0000;
        static uint HourMap => 0x0000FFFF;
        public static Timing GenerateEmptyTiming => new Timing("", "");

        [JsonConstructor]
        public Timing(List<uint> input) => Entries.AddRange(input);
        public Timing(string days, string hours) : this((days, hours)) { }
        public Timing(IEnumerable<(string days, string hours)> input) : this(input.ToArray()) { }
        public Timing(params (string days, string hours)[] Timings)
        {
            foreach (var (days, hours) in Timings)
            {
                uint day = 0, hour = 0;
                foreach (var x in GenerateDaysOfWeek(days))
                    day |= (uint)(1 << (int)x);

                foreach (var x in GenerateHours(hours))
                    hour |= (uint)(1 << x);

                Entries.Add(Construct(day, hour));
            }
            Entries.Sort();
        }

        (uint days, uint hours) Deconstruct(uint input) => ((input & DayMap) >> 16, input & HourMap);
        uint Construct(uint days, uint hours) => ((days << 16) & DayMap) | (hours & HourMap);

        public bool IsClashing(Timing t)
        {
            foreach (var x in t.Entries)
                foreach (var y in Entries)
                {
                    var (days, hours) = Deconstruct(x);
                    var t2 = Deconstruct(y);

                    if ((hours & t2.hours) != 0 && (days & t2.days) != 0)
                        return true;
                }

            return false;
        }

        private IEnumerable<DayOfWeek> GenerateDaysOfWeek(string s)
        {
            Dictionary<string, DayOfWeek> dict = new Dictionary<string, DayOfWeek>
            {
                { "M", DayOfWeek.Monday },
                { "T", DayOfWeek.Tuesday },
                { "W", DayOfWeek.Wednesday },
                { "TH", DayOfWeek.Thursday },
                { "F", DayOfWeek.Friday },
                { "S", DayOfWeek.Saturday }
            };

            foreach (var x in from a in s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) select a.ToUpper())
                yield return dict[x];
        }

        private IEnumerable<int> GenerateHours(string hours) => from x in hours.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) select int.Parse(x);

        public override bool Equals(object obj) => obj is Timing t && t.Entries.SequenceEqual(Entries);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}