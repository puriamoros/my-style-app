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


namespace MyStyleApp.ViewModels
{
    class EstablishmentsViewModel : NavigableViewModelBase
    {
        private ObservableCollection<Establishment> _establishmentList;

        public ICommand ViewDetailsCommand { get; private set; }
        public ICommand BookCommand { get; private set; }

        public EstablishmentsViewModel(
            INavigator navigator, 
            IUserNotificator userNotificator, 
            LocalizedStringsService localizedStringsService) : 
            base(navigator, userNotificator, localizedStringsService)
        {
            this.ViewDetailsCommand = new Command<Establishment>(this.ViewDetails);
            this.BookCommand = new Command<Establishment>(this.Book);

            // REMOVE!!!
            FillWithMockData();
        }
        private void FillWithMockData()
        {
            var list = new ObservableCollection<Establishment>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(new Establishment()
                {
                    Id = i,
                    Name = "Name " + i,
                    Address = "Address " + i
                }
                );
            }
            this.EstablishmentsList = list;
        }
        public ObservableCollection<Establishment> EstablishmentsList
        {
            get { return _establishmentList; }
            set { SetProperty(ref _establishmentList, value); }
        }

        private async void ViewDetails(Establishment establishment)
        {
            await this.PushNavPageAsync<EstablishmentDetailsViewModel>((establishmentDetailsVM) => 
                {
                    establishmentDetailsVM.Establishment = establishment;
                }
            );
        }

        private async void Book(Establishment establishment)
        {
            //await this.PushAsync<BookViewModel>((bookVM) =>
            //{
            //    bookVM.Establishment = establishment;
            //}
            //);
        }
    }
}



