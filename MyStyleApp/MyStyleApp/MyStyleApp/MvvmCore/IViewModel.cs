using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MvvmCore
{
	public interface IViewModel : INotifyPropertyChanged
	{
		string Title { get; set; }
        INavigation Navigation { get; set; }


        void SetState<T>(Action<T> action) where T : class, IViewModel;
		void OnAppearing ();
		void OnDisappearing ();
		void OnPushed();
		void OnPopped();
	}
}

