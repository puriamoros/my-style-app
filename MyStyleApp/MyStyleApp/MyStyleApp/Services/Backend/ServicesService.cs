using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Net.Http;
using MyStyleApp.Constants;

namespace MyStyleApp.Services.Backend
{
    public class ServicesService : BackendServiceBase, IServicesService
    {
        private const string LANGUAGE_CODE_TOKEN = "language_code";
        private DateTime _lastUpdate;
        private string _lastLanguaje;
        private List<Service> _serviceList;
        private LocalizedStringsService _localizedStrings;

        public ServicesService(
            HttpService httpService,
            LocalizedStringsService localizedStrings) :
            base(httpService)
        {
            this._localizedStrings = localizedStrings;
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            // Check if there is a better way to get the current app language (without using LocalizedStringsService)
            string language = this._localizedStrings.GetString(LANGUAGE_CODE_TOKEN);

            if (this._serviceList == null || language != this._lastLanguaje ||
                (DateTime.UtcNow - this._lastUpdate) > TimeSpan.FromHours(1))
            {
                string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

                this._serviceList = await this.HttpService.InvokeAsync<List<Service>>(
                    HttpMethod.Get,
                    BackendConstants.SERVICES_URL,
                    credentials,
                    new object[] { language });

                this._lastLanguaje = language;
                this._lastUpdate = DateTime.UtcNow;
            }

            return this._serviceList;
        }
    }
}
