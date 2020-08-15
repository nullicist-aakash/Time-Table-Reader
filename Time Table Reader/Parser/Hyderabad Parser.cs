using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Generator
{
    class Hyderabad_Parser : ExcelToTimeTable
    {
        protected override TimeTable DoGenerateTimeTable(List<string[]> contents)
        {
            var SplittedCourses = SplitCourses(contents);

            var tt = new TimeTable();
            tt.Courses.AddRange(from x in SplittedCourses select GenerateCourses(x));

            return tt;
        }

        private List<List<string[]>> SplitCourses(List<string[]> allRows)
        {
            if (allRows.Count == 0)
                return new List<List<string[]>>();
            if (allRows.Count == 1)
                return new List<List<string[]>> { allRows };

            var _startIndices = FindStartIndices(allRows);
            var _ranges = FindRanges(_startIndices);

            return GetRangeRows(allRows, _ranges);
        }

        private Course GenerateCourses(List<string[]> courseEntry)
        {
            var course = new Course
            {
                COMCOD = int.Parse(courseEntry[0][0]),
                CourseNo_Dept = courseEntry[0][1].Split(' ')[0],
                CourseNo_Id = courseEntry[0][1].Split(' ')[1],
                Course_Name = courseEntry[0][2].Replace("\n", "").Replace("\r", ""),
                Credits = new Credit
                {
                    Lecture = GetCredit(courseEntry[0][3]),
                    Practical = GetCredit(courseEntry[0][4]),
                    Units = GetCredit(courseEntry[0][5])
                }
            };
            Console.WriteLine("{0} : Processing course : {1}", DateTime.Now, course.Course_Name);

            var ClassIndices = FindStartIndices(courseEntry, 2);
            var Ranges = FindRanges(ClassIndices);
            var SplittedRows = GetRangeRows(courseEntry, Ranges);

            if (SplittedRows.Count > 0)
                course.GeneralClass.AddRange(GenerateClassEntries(SplittedRows[0]));

            for (int i = 1; i < SplittedRows.Count; ++i)
            {
                var classes = GenerateClassEntries(SplittedRows[i]);

                if (SplittedRows[i][0][2].ToUpper() == "TUTORIAL")
                    course.TutorialClass.AddRange(classes);
                else if (SplittedRows[i][0][2].ToUpper() == "PRACTICAL")
                    course.PracticalClass.AddRange(classes);
                else
                    throw new Exception("Help me!! I was not designed to do this. Something bad happened in course " + course.Course_Name);
            }
            return course;
        }

        private IEnumerable<Section> GenerateClassEntries(List<string[]> classes)
        {
            classes[0][6] = "1";
            var ClassIndices = FindStartIndices(classes, 6);
            var Ranges = FindRanges(ClassIndices);
            var SplittedRows = GetRangeRows(classes, Ranges);

            foreach (var section in SplittedRows)
                yield return GenerateSection(section);
        }

        List<int> FindStartIndices(List<string[]> rows, int CheckIndex = 0)
        {
            var StartIndices = new List<int>();

            for (int i = 0; i < rows.Count; ++i)
                if (rows[i][CheckIndex] != "")
                    StartIndices.Add(i);
            StartIndices.Add(rows.Count);
            return StartIndices;
        }

        List<(int StartIndex, int Length)> FindRanges(List<int> startIndices)
        {
            var Range = new List<(int StartIndex, int Length)>();

            for (int i = 0; i < startIndices.Count - 1; ++i)
                Range.Add((startIndices[i], startIndices[i + 1] - startIndices[i]));
            //     Range.Add((startIndices[startIndices.Count - 2], startIndices.Count - startIndices[startIndices.Count - 1]));

            return Range;
        }

        List<List<string[]>> GetRangeRows(List<string[]> rows, List<(int StartIndex, int Length)> range)
        {
            var lst = new List<List<string[]>>();

            foreach (var (StartIndex, Length) in range)
            {
                lst.Add(new List<string[]>());

                for (int i = 0; i < Length; ++i)
                    lst[lst.Count - 1].Add(rows[StartIndex + i]);

            }

            return lst;
        }

        int GetCredit(string input)
        {
            if (int.TryParse(input, out int ex))
                return ex;
            return 0;
        }

        (int Room, Timing timing) GetDayTimeLoc(string days, string hours, string room)
        {
            if (string.IsNullOrWhiteSpace(room))
                room = "-1";

            return (int.Parse(room), new Timing(days, hours));
        }

        public Section GenerateSection(List<string[]> rows)
        {
            var FirstLine = rows[0];
            var section = new Section
            {
                SectionNo = int.Parse(FirstLine[6])
            };

            (section.Room, section.ClassTiming) = GetDayTimeLoc(days: FirstLine[8], hours: FirstLine[9], room: "-1");
            /*
            if (FirstLine[11] == "")
                FirstLine[11] = "  ";
            var s = FirstLine[11].Split(' ');
            */
            (section.CommonHourRoom, section.CommonHourTiming) = (-1, Timing.GenerateEmptyTiming);// GetDayTimeLoc(days: s[0], hours: s[1], room: s[2]);

            section.Teachers.AddRange(from row in rows select TeacherGenerator.GenerateTeacher(row[7]));

            return section;
        }
    }
}
