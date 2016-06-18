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
        public EstablishmentsService(
            HttpService httpService) :
            base(httpService)
        {
        }

        public async Task<IList<Establishment>> GetEstablishmentsAsync(Province province, Service service)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            IList<Establishment> list = await this.HttpService.InvokeAsync<IList<Establishment>>(
                   HttpMethod.Get,
                   BackendConstants.GET_ESTABLISHMENTS_URL,
                   credentials,
                   new object[] { province.Id, service.Id });

            return list;
        }
    }
}
