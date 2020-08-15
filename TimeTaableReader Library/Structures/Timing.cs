using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TimeTableReader
{
    [JsonObject(MemberSerialization.Fields)]
    public class Timing
    {
        [JsonProperty]
        internal readonly List<uint> Entries = new List<uint>();
        internal static uint DayMap => 0xFFFF0000;
        internal static uint HourMap => 0x0000FFFF;
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
                    day |= (uint)(1 << x);

                foreach (var x in GenerateHours(hours))
                    hour |= (uint)(1 << x);

                Entries.Add(Construct(day, hour));
            }
            Entries.Sort();
        }

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

        internal static (uint days, uint hours) Deconstruct(uint input) => ((input & DayMap) >> 16, input & HourMap);

        internal static uint Construct(uint days, uint hours) => ((days << 16) & DayMap) | (hours & HourMap);

        IEnumerable<int> GenerateDaysOfWeek(string s)
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
                yield return (int)dict[x];
        }

        IEnumerable<int> GenerateHours(string hours) => from x in hours.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) select int.Parse(x);

        internal static IEnumerable<int> Hours(uint hours)
        {
            for (int i = 1; i < 16; ++i)
                if ((hours & 1 << i) != 0)
                    yield return i;
        }

        internal static IEnumerable<int> Days(uint days)
        {
            for (int i = 0; i < 8; ++i)
                if ((days & 1 << i) != 0)
                    yield return i;
        }


        public override bool Equals(object obj) => obj is Timing t && t.Entries.SequenceEqual(Entries);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var dict = new Dictionary<int, string> { { 1, "M" }, { 2, "T" }, { 3, "W" }, { 4, "Th" }, { 5, "F" }, { 6, "S" } };

            var outputStrings = new List<string>();
            foreach (var x in Entries)
            {
                var (days, hours) = Deconstruct(x);
                var _days = string.Join(" ", from a in Days(days) select dict[a]);
                var _hours = string.Join(" ", from a in Hours(hours) select a);
                outputStrings.Add(string.Join(" ", new[] { _days, _hours }));
            }
            return string.Join(", ", outputStrings);
        }

    }
}