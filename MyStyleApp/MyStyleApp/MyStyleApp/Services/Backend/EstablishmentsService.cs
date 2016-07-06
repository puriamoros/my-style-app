using System;
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

        public EstablishmentsService(
            HttpService httpService,
            ProvincesService provincesService) :
            base(httpService)
        {
            this._provincesService = provincesService;
        }

        public async Task<IList<Establishment>> GetEstablishmentsAsync(Province province, Service service)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            IList<Establishment> list = await this.HttpService.InvokeAsync<IList<Establishment>>(
                   HttpMethod.Get,
                   BackendConstants.GET_ESTABLISHMENTS_URL,
                   credentials,
                   new object[] { province.Id, service.Id });

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
                   BackendConstants.GET_ESTABLISHMENT_URL,
                   credentials,
                   new object[] { id });

            establishment.ProvinceName = _provincesService.GetProvince(establishment.IdProvince);

            return establishment;
        }
    }
}
