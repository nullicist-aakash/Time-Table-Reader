using System;
using System.Collections.Generic;
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
using Windows.UI.Text;
using Windows.UI;

namespace Time_Table__Windows_.Frames
{
    public sealed partial class TimeTableViewer : Page
    {
        public TimeTableModel Ref => Global.Instance.SelectedCourses;

        readonly bool[,] arr = new bool[6, 13];

        public TimeTableViewer()
        {
            this.InitializeComponent();
        }

        public Button GetView((string Title, string Type, List<Teacher> Teachers, DayOfWeek WeekDay, uint Hour, uint hours, string Room) Entry)
        {
            StackPanel panel = new StackPanel { HorizontalAlignment = HorizontalAlignment.Stretch };

            panel.Children.Add(new TextBlock
            {
                Text = Entry.Title,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            });

            panel.Children.Add(new TextBlock
            {
                Text = Entry.Type.ToString(),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextWrapping = TextWrapping.Wrap,
                HorizontalTextAlignment = TextAlignment.Center
            });

            Button button = new Button
            {
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.Transparent),
                Content = panel,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };

            Grid.SetRow(button, (int)Entry.WeekDay + 1);
            Grid.SetColumn(button, (int)Entry.Hour);
            Grid.SetColumnSpan(button, (int)Entry.hours);

            for (int i = 0; i < Entry.hours; ++i)
            {
                arr[(int)Entry.WeekDay - 1, (int)Entry.Hour + i - 1] = true;
            }

            Flyout flyout = new Flyout
            {
                OverlayInputPassThroughElement = button
            };
            button.Click += (sender, e) =>
            {
                flyout.ShowAt(sender as FrameworkElement);
            };

            flyout.Content = new TextBlock
            {
                Text = string.Format(
                    "{0} - {1}\n" +
                    "Teacher\t: {2}\n" +
                    "Room\t: {3}\n",
                    Entry.Title,
                    Entry.Type.ToString(),
                    string.Join(',', from t in Entry.Teachers select t.Name),
                    Entry.Room)
            };

            return button;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach (var x in GetAllTimings())
                _View.Children.Add(GetView(x));
            // _View.Children.Add(GetView(("AB", "Practica", new List<Teacher> { new Teacher { Name = "Kannan" } }, DayOfWeek.Monday, 2, 2, "1512")));

            for (int i = 2; i < 8; ++i)
            {
                for (int j = 1; j < 13; ++j)
                {
                    if (arr[i - 2, j - 1])
                        continue;

                    Button b = new Button
                    {
                        BorderBrush = new SolidColorBrush(Colors.Gray),
                        BorderThickness = new Thickness(1),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Background = new SolidColorBrush(Colors.Transparent)
                    };

                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);

                    _View.Children.Add(b);
                }
            }
        }

        public IEnumerable<(string Title, string Type, List<Teacher> Teachers, DayOfWeek WeekDay, uint Hour, uint hours, string Room)> GetAllTimings()
        {
            foreach (var course in Ref.Courses)
            {
                var model = course.GeneralComponent.SelectedModel;
                if (model != null)
                    foreach (var (WeekDay, Hour, hours) in GetTimings(model.ClassTiming))
                        yield return (course.Course_Name, "Lecture", model.Teachers, WeekDay, Hour, hours, model.RoomNo);

                model = course.PracticalComponent.SelectedModel;
                if (model != null)
                    foreach (var (WeekDay, Hour, hours) in GetTimings(model.ClassTiming))
                        yield return (course.Course_Name, "Practical", model.Teachers, WeekDay, Hour, hours, model.RoomNo);

                model = course.TutorialComponent.SelectedModel;
                if (model != null)
                    foreach (var (WeekDay, Hour, hours) in GetTimings(model.ClassTiming))
                        yield return (course.Course_Name, "Tutorial", model.Teachers, WeekDay, Hour, hours, model.RoomNo);
            }
        }

        private IEnumerable<(DayOfWeek WeekDay, uint Hour, uint hours)> GetTimings(Timing t)
        {
            var map = new TimingMap(t);

            for (int i = 0; i < map.Record.Length; ++i)
                if (map.Record[i] != 0)
                {
                    var lst = new List<uint>();

                    // Add
                    foreach (var entry in Timing.Hours(map.Record[i]))
                        lst.Add((uint)entry);

                    // Convert1
                    var lst2 = new uint[16];
                    uint lastIndex = lst[0];
                    lst2[lastIndex]++;
                    for (int j = 1; j < lst.Count; ++j)
                        lst2[lst[j] == lst[j - 1] + 1 ? lastIndex : lastIndex = lst[j]]++;

                    // return
                    for (uint j = 0; j < lst2.Length; ++j)
                        if (lst2[j] > 0)
                            yield return ((DayOfWeek)i, j, lst2[j]);
                }
        }
    }
}