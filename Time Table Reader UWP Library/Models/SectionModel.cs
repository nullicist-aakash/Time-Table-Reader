using System.Collections.Generic;

namespace TimeTableReader
{
    public class SectionModel
    {
        public readonly List<Teacher> Teachers = new List<Teacher>();
        public int SectionNo { get; set; }
        public string RoomNo { get; set; }
        public Timing ClassTiming { get; set; }

        public SectionModel(Section s)
        {
            Teachers = s.Teachers;
            SectionNo = s.SectionNo;
            ClassTiming = s.ClassTiming;
            RoomNo = s.Room.ToString();
        }

        public override string ToString() => string.Format("{0} - {1} ({2})", SectionNo, Teachers[0].Name, ClassTiming);
    }
}