using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTableReader
{
    public class TeacherGenerator
    {
        static readonly List<Teacher> AllTeachers = new List<Teacher>();

        public static Teacher GenerateTeacher(string Name)
        {
            Name = Name.ToUpper();

            foreach (var t in AllTeachers)
                if (t.Name == Name)
                    return t;

            return new Teacher() { Name = Name };
        }
    }

    [JsonObject]
    public class Teacher
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Teacher t)
                return t.Name.Equals(Name);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}