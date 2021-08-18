using Windows.UI.Xaml.Controls;
using TimeTableReader;
using System.Collections.Generic;
using System.Linq;

namespace Time_Table_Reader_UWP.Frames
{
    public sealed partial class Section_Selector : Page
    {
        enum CompreClashState
        {
            NO_COURSE,
            NO_CLASH,
            CLASH
        }

        CompreClashState state = CompreClashState.NO_COURSE;

        public Section_Selector()
        {
            this.InitializeComponent();
            ViewModel = Global.Instance.SelectedCourses;

            if (ViewModel.Courses.Count > 0)
                state = CompreClashState.NO_CLASH;

            IEnumerable<string> compreDuplicates = from x
                                   in
                                       from y
                                       in ViewModel.Courses
                                       where y.CompreTiming != ""
                                       select y.CompreTiming
                                   group x by x into g
                                   where g.Count() > 1
                                   select g.Key;

            if (compreDuplicates.Count() > 0)
                state = CompreClashState.CLASH;

            switch (state)
            {
                case CompreClashState.CLASH: IsCompreClash.Text = "There is clash in compre dates"; break;
                default: IsCompreClash.Text = ""; break;
            }
        }

        public TimeTableModel ViewModel;
    }
}