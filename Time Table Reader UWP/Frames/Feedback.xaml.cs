using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Time_Table_Reader_UWP.Frames
{
    public sealed partial class Feedback : Page
    {
        private readonly Uri NewFeedBackUri = new Uri(@"https://docs.google.com/forms/d/e/1FAIpQLSdx37TrK9C9DkGe2tav2TmAYKuUDjj6yoyAhTJvttmBikwaCA/viewform?usp=sf_link");

        public Feedback()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView1.NavigationStarting += (sender, args) =>
            {
                Progress.IsActive = true;
            };

            webView1.NavigationCompleted += (sender, args) =>
            {
                Progress.IsActive = false;
            };

            combo.Click += (sender, a) =>
            {
                webView1.Navigate(NewFeedBackUri);
            };

            webView1.Navigate(NewFeedBackUri);
        }
    }
}
