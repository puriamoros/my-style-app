﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using MyStyleApp.Constants;
using System.Net.Http;

namespace MyStyleApp.Services.Backend
{
    public class EstablishmentsService : BackendServiceBase, IEstablishmentsService
    {
        private ProvincesService _provincesService;
        private IUsersService _userService;

        public EstablishmentsService(
            HttpService httpService,
            ProvincesService provincesService,
            IUsersService userService) :
            base(httpService)
        {
            this._provincesService = provincesService;
            this._userService = userService;
        }

        public async Task<List<Establishment>> SearchEstablishmentsAsync(Province province, Service service)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            List<Establishment> list = await this.HttpService.InvokeAsync<List<Establishment>>(
                   HttpMethod.Get,
                   BackendConstants.GET_ESTABLISHMENTS_URL,
                   credentials,
                   new object[] { province.Id, service.Id, this._userService.LoggedUser.Id });

            for(int i=0; i<list.Count; i++)
            {
                list[i].ProvinceName = _provincesService.GetProvince(list[i].IdProvince);
            }

            return list;
        }

        public async Task<Establishment> GetEstablishmentAsync(int id)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

           Establishment establishment = await this.HttpService.InvokeAsync<Establishment>(
                   HttpMethod.Get,
                   BackendConstants.ESTABLISHMENT_URL,
                   credentials,
                   new object[] { id });

            establishment.ProvinceName = _provincesService.GetProvince(establishment.IdProvince);

            return establishment;
        }

        public async Task<List<Establishment>> GetOwnerEstablishmentsAsync()
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            List<Establishment> list = await this.HttpService.InvokeAsync<List<Establishment>>(
                   HttpMethod.Get,
                   BackendConstants.GET_OWNER_ESTABLISHMENTS_URL,
                   credentials,
                   new object[] { this._userService.LoggedUser.Id });

            return list;
        }

        public async Task<Establishment> CreateEstablishmentAsync(Establishment establishment)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            var newEstablishment = await this.HttpService.InvokeWithContentAsync<Establishment, Establishment>(
                   HttpMethod.Post,
                   BackendConstants.ESTABLISHMENTS_URL,
                   credentials,
                   establishment,
                   null);

            return newEstablishment;
        }

        public async Task UpdateEstablishmentAsync(Establishment establishment)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeWithContentAsync<Establishment>(
                   HttpMethod.Put,
                   BackendConstants.ESTABLISHMENT_URL,
                   credentials,
                   establishment,
                   new object[] { establishment.Id });
        }

        public async Task UpdateEstablishmentServicesAsync(Establishment establishment)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            ShortenServiceListContainer services = new ShortenServiceListContainer()
            {
                ShortenServices = establishment.ShortenServices
            };

            await this.HttpService.InvokeWithContentAsync<ShortenServiceListContainer>(
                   HttpMethod.Put,
                   BackendConstants.ESTABLISHMENT_SERVICES_URL,
                   credentials,
                   services,
                   new object[] { establishment.Id });
        }
    }
}
