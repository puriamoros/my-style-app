using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Services;
using XamarinFormsAutofacMvvmStarterKit;

namespace MyStyleApp.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        private HttpService _httpService;

        public StartViewModel(
            INavigator navigator,
            LocalizedStringsService localizedStringsService,
            HttpService httpService) :
            base(navigator, localizedStringsService)
        {
            this._httpService = httpService;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            this.IsBusy = true;
            string apiKey = await this._httpService.GetApiKeyAuthorization();
            if(apiKey == null)
            {
                await this.Navigator.PushAsync<LoginViewModel>();
            }
            else
            {
                await this.Navigator.PushAsync<RegisteredStoresViewModel>();
            }
            this.IsBusy = false;
        }
    }
}
