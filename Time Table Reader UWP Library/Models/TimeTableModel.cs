using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace TimeTableReader
{
    public class TimeTableModel
    {
        public ObservableCollection<CourseModel> Courses { get; } = new ObservableCollection<CourseModel>(); 
        public int totalUnits;

        TimingMap map = new TimingMap();
        readonly List<SectionModel> lst = new List<SectionModel>();

        public TimeTableModel()
        {
            Courses.CollectionChanged += (sender, e) =>
            {
                totalUnits = (from x in Courses select x.Credits.Units).Sum();
            };
        }

        public TimeTableModel(TimeTable tt)
        {
            foreach (var x in tt.Courses)
                Courses.Add(new CourseModel(x));
        }


        public void Sort()
        {
            var abc = Courses.OrderBy(x => x.CourseNo_Dept + " " + x.CourseNo_Id).ToList();
            Courses.Clear();
            foreach (var x in abc)
                Courses.Add(x);
        }

        public void UpdateMap_NewEntry(SectionModel sm)
        {
            lst.Add(sm);
            map = TimingMap.Union(map, new TimingMap(sm.ClassTiming));
        }

        public void RemoveEntry(SectionModel model)
        {
            lst.RemoveAll(x => x == model);

            map = new TimingMap();
            foreach (var x in from a in lst select a.ClassTiming)
                map = TimingMap.Union(map, new TimingMap(x));
        }

        public bool IsClashing(CourseComponentsModel ccm)
        {
            if (ccm.SelectedModel == null)
                return false;

            int ClashCount = 0;
            foreach (var x in from a in lst select new TimingMap(a.ClassTiming))
                if (TimingMap.Clash(x, new TimingMap(ccm.SelectedModel.ClassTiming)))
                    ++ClashCount;

            return ClashCount > 1;
        }
    }
}