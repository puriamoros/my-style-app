using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Net.Http;
using MyStyleApp.Constants;
using MyStyleApp.Enums;

namespace MyStyleApp.Services.Backend
{
    public class AppointmentsService : BackendServiceBase, IAppointmentsService
    {
        IUsersService _userService;

        public AppointmentsService(HttpService httpService, IUsersService userService) : base(httpService)
        {
            this._userService = userService;
        }

        public async Task<List<Appointment>> GetClientAppointmentsAsync(DateTime from)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            return await this.HttpService.InvokeAsync<List<Appointment>>(
                HttpMethod.Get,
                BackendConstants.GET_CLIENT_APPOINTMENTS_URL,
                authorization,
                new object[] {
                    this._userService.LoggedUser.Id,
                    from.ToString(BackendConstants.DATETIME_FORMAT) });
        }

        public async Task<List<Appointment>> GetEstablishmentAppointmentsAsync(Establishment establishment, DateTime from, DateTime to)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            return await this.HttpService.InvokeAsync<List<Appointment>>(
                HttpMethod.Get,
                BackendConstants.GET_ESTABLISHMENT_APPOINTMENTS_URL,
                authorization,
                new object[] {
                    establishment.Id,
                    from.ToString(BackendConstants.DATETIME_FORMAT),
                    to.ToString(BackendConstants.DATETIME_FORMAT) });
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            return await this.HttpService.InvokeWithContentAsync<Appointment, Appointment>(
                HttpMethod.Post,
                BackendConstants.APPOINTMENTS_URL,
                authorization,
                appointment,
                null);
        }

        public async Task UpdateAppointmentStatusAsync(Appointment appointment, AppointmentStatusEnum status)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeWithContentAsync<GenericStatus>(
                HttpMethod.Put,
                BackendConstants.APPOINTMENT_STATUS_URL,
                authorization,
                new GenericStatus() { Status = (int) status },
                new object[] { appointment.Id });
        }

        public async Task UpdateAppointmentNotesAsync(Appointment appointment)
        {
            string authorization = await this.HttpService.GetApiKeyAuthorizationAsync();

            //await this.HttpService.InvokeWithContentAsync<GenericStatus>(
            //    HttpMethod.Put,
            //    BackendConstants.APPOINTMENT_STATUS_URL,
            //    authorization,
            //    new GenericStatus() { Status = (int)status },
            //    new object[] { appointment.Id });
        }
    }
}
