using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyStyleApp.Views
{
    public class CustomContentPage: ContentPage
    {
        public CustomContentPage() : base()
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // We just want a navigation bar on iOS
            if (Device.OS != TargetPlatform.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
        }

        // Sets the height of the parent StackLayout to the label height
        // This is necessary when we have some labels ones on the top of others and some
        // of them can need more than one line for their texts
        protected void OnLabelSizeChanged(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                Label label = sender as Label;
                if (label.Parent is StackLayout)
                {
                    StackLayout stackLayout = label.Parent as StackLayout;
                    stackLayout.HeightRequest = label.Height;
                }
            }
        }

        // Disable selection on ListViews (clicks)
        protected void OnItemListSelection(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        // Android switches are huge in some os versions, so check it and resize them if necessary
        protected void OnSwitchSizeChanged(object sender, System.EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android && sender is Switch)
            {
                Switch sw = sender as Switch;
                if (sw.Parent != null && sw.Parent is VisualElement)
                {
                    VisualElement parent = sw.Parent as VisualElement;
                    var maxWidth = parent.Width * 0.20;
                    if (sw.Width > maxWidth)
                    {
                        var scale = maxWidth / sw.Width;
                        sw.Scale = scale;
                    }
                }
            }
        }
    }
}
