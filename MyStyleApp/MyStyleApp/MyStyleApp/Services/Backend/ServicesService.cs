using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Net.Http;
using MyStyleApp.Constants;

namespace MyStyleApp.Services.Backend
{
    public class ServicesService : BackendServiceBase, IServicesService
    {
        public ServicesService(HttpService httpService):
            base(httpService)
        {
        }

        public async Task<IList<Service>> GetServicesAsync()
        {
            string apiKey = await this.HttpService.GetApiKeyAuthorizationAsync();

            return await this.HttpService.InvokeAsync<IList<Service>>(
                HttpMethod.Get,
                BackendConstants.SERVICES_URL,
                apiKey,
                null);
        }
    }
}
