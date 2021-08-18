using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TimeTableReader
{
    public class Global
    {
        public static Global Instance { get; } = new Global();
        Global() { }

        public string JsonLocation;
        public TimeTableModel AvailableCourses = new TimeTableModel();
        public TimeTableModel SelectedCourses = new TimeTableModel();

        public void RegisterOnce(TimeTable tt)
        {
            foreach (var x in new TimeTableModel(tt).Courses)
                AvailableCourses.Courses.Add(x);
        }
        
        public void SelectCourse(CourseModel c) => SelectCourse(AvailableCourses.Courses.IndexOf(c));

        public void SelectCourse(int index)
        {
            SelectedCourses.Courses.Add(AvailableCourses.Courses[index]);
            AvailableCourses.Courses.RemoveAt(index);
            SelectedCourses.Sort();
        }

        public void RemoveCourse(CourseModel c) => RemoveCourse(SelectedCourses.Courses.IndexOf(c));

        public void RemoveCourse(int index)
        {
            SelectedCourses.Courses[index].OnCourseDeselected();
            AvailableCourses.Courses.Add(SelectedCourses.Courses[index]);
            SelectedCourses.Courses.RemoveAt(index);
        }
    }
}
