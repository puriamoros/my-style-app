using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyStyleApp.Views.Controls
{
    public partial class WaitingOverlayControl : Grid
    {
        public WaitingOverlayControl()
        {
            InitializeComponent();
        }
        
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            "Text",
            typeof(string),
            typeof(WaitingOverlayControl),
            null,
            BindingMode.OneWay,
            (bindable, value) => { return (value == null || value is string); },
            (bindable, oldValue, newValue) => ((WaitingOverlayControl) bindable).label.Text = (string) newValue);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
