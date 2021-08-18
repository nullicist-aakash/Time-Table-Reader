using System;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableReader
{
    public class CourseModel
    {
        public string CourseNo_Dept { get; set; }
        public string CourseNo_Id { get; set; }
        public string Course_Name { get; set; }
        public Credit Credits { get; set; }
        public CourseComponentsModel GeneralComponent { get; set; }
        public CourseComponentsModel PracticalComponent { get; set; }
        public CourseComponentsModel TutorialComponent { get; set; }
        public string CompreTiming { get; set; }

        public CourseModel() { }

        public CourseModel(Course c)
        {
            CourseNo_Dept = c.CourseNo_Dept;
            CourseNo_Id = c.CourseNo_Id;
            Course_Name = c.Course_Name;
            Credits = c.Credits;
            GeneralComponent = new CourseComponentsModel(c.GeneralClass);
            PracticalComponent = new CourseComponentsModel(c.PracticalClass);
            TutorialComponent = new CourseComponentsModel(c.TutorialClass);
            CompreTiming = c.CompreTiming;
        }

        public void OnCourseDeselected()
        {
            GeneralComponent.OnDeselectCourseOperation();
            PracticalComponent.OnDeselectCourseOperation();
            TutorialComponent.OnDeselectCourseOperation();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} - {2}", CourseNo_Dept, CourseNo_Id, Course_Name);
        }
    }
}