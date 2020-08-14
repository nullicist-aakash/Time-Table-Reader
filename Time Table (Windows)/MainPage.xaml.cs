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

namespace Time_Table__Windows_
{
    public sealed partial class MainPage : Page
    {
        private readonly LinkedList<(NavigationViewItem item, Type type, string header, Frame frame)> list = new LinkedList<(NavigationViewItem, Type, string, Frame)>();
        public MainPage()
        {
            this.InitializeComponent();
            list.AddLast((SelectCourse, typeof(Frames.Course_Selector), "Select Courses to Consider", NavigationFrame));
            list.AddLast((SelectTiming, typeof(Frames.Section_Selector), "Choose Sections", NavigationFrame));
            list.AddLast((View, typeof(Frames.TimeTableViewer), "Time Table View", NavigationFrame));
            list.AddLast((FeedBack, typeof(Frames.Feedback), "Submit Feedback", NavigationFrame));
        }

        private void ShowAbout(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach (var (item, type, header, frame) in list)
            {
                item.Tapped += (sender, abcd) =>
                {
                    frame.Navigate(type);

                    frame.BackStack.Clear();
                    frame.CacheSize = 0;
                    NavView.Header = header;
                    NavView.SelectedItem = sender;
                };
            }
        }

    }
}