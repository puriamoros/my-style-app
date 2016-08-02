using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyStyleApp.Views.Controls
{
    public class CheckBoxImage : Image
    {
        private bool _initialized;

        public CheckBoxImage()
        {
            this._initialized = false;

            this.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command((o) =>
                    {
                        this.IsChecked = !this.IsChecked;
                    })
                });
        }

        private void Initialize()
        {
            if(!this._initialized)
            {
                if(this.IsChecked && CheckedSource != null)
                {
                    SetValue(SourceProperty, CheckedSource);
                    this._initialized = true;
                }
                else if (!this.IsChecked && UncheckedSource != null)
                {
                    SetValue(SourceProperty, UncheckedSource);
                    this._initialized = true;
                }
            }
        }

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create("Name", typeof(string), typeof(CheckBoxImage), "");

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
            "IsCheckedProperty",
            typeof(bool),
            typeof(CheckBoxImage),
            false,
            BindingMode.TwoWay,
            (bindable, value) => { return (value == null || value is bool); },
            (bindable, oldValue, newValue) => ((CheckBoxImage)bindable).IsChecked = (bool)newValue);

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set
            {
                SetValue(IsCheckedProperty, value);
                if(value)
                {
                    SetValue(SourceProperty, CheckedSource);
                }
                else
                {
                    SetValue(SourceProperty, UncheckedSource);
                }
            }
        }

        public static readonly BindableProperty CheckedSourceProperty = BindableProperty.Create(
            "CheckedSourceProperty",
            typeof(ImageSource),
            typeof(CheckBoxImage),
            null,
            BindingMode.OneWay,
            (bindable, value) => { return (value == null || value is ImageSource); },
            (bindable, oldValue, newValue) =>
            {
                ((CheckBoxImage)bindable).CheckedSource = (ImageSource)newValue;
                ((CheckBoxImage)bindable).Initialize();
            });

        public ImageSource CheckedSource
        {
            get { return (ImageSource)GetValue(CheckedSourceProperty); }
            set { SetValue(CheckedSourceProperty, value); }
        }

        public static readonly BindableProperty UncheckedSourceProperty = BindableProperty.Create(
            "UncheckedSourceProperty",
            typeof(ImageSource),
            typeof(CheckBoxImage),
            null,
            BindingMode.OneWay,
            (bindable, value) => { return (value == null || value is ImageSource); },
            (bindable, oldValue, newValue) =>
            {
                ((CheckBoxImage)bindable).UncheckedSource = (ImageSource)newValue;
                ((CheckBoxImage)bindable).Initialize();
            });

        public ImageSource UncheckedSource
        {
            get { return (ImageSource)GetValue(UncheckedSourceProperty); }
            set { SetValue(UncheckedSourceProperty, value); }
        }
    }
}
