using MyStyleApp.Services;
using System;
using System.Collections.Generic;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public abstract class ViewModelBase : XamarinFormsAutofacMvvmStarterKit.ViewModelBase
    {
        private bool _isBusy;
        private LocalizedStringsService _localizedStringsService;
        private INavigator _navigator;

        public ViewModelBase(
            INavigator navigator,
            LocalizedStringsService localizedStringsService)
        {
            this._navigator = navigator;
            this._localizedStringsService = localizedStringsService;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public INavigator Navigator
        {
            get
            {
                return this._navigator;
            }
        }

        public LocalizedStringsService LocalizedStrings
        {
            get
            {
                return this._localizedStringsService;
            }
        }
    }
}
