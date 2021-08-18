using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TimeTableReader
{
    [JsonObject]
    public class Course
    {
        public int COMCOD { get; set; }
        public string CourseNo_Dept { get; set; }
        public string CourseNo_Id { get; set; }
        public string Course_Name { get; set; }
        public Credit Credits { get; set; }

        public readonly List<Section> GeneralClass = new List<Section>();
        public readonly List<Section> PracticalClass = new List<Section>();
        public readonly List<Section> TutorialClass = new List<Section>();

        public string CompreTiming { get; set; }

        public Course() { }

        public override bool Equals(object obj)
        {
            return obj is Course course &&
                   COMCOD == course.COMCOD &&
                   CourseNo_Dept == course.CourseNo_Dept &&
                   CourseNo_Id == course.CourseNo_Id &&
                   Course_Name == course.Course_Name &&
                   Credits.Equals(course.Credits) &&
                   GeneralClass.SequenceEqual(course.GeneralClass) &&
                   PracticalClass.SequenceEqual(course.PracticalClass) &&
                   TutorialClass.SequenceEqual(course.TutorialClass);
        }

        public override int GetHashCode()
        {
            int hashCode = -1771483375;
            hashCode = hashCode * -1521134295 + COMCOD.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CourseNo_Dept);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CourseNo_Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Course_Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Credit>.Default.GetHashCode(Credits);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Section>>.Default.GetHashCode(GeneralClass);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Section>>.Default.GetHashCode(PracticalClass);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Section>>.Default.GetHashCode(TutorialClass);
            return hashCode;
        }
    }
}