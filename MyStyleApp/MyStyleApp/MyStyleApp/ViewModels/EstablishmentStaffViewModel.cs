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
    public class EstablishmentStaffViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;
        private ObservableCollection<Staff> _staffList;

        private IUsersService _usersService;
        private IEstablishmentsService _establishmentsService;

        private Establishment _selectedEstablishment;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand NewStaffCommand { get; private set; }

        public EstablishmentStaffViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService,
            IUsersService usersService,
            IEstablishmentsService establishmentsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Staff>(this.ViewDetailsAsync);
            //this.NewEstablishmentCommand = new Command<Establishment>(this.NewEstablishmentAsync);

            this._usersService = usersService;
            this._establishmentsService = establishmentsService;

            this.InitializeAsync();

            this.SubscribeToMessages();
        }

        protected virtual void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<string>(this, "userLogin", (userType) =>
            {
                MessagingCenter.Subscribe<Staff>(this, "staffChanged", this.OnStaffChanged);
            });
            MessagingCenter.Subscribe<string>(this, "userLogout", (userType) =>
            {
                MessagingCenter.Unsubscribe<Staff>(this, "staffChanged");
            });

            // Need to subscribe on ctor as well since first userLogin message is delivered before this object is created
            MessagingCenter.Subscribe<Staff>(this, "staffChanged", this.OnStaffChanged);
        }

        public async void InitializeAsync()
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    var establishments = await this._establishmentsService.GetOwnerEstablishmentsAsync();

                    this.EstablishmentList = new ObservableCollection<Establishment>(establishments);
                });
        }

        private void OnStaffChanged(Staff staff)
        {
            bool hadThisStaff = false;
            if(this.StaffList != null)
            {
                var result =
                    from oldStaff in StaffList
                    where oldStaff.Id == staff.Id
                    select oldStaff;

                hadThisStaff = result.Count() > 0;
            }

            if(hadThisStaff || (SelectedEstablishment != null && SelectedEstablishment.Id == staff.IdEstablishment))
            {
                this.OnEstablishmentChanged();
            }
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

        private async void ViewDetailsAsync(Staff staff)
        {
            await this.ExecuteBlockingUIAsync(
                async () =>
                {
                    await this.PushNavPageAsync<StaffAccountDetailsViewModel>(async (staffAccountDetailsVM) =>
                    {
                        await staffAccountDetailsVM.Initialize(staff);
                    }
                    );
                });
        }

        private async void NewStaffAsync()
        {

        }

    }
}



