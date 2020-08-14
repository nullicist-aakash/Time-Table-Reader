using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Time_Table_Generator
{
    public class ExcelToTimeTable : IDisposable
    {
        public string FileLoc { get; set; }
        Workbook Workbook { get; set; }
        Worksheet Worksheet { get; set; }

        public void Dispose()
        {
            Marshal.ReleaseComObject(Worksheet);
            Workbook.Close();
            Marshal.ReleaseComObject(Workbook);
        }

        public void Load()
        {
            var app = new Application();
            Workbook = app.Workbooks.Open(FileLoc);

            foreach (Worksheet xlWorkSheet in Workbook.Sheets)
            {
                Worksheet = xlWorkSheet;
                break;
            }
        }

        public TimeTable GenerateTimeTable()
        {
            var contents = ExtractRows(Worksheet.UsedRange);
            var courses_splitted = from x in contents select GetCourseList(x);
            var tt = new TimeTable();
            tt.Courses.AddRange(GenerateCourses(courses_splitted.ToList()));
            return tt;
        }

        private List<string[]> ExtractRows(Range range)
        {
            int TotalRows = range.Rows.Count;
            int TotalColumns = range.Columns.Count;
            object[,] array = range.Cells.Value;

            List<string[]> Content = new List<string[]>();

            for (int i = 1; i <= TotalRows; ++i)
            {
                var row = new List<string>();

                for (int j = 1; j <= TotalColumns; ++j)
                    row.Add(array[i, j]?.ToString() ?? "");

                Content.Add(row.ToArray());
            }
            return Content;
        }

        IntermediateStructure GetCourseList(string[] row)
        {
            return (-1,
                row[2],
                row[3].Replace("\n", "").Replace("\r", ""),
                GenerateCredits(row[4], row[5]),
                row[5] == "R" ? "L" : row[5],
                int.Parse(row[6]),
                (from x
                 in row[7].Replace("\n", "").Replace("\r", "").Split(new char[] { ',', '/' }, StringSplitOptions.RemoveEmptyEntries)
                 select x.ToUpper()).ToList(),
                 GenerateTiming(row[8]));
        }

        List<Course> GenerateCourses(List<IntermediateStructure> structures)
        {
            var courses = new List<Course>();

            for (int i = 0; i < structures.Count; )
            {
                int j = i;
                var common = new List<IntermediateStructure>();

                Console.WriteLine("{0} : Processing course : {1}", DateTime.Now, structures[j].CourseName);
                while (i < structures.Count && structures[i].CourseName == structures[j].CourseName)
                    common.Add(structures[i++]);

                courses.Add(CombineCourse(common));
            }

            return courses;
        }

        Course CombineCourse(List<IntermediateStructure> entries)
        {
            Course c = new Course
            {
                COMCOD = entries[0].COMCOD,
                CourseNo_Dept = entries[0].CourseID.Split(' ')[0],
                CourseNo_Id = entries[0].CourseID.Split(' ')[1],
                Course_Name = entries[0].CourseName,
                Credits = entries[0].credits
            };

            var generalList = new List<IntermediateStructure>();
            var practicalList = new List<IntermediateStructure>();
            var tutorialList = new List<IntermediateStructure>();

            foreach (var x in entries)
                switch (x.Type)
                {
                    case "L": generalList.Add(x); break;
                    case "P": practicalList.Add(x); break;
                    case "T": tutorialList.Add(x); break;
                }

            c.GeneralClass.AddRange(CombineComponents(generalList));
            c.PracticalClass.AddRange(CombineComponents(practicalList));
            c.TutorialClass.AddRange(CombineComponents(tutorialList));

            return c;
        }

        IEnumerable<Section> CombineComponents(List<IntermediateStructure> sections)
        {
            foreach (var x in sections)
            {
                var section = new Section
                {
                    ClassTiming = x.time,
                    CommonHourRoom = -1,
                    CommonHourTiming = new Timing(0, 0),
                    Room = -1,
                    SectionNo = x.Section
                };
                section.Teachers.AddRange(from y in x.Teachers select TeacherGenerator.GenerateTeacher(y));
                yield return section;
            }
        }

        Credit GenerateCredits(string cell, string type)
        {
            var units = (from x in cell.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) select int.Parse(x)).ToList();

            Credit credits;
            if (units.Count == 1 && type != "P")
                credits = new Credit { Lecture = units[0], Practical = 0, Units = units[0] };
            else if (units.Count == 1 && type == "P")
                credits = new Credit { Lecture = 0, Practical = units[0], Units = units[0] };
            else
                credits = new Credit { Lecture = units[0], Practical = units[0], Units = units[0] };

            return credits;
        }

        Timing GenerateTiming(string cell)
        {
            if (cell == "TBA")
                return new Timing(0, 0);
            var splits = cell.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var days = new List<string>();
            var hours = new List<int>();

            foreach (var x in splits)
                if (int.TryParse(x, out int converted))
                    hours.Add(converted);
                else
                    days.Add(x);

            return new Timing(string.Join(" ", days), string.Join(" ", hours));
        }
    }

    internal class IntermediateStructure
    {
        public int COMCOD;
        public string CourseID;
        public string CourseName;
        public Credit credits;
        public string Type;
        public int Section;
        public List<string> Teachers;
        public Timing time;

        public IntermediateStructure(int cOMCOD, string courseID, string courseName, Credit credits, string type, int section, List<string> teachers, Timing time)
        {
            COMCOD = cOMCOD;
            CourseID = courseID;
            CourseName = courseName;
            this.credits = credits;
            Type = type;
            Section = section;
            Teachers = teachers;
            this.time = time;
        }

        public override bool Equals(object obj)
        {
            return obj is IntermediateStructure other &&
                   COMCOD == other.COMCOD &&
                   CourseID == other.CourseID &&
                   CourseName == other.CourseName &&
                   credits.Equals(other.credits) &&
                   Type == other.Type &&
                   Section == other.Section &&
                   Teachers.SequenceEqual(other.Teachers) &&
                   time.Equals(other.time);
        }

        public override int GetHashCode()
        {
            int hashCode = -882678744;
            hashCode = hashCode * -1521134295 + COMCOD.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CourseID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CourseName);
            hashCode = hashCode * -1521134295 + EqualityComparer<Credit>.Default.GetHashCode(credits);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + Section.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Teachers);
            hashCode = hashCode * -1521134295 + EqualityComparer<Timing>.Default.GetHashCode(time);
            return hashCode;
        }

        public void Deconstruct(out int cOMCOD, out string courseID, out string courseName, out Credit credits, out string type, out int section, out List<string> teachers, out Timing time)
        {
            cOMCOD = COMCOD;
            courseID = CourseID;
            courseName = CourseName;
            credits = this.credits;
            type = Type;
            section = Section;
            teachers = Teachers;
            time = this.time;
        }

        public static implicit operator (int COMCOD, string CourseID, string CourseName, Credit credits, string Type, int Section, List<string> Teachers, Timing time)(IntermediateStructure value)
        {
            return (value.COMCOD, value.CourseID, value.CourseName, value.credits, value.Type, value.Section, value.Teachers, value.time);
        }

        public static implicit operator IntermediateStructure((int COMCOD, string CourseID, string CourseName, Credit credits, string Type, int Section, List<string> Teachers, Timing time) value)
        {
            return new IntermediateStructure(value.COMCOD, value.CourseID, value.CourseName, value.credits, value.Type, value.Section, value.Teachers, value.time);
        }
    }
}