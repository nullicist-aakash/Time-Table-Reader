using System;
using System.Collections.Generic;

namespace Time_Table_Database.DataModels
{
    public class Timing
    {
        public DayOfWeek Day { get; set; }
        public int Hour { get; set; }
        public List<Section> Sections { get; set; }
    }
}