using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Time_Table_Generator
{
    [JsonObject]
    public class TimeTable
    {
        public readonly List<Course> Courses = new List<Course>();

        public override bool Equals(object obj)
        {
            return obj is TimeTable table &&
                   Courses.SequenceEqual(table.Courses);
        }

        public override int GetHashCode()
        {
            return -1609692935 + EqualityComparer<List<Course>>.Default.GetHashCode(Courses);
        }
    }
}