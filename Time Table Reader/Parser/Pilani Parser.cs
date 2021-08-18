using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Generator
{
    class PilaniRow
    {
        List<string> Row { get; } = new List<string>();

        public static int ClassTypeIndex => 2;
        public static int SectionIndex => 6;

        public PilaniRow(string[] row)
        {
            foreach (var x in row)
                Row.Add(x);
        }

        static int GetCredit(string input)
        {
            if (int.TryParse(input, out int ex))
                return ex;
            return 0;
        }
        static Timing GetTiming(string daytime)
        {
            if (string.IsNullOrWhiteSpace(daytime))
                return Timing.GenerateEmptyTiming;

            var splitted = daytime.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var split_indices = new List<int>();

            #region SplitIndices
            split_indices.Add(0);

            for (int i = 1; i < splitted.Length; ++i)
            {
                bool prev_time = int.TryParse(splitted[i - 1], out _);
                bool cur_time = int.TryParse(splitted[i], out _);

                if ((prev_time && !cur_time) || (!prev_time && cur_time))
                    split_indices.Add(i);
            }
            split_indices.Add(splitted.Length);

            #endregion

            var entries = new List<string>[2]
            {
                new List<string>(),
                new List<string>()
            };

            for (int i = 0; i < split_indices.Count - 1; ++i)
            {
                int start_index = split_indices[i];
                int end_index = split_indices[i + 1];
                var entrs = new List<string>();
                for (int j = start_index; j < end_index; ++j)
                    entrs.Add(splitted[j]);

                entries[i % 2].Add(string.Join(" ", entrs));
            }

            var lst = new List<(string days, string hours)>();
            for (int i = 0; i < entries[0].Count; ++i)
                lst.Add((entries[0][i], entries[1][i]));

            return new Timing(lst);
        }

        public int COMCOD => int.Parse(Row[0]);
        public string CourseNo_Dept => Row[1].Split(' ')[0];
        public string CourseNo_Id => Row[1].Split(' ')[1];
        public string Course_Name => Row[2];
        public int LectureUnits => GetCredit(Row[3]);
        public int PracticalUnits => GetCredit(Row[4]);
        public int TotalUnits => GetCredit(Row[5]);
        public Section Section => new Section() { SectionNo = int.Parse((Row[6].EndsWith("N") || Row[6].EndsWith("M")) ? Row[6].Substring(1, Row[6].Length - 2) :  Row[6].Substring(1)) };
        public string TeacherName => Row[7];
        public int RoomNo
        {
            get
            {
                var room = Row[8];
                if (string.IsNullOrWhiteSpace(room))
                    room = "-1";
                return int.Parse(room);
            }
        }
        public Timing Timing => GetTiming(Row[9]);
        public int CommonRoomVenue
        {
            get
            {
                if (Row[10] == "")
                    Row[10] = "  ";
                var room = Row[10].Split(' ')[0];
                if (string.IsNullOrWhiteSpace(room) || !int.TryParse(room, out _))
                    room = "-1";
                return int.Parse(room);
            }
        }
        public Timing CommonRoomTiming
        {
            get
            {
                if (Row[10] == "")
                    Row[10] = "  ";
                var s = Row[10].Split(' ');
                return new Timing(s[0], s[1]);
            }
        }
        public string CompreTiming => Row[11];
    }

    class Pilani_Parser : ExcelToTimeTable
    {
        static List<int> FindStartIndices(List<string[]> rows, int CheckIndex = 0)
        {
            var StartIndices = new List<int>();

            for (int i = 0; i < rows.Count; ++i)
                if (rows[i][CheckIndex] != "")
                    StartIndices.Add(i);
            StartIndices.Add(rows.Count);
            return StartIndices;
        }

        static List<(int StartIndex, int Length)> FindRanges(List<int> startIndices)
        {
            var Range = new List<(int StartIndex, int Length)>();

            for (int i = 0; i < startIndices.Count - 1; ++i)
                Range.Add((startIndices[i], startIndices[i + 1] - startIndices[i]));
            //     Range.Add((startIndices[startIndices.Count - 2], startIndices.Count - startIndices[startIndices.Count - 1]));

            return Range;
        }

        static List<List<string[]>> GetRangeRows(List<string[]> rows, List<(int StartIndex, int Length)> range)
        {
            var lst = new List<List<string[]>>();

            foreach (var (StartIndex, Length) in range)
            {
                lst.Add(new List<string[]>());

                for (int i = 0; i < Length; ++i)
                    lst[lst.Count -1].Add(rows[StartIndex + i]);
            }

            return lst;
        }

        private static List<List<string[]>> SplitCourses(List<string[]> allRows)
        {
            if (allRows.Count == 0)
                return new List<List<string[]>>();
            if (allRows.Count == 1)
                return new List<List<string[]>> { allRows };

            var _startIndices = FindStartIndices(allRows);
            var _ranges = FindRanges(_startIndices);

            return GetRangeRows(allRows, _ranges);
        }

        public static Section GenerateSection(List<string[]> rows)
        {
            PilaniRow row = new PilaniRow(rows[0]);
            var section = row.Section;
            section.Room = row.RoomNo;
            section.ClassTiming = row.Timing;
            section.CommonHourRoom = row.CommonRoomVenue;
            section.CommonHourTiming = row.CommonRoomTiming;

            section.Teachers.AddRange(from r in rows select TeacherGenerator.GenerateTeacher(new PilaniRow(r).TeacherName));

            return section;
        }

        private static IEnumerable<Section> GenerateClassEntries(List<string[]> classes)
        {
            classes[0][PilaniRow.SectionIndex] = "L1";
            var ClassIndices = FindStartIndices(classes, 6);
            var Ranges = FindRanges(ClassIndices);
            var SplittedRows = GetRangeRows(classes, Ranges);

            foreach (var section in SplittedRows)
                yield return GenerateSection(section);
        }

        private static Course GenerateCourses(List<string[]> courseEntry)
        {
            PilaniRow row = new PilaniRow(courseEntry[0]);

            var course = new Course
            {
                COMCOD = row.COMCOD,
                CourseNo_Dept = row.CourseNo_Dept,
                CourseNo_Id = row.CourseNo_Id,
                Course_Name = row.Course_Name,
                Credits = new Credit
                {
                    Lecture = row.LectureUnits,
                    Practical = row.PracticalUnits,
                    Units = row.TotalUnits
                },
                CompreTiming = row.CompreTiming
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

                var sectionType = SplittedRows[i][0][PilaniRow.ClassTypeIndex].ToUpper();

                if (sectionType == "TUTORIAL")
                    course.TutorialClass.AddRange(classes);
                else if (sectionType == "PRACTICAL")
                    course.PracticalClass.AddRange(classes);
                else
                    throw new Exception("Help me!! I was not designed to do this. Something bad happened in course " + course.Course_Name);
            }
            return course;
        }

        protected override TimeTable DoGenerateTimeTable(List<string[]> contents)
        {
            var SplittedCourses = SplitCourses(contents);

            var tt = new TimeTable();
            tt.Courses.AddRange(from x in SplittedCourses select GenerateCourses(x));

            return tt;
        }
    }
}
