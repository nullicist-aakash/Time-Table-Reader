using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using TimeTableReader;
using Newtonsoft.Json;

namespace Time_Table_Reader_UWP.Frames
{
    public sealed partial class Course_Selector : Page
    {
        Global Ref => Global.Instance;

        public Course_Selector()
        {
            this.InitializeComponent();
        }

        private async void JSONFileSelectClicked(object sender, RoutedEventArgs e)
        {
            Progress.IsIndeterminate = true;
            Progress.Visibility = Visibility.Visible;
            var picker = new FileOpenPicker();

            picker.FileTypeFilter.Add(".json");
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;

            StorageFile file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                Progress.IsIndeterminate = false;
                return;
            }
            string contents = await FileIO.ReadTextAsync(file);
            TimeTable tt = JsonConvert.DeserializeObject<TimeTable>(contents);

            Progress.IsIndeterminate = false;
            Progress.Visibility = Visibility.Collapsed;
            if (tt != null)
            {
                JSON_Location.Text = file.Path;
                Ref.JsonLocation = file.Path;
                Ref.RegisterOnce(tt);
                SuggestionsBox.ItemsSource = Ref.AvailableCourses.Courses;
                Combo_Courses.ItemsSource = Ref.SelectedCourses.Courses;
                List_Courses.ItemsSource = Ref.SelectedCourses.Courses;
            }
        }

        private void Combo_Courses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo_Courses.SelectedItem != null)
                RemoveCourseButton.IsEnabled = true;
        }

        private void RemoveCourseButton_Click(object sender, RoutedEventArgs e)
        {
            var index = Combo_Courses.SelectedIndex;
            Ref.RemoveCourse(index);
            RemoveCourseButton.IsEnabled = false;
        }

        private void SuggestionTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AddCourseButton.IsEnabled = false;
            if (args.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen)
                AddCourseButton.IsEnabled = true;
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                Regex regex = new Regex(sender.Text.ToUpper().Replace(" ", ".*"));
                SuggestionsBox.ItemsSource = from x in Ref.AvailableCourses.Courses
                                             where regex.Match(x.ToString().ToUpper()).Success
                                             select x;
            }
        }

        private void AddCourseClicked(object sender, RoutedEventArgs e)
        {
            string input = SuggestionsBox.Text;

            foreach (var x in Ref.AvailableCourses.Courses)
                if (input == x.ToString())
                {
                    Ref.SelectCourse(x); 
                    break;
                }

            SuggestionsBox.Text = "";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            JSON_Select_Button.Click += JSONFileSelectClicked;
            AddCourseButton.Click += AddCourseClicked;
            RemoveCourseButton.Click += RemoveCourseButton_Click;
            SuggestionsBox.TextChanged += SuggestionTextChanged;
            Combo_Courses.SelectionChanged += Combo_Courses_SelectionChanged;

            if (!string.IsNullOrEmpty(Ref.JsonLocation))
            {
                JSON_Location.Text = Ref.JsonLocation;
                SuggestionsBox.ItemsSource = Ref.AvailableCourses.Courses;
                Combo_Courses.ItemsSource = Ref.SelectedCourses.Courses;
                List_Courses.ItemsSource = Ref.SelectedCourses.Courses;
            }
        }
    }
}
