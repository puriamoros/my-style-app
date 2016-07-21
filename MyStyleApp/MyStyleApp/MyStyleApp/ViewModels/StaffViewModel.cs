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
    public class StaffViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;
        private ObservableCollection<Staff> _staffList;

        private IUsersService _usersService;
        private IEstablishmentsService _establishmentsService;

        private Establishment _selectedEstablishment;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand NewStaffCommand { get; private set; }

        public StaffViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            //this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetailsAsync);
            //this.NewEstablishmentCommand = new Command<Establishment>(this.NewEstablishmentAsync);

            this._usersService = usersService;
            this._establishmentsService = establishmentsService;

            this.InitializeAsync();
        }

        public async void InitializeAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var establishments = await this._establishmentsService.GetMyEstablishmentsAsync();

                    this.EstablishmentList = new ObservableCollection<Establishment>(establishments);
                });
        }

        public ObservableCollection<Establishment> EstablishmentList
        {
            get { return _establishmentList; }
            set { SetProperty(ref _establishmentList, value); }
        }

        public Establishment SelectedEstablishment
        {
            get { return _selectedEstablishment; }
            set
            {
                SetProperty(ref _selectedEstablishment, value);
                this.OnEstablishmentChanged();
            }
        }

        private async void OnEstablishmentChanged()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    if (this.SelectedEstablishment != null)
                    {
                        var staff = await this._usersService.GetStaffAsync(this.SelectedEstablishment);
                         
                        this.StaffList = new ObservableCollection<Staff>(staff);
                    }
                    else
                    {
                        if (this.StaffList != null)
                        {
                            this.StaffList.Clear();
                        }
                    }
                });
        }

        public ObservableCollection<Staff> StaffList
        {
            get { return _staffList; }
            set { SetProperty(ref _staffList, value); }
        }

        private async void ViewDetailsAsync(User staff)
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

        private async void NewStaffAsync()
        {

        }

    }
}



