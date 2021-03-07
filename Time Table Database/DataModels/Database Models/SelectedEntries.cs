using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Database.DataModels
{
    public class SelectedEntries
    {
        public int ID { get; set; }
        public Course Course { get; set; }
        public Section MainSection { get; set; }
        public Section TutorialSection { get; set; }
        public Section PracticalSection { get; set; }
    }
}