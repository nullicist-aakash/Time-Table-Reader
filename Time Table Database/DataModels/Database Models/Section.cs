using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Time_Table_Database.DataModels
{
    public class Section
    {
        public int ID { get; set; }
        public Course Course { get; set; }
        public int SectionType { get; set; }
        public int SectionNo { get; set; }
        public List<Teacher> Teachers { get; set; }
        public int? Room { get; set; }
        public List<Timing> ClassTiming { get; set; }
        
        public Section(ESectionType type)
        {
            SectionType = (int)type;
        }
    }
}