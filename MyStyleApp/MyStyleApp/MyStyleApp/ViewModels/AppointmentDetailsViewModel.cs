﻿using System.Threading.Tasks;
using MyStyleApp.Services;
using MvvmCore;
using System;
using System.Linq;
using MyStyleApp.Enums;
using MyStyleApp.Models;
using MyStyleApp.Services.Backend;
using System.Collections.ObjectModel;
using MyStyleApp.Exceptions;
using Xamarin.Forms;
using System.Windows.Input;

namespace MyStyleApp.ViewModels
{
    public class AppointmentDetailsViewModel : NavigableViewModelBase
    {
        private IAppointmentsService _appointmentsService;
        private Appointment _appointment;
        private string _notes;

        public ICommand SaveNotesCommand { get; private set; }

        public AppointmentDetailsViewModel(
            INavigator navigator,
            IUserNotificator userNotificator,
            LocalizedStringsService localizedStringsService,
            IUsersService userService,
            IServicesService servicesService,
            IAppointmentsService appointmentsService,
            IEstablishmentsService establishmentsService) :
            base(navigator, userNotificator, localizedStringsService)
        {
            this._appointmentsService = appointmentsService;

            this.SaveNotesCommand = new Command(this.SaveNotesAsync);
        }

        public void Initialize(Appointment appointment)
        {
            this.Appointment = appointment;
            this.Notes = appointment.Notes;
        }

        public Appointment Appointment
        {
            get { return _appointment; }
            set { SetProperty(ref _appointment, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public async void SaveNotesAsync()
        {
            if(this.Notes == null)
            {
                this.Notes = "";
            }
            await this.ExecuteBlockingUIAsync(
                    async () =>
                    {
                        await this._appointmentsService.UpdateAppointmentNotesAsync(this.Appointment, this.Notes);
                        this.Appointment.Notes = this.Notes;

                        //TODO: avisar al usuario de que las notas se han actualizado
                    });
        }

    }
}
