using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Time_Table_Database.DataModels
{
    public class Course
    {
        [Key]
        public int COMCOD { get; set; }
        public string CourseCode { get; set; }

        public string CourseName { get; set; }
        public byte LectureUnits { get; set; }
        public byte PracticalUnits { get; set; }
        public byte TotalUnits { get; set; }

        public virtual List<Section> Sections { get; private set; }
    }
}