using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TimeTableReader;
using Time_Table__Windows_.Frames;

namespace Time_Table__Windows_.Frames
{
    /*
    public class AllCourses
    {
        public static AllCourses Instance = new AllCourses();
        private AllCourses() { }

        public readonly List<SelectSectionView> AllCoursesUI = new List<SelectSectionView>();

        public void Update()
        {
            List<Section> selectedsectionstillnow = new List<Section>();
            foreach (var c in AllCoursesUI)
                foreach (var cb in new[] { c.LectureBox, c.TutorialBox, c.PracticalBox})
                    if (cb.SelectedSection != null)
                        selectedsectionstillnow.Add(cb.SelectedSection);

            foreach (var c in AllCoursesUI)
            {

            }
        }
    }

    public class SectionComboBox : ComboBox
    {
        public readonly List<Section> ListOfSections;
        public Section SelectedSection { get; set; }

        public bool ShouldVisible => ListOfSections.Count > 0;

        public bool IsValidSelection => SelectedSection != null || !ShouldVisible;

        public SectionComboBox(List<Section> ListOfSections)
        {
            this.ListOfSections = ListOfSections;
            Update(ListOfSections);
        }

        public void Update(IEnumerable<Section> newList)
        {
            Items.Clear();

            foreach (var x in newList)
                Items.Add(x.ToString());
        }
    }

    public class SelectSectionView : Grid
    {
        public readonly SectionComboBox LectureBox;
        public readonly SectionComboBox TutorialBox;
        public readonly SectionComboBox PracticalBox;
        public readonly Course course;

        public SelectSectionView(Course c)
        {
            LectureBox = new SectionComboBox(c.GeneralClass);
            TutorialBox = new SectionComboBox(c.TutorialClass);
            PracticalBox = new SectionComboBox(c.PracticalClass);
            course = c;

            LectureBox.SelectionChanged += SelectionChanged;
            TutorialBox.SelectionChanged += SelectionChanged;
            PracticalBox.SelectionChanged += SelectionChanged;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as SectionComboBox;
            if (box.SelectedItem == null)
                return;

            box.SelectedSection = box.ListOfSections[box.SelectedIndex];
        }

        public bool AllCoursesSelected => LectureBox.IsValidSelection && TutorialBox.IsValidSelection && PracticalBox.IsValidSelection;
    }
    */

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