using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MvvmCore
{
	public abstract class ViewModelBase : IViewModel
	{
		private string _title;

		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void SetState<T>(Action<T> action) where T : class, IViewModel
		{
			action(this as T);
		}

        public INavigation Navigation { get; set; }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(storage, value)) return false;

			storage = value;
			OnPropertyChanged(propertyName);

			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var eventHandler = PropertyChanged;
			if (eventHandler != null)
			{
				eventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public virtual void OnAppearing () { }
		public virtual void OnDisappearing (){ }
		public virtual void OnPushed (){}
		public virtual void OnPopped (){}
	}
}

