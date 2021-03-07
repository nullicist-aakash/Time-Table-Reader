using System.Collections.Generic;

namespace Time_Table_Database.DataModels
{
    public class Teacher
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Section> Sections { get; set; }
    }
}