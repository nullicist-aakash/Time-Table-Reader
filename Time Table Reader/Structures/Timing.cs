using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Time_Table_Generator
{
    [JsonObject(MemberSerialization.Fields)]
    public class Timing
    {
        readonly int days = 0;
        readonly int hours = 0;

        public IEnumerable<int> Hours
        {
            get
            {
                for (int i = 1; i < 16; ++i)
                    if ((hours & 1 << i) != 0)
                        yield return i;
            }
        }

        public IEnumerable<DayOfWeek> Days
        {
            get
            {
                for (int i = 0; i < 8; ++i)
                    if ((days & 1 << i) != 0)
                        yield return (DayOfWeek)i;
            }
        }

        [JsonConstructor]
        public Timing(int days, int hours)
        {
            this.days = days;
            this.hours = hours;
        }

        public Timing(string days, string hours)
        {
            foreach (var x in GenerateDaysOfWeek(days))
                this.days |= 1 << (int)x;

            foreach (var x in GenerateHours(hours))
                this.hours |= 1 << x;
        }

        public bool IsClashing(Timing t) => (t.days & days) != 0 && (t.hours & hours) != 0;

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

        public override bool Equals(object obj)
        {
            if (obj is Timing t)
                return t.days.Equals(days) && t.hours.Equals(hours);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}