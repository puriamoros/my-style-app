using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCore;
using MyStyleApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyStyleApp.Models;
using Xamarin.Forms;
using MyStyleApp.Services.Backend;
using System.Collections.Specialized;

namespace MyStyleApp.ViewModels
{
    class MyEstablishmentsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentsList;
        private IEstablishmentsService _establishmentsService;       
        
        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand NewEstablishmentCommand { get; private set; }

        public MyEstablishmentsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IEstablishmentsService establishmentsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            //this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            //this.NewEstablishmentCommand = new Command<Establishment>(this.NewEstablishmentAsync);

            this._establishmentsService = establishmentsService;

            this.InitializeAsync();
        }

        public async void InitializeAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var establishments = await this._establishmentsService.GetMyEstablishmentsAsync();
                    
                    this.EstablishmentsList = new ObservableCollection<Establishment> (establishments);
                });

        }

        public ObservableCollection<Establishment> EstablishmentsList
        {
            get { return _establishmentsList; }
            set { SetProperty(ref _establishmentsList, value); }
        }

        private async void ViewDetailsAsync(Establishment establishment)
        {
            //await this.ExecuteBlockingUIAsync(
            //    async () =>
            //    {
            //        //await this.PushNavPageAsync<EstablishmentDetailsViewModel>(async (myEstablishmentDetailsVM) =>
            //        //{
            //        //    // Get establishment details from BE
            //        //    var myEstablishmentDetails = await this._establishmentsService.GetEstablishmentAsync(establishment.Id);
            //        //}
            //        //);
            //    });
        }

        private async void NewEstablishmentAsync()
        {

        }

    }
}



