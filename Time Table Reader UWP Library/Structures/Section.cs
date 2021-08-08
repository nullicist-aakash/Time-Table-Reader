using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace TimeTableReader
{
    [JsonObject]
    public class Section
    {
        public readonly List<Teacher> Teachers = new List<Teacher>();
        public int SectionNo { get; set; }
        public int Room { get; set; }
        public Timing ClassTiming { get; set; }
        public int CommonHourRoom { get; set; }
        public Timing CommonHourTiming { get; set; }

        public Section() { }

        [JsonConstructor]
        public Section(List<Teacher> teachers, int sectionNo, int room, Timing classTiming, int commonHourRoom, Timing commonHourTiming)
        {
            SectionNo = sectionNo;
            Room = room;
            ClassTiming = classTiming;
            CommonHourRoom = commonHourRoom;
            CommonHourTiming = commonHourTiming;
            Teachers.AddRange(from x in teachers select TeacherGenerator.GenerateTeacher(x.Name));
        }

        public override bool Equals(object obj)
        {
            return obj is Section section &&
                   Teachers.SequenceEqual(section.Teachers) &&
                   SectionNo == section.SectionNo &&
                   Room == section.Room &&
                   EqualityComparer<Timing>.Default.Equals(ClassTiming, section.ClassTiming) &&
                   CommonHourRoom == section.CommonHourRoom &&
                   EqualityComparer<Timing>.Default.Equals(CommonHourTiming, section.CommonHourTiming);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", SectionNo, Teachers.First()?.Name ?? "", ClassTiming);
        }
    }
}