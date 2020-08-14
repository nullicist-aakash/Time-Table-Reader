using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace TimeTableReader
{
    public class CourseComponentsModel
    {
        public ObservableCollection<SectionModel> SectionList { get; } = new ObservableCollection<SectionModel>();

        public SectionModel SelectedModel { get; set; }
        public bool EnableComboBox => SectionList.Count != 0;
        public SolidColorBrush ComboBoxBrush => EnableComboBox && Global.Instance.SelectedCourses.IsClashing(this) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);

        public CourseComponentsModel(List<Section> s)
        {
            foreach (var x in s)
                SectionList.Add(new SectionModel(x));
        }

        public void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = (sender as ComboBox).SelectedIndex;
            if (selection < 0)
                return;
            OnDeselectCourseOperation();
            SelectedModel = SectionList[selection];
            Global.Instance.SelectedCourses.UpdateMap_NewEntry(SelectedModel);
            (sender as ComboBox).BorderBrush = ComboBoxBrush;
        }

        public void OnDeselectCourseOperation()
        {
            if (SelectedModel != null)
            Global.Instance.SelectedCourses.RemoveEntry(SelectedModel);
            SelectedModel = null;
        }

        public int SelectionIndex => SelectedModel == null ? -1 : SectionList.IndexOf(SelectedModel);
    }
}