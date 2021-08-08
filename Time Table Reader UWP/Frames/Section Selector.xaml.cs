using Windows.UI.Xaml.Controls;
using TimeTableReader;

namespace Time_Table_Reader_UWP.Frames
{
    public sealed partial class Section_Selector : Page
    {
        public Section_Selector()
        {
            this.InitializeComponent();
            ViewModel = Global.Instance.SelectedCourses;
        }

        public TimeTableModel ViewModel;
    }
}