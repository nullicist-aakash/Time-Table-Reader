using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Time_Table__Windows_.Frames
{
    public sealed partial class Feedback : Page
    {
        private readonly Uri HomePageUri = new Uri(@"ms-appx-web:///Assets/index.html");
        private readonly Uri NewFeedBackUri = new Uri(@"https://docs.google.com/forms/d/e/1FAIpQLSdx37TrK9C9DkGe2tav2TmAYKuUDjj6yoyAhTJvttmBikwaCA/viewform?usp=sf_link");

        public Feedback()
        {
            this.InitializeComponent();
        }

        private void UpdateComboBox()
        {
            combo.Items.Clear();
            combo.Items.Add("Homepage");
            combo.Items.Add("New Feedback");
        }

        private Uri GetUriFromComboIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return HomePageUri;
                case 1:
                    return NewFeedBackUri;

            }
            throw new Exception();
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

            combo.SelectionChanged += (sender, a) =>
            {
                webView1.Navigate(GetUriFromComboIndex(combo.SelectedIndex));
            };
            UpdateComboBox();
            combo.SelectedIndex = 0;
        }
    }
}
