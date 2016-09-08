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
using MyStyleApp.Enums;

namespace MyStyleApp.ViewModels
{
    public class OwnerEstablishmentsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentsList;
        private IEstablishmentsService _establishmentsService;       
        
        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand NewEstablishmentCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public OwnerEstablishmentsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IEstablishmentsService establishmentsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            this.NewEstablishmentCommand = new Command(this.NewEstablishmentAsync);
            this.RefreshCommand = new Command(this.InitializeAsync);

            this._establishmentsService = establishmentsService;

            this.SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                if (userType == UserTypeEnum.Owner.ToString())
                {
                    MessagingCenter.Subscribe<Establishment>(this, "establishmentModified", this.OnEstablishmentModified);
                }
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", (userType) =>
            {
                if (userType == UserTypeEnum.Owner.ToString())
                {
                    MessagingCenter.Unsubscribe<Establishment>(this, "establishmentModified");
                }
            });

            // Need to subscribe on ctor as well since first userLogin message is delivered before this object is created
            MessagingCenter.Subscribe<Establishment>(this, "establishmentModified", this.OnEstablishmentModified);
        }

        private void OnEstablishmentModified(Establishment establishment)
        {
            var establishments = from item in this.EstablishmentsList
                                 where item.Id == establishment.Id
                                 select item;

            if (establishments.Count() > 0)
            {
                // Replace modified establishment
                this.EstablishmentsList.Remove(establishments.ElementAt(0));
                this.EstablishmentsList.Add(establishment);
            }
            else
            {
                // Add new establishment
                this.EstablishmentsList.Add(establishment);
            }

            this.RefreshEstablishmentList();
        }

        private void RefreshEstablishmentList()
        {
            var establishments = new List<Establishment>(this.EstablishmentsList);
            establishments.Sort((one, other) =>
            {
                return one.Name.CompareTo(other.Name);
            });
            this.EstablishmentsList = new ObservableCollection<Establishment>(establishments);
        }

        public async void InitializeAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var establishments = await this._establishmentsService.GetOwnerEstablishmentsAsync();
                    establishments.Sort((one, other) =>
                    {
                        return one.Name.CompareTo(other.Name);
                    });
                    this.EstablishmentsList = new ObservableCollection<Establishment> (establishments);

                    MessagingCenter.Send<string>("", "ownerEstablishmentsRefreshed");
                });

        }

        public ObservableCollection<Establishment> EstablishmentsList
        {
            get { return _establishmentsList; }
            set { SetProperty(ref _establishmentsList, value); }
        }

        private async void ViewDetailsAsync(Establishment establishment)
        {
            await this.ExecuteBlockingUIAsync(async () =>
                {
                    // Get establishment details from BE
                    var establishmentDetails = await this._establishmentsService.GetEstablishmentAsync(establishment.Id);
    
                    await this.PushNavPageAsync<OwnerEstablishmentDetailsViewModel>((ownerEstablishmentDetailsVM) =>
                    {
                        ownerEstablishmentDetailsVM.Initialize(establishmentDetails);
                    });
                });
        }

        private async void NewEstablishmentAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<CreateEstablishmentViewModel>((CreateEstablishmentVM) =>
                    {
                        CreateEstablishmentVM.Initialize();
                    }
                    );
                });
        }

    }
}



