using Newtonsoft.Json;
using System;

namespace TimeTableReader
{
    [JsonObject]
    public class Credit
    {
        public int Lecture { get; set; }
        public int Practical { get; set; }
        public int Units { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Credit c))
                return false;

            return c.Lecture.Equals(Lecture) && c.Practical.Equals(Practical) && c.Units.Equals(Units);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}