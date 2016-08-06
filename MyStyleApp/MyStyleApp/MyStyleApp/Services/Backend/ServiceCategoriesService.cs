using MyStyleApp.Constants;
using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public class ServiceCategoriesService : BackendServiceBase, IServiceCategoriesService
    {
        private const string LANGUAGE_CODE_TOKEN = "language_code";
        private DateTime _lastUpdate;
        private string _lastLanguaje;
        private List<ServiceCategory> _serviceCategoryList;
        private LocalizedStringsService _localizedStrings;

        public ServiceCategoriesService(
            HttpService httpService,
            LocalizedStringsService localizedStrings) :
            base(httpService)
        {
            this._localizedStrings = localizedStrings;
        }

        public async Task<List<ServiceCategory>> GetServiceCategoriesAsync()
        {
            // Check if there is a better way to get the current app language (without using LocalizedStringsService)
            string language = this._localizedStrings.GetString(LANGUAGE_CODE_TOKEN);

            if (this._serviceCategoryList == null || language != this._lastLanguaje ||
                (DateTime.UtcNow - this._lastUpdate) > TimeSpan.FromHours(1))
            {
                string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

                this._serviceCategoryList = await this.HttpService.InvokeAsync<List<ServiceCategory>>(
                    HttpMethod.Get,
                    BackendConstants.SERVICE_CATEGORIES_URL,
                    credentials,
                    new object[] { language });

                this._lastLanguaje = language;
                this._lastUpdate = DateTime.UtcNow;
            }
            
            return this._serviceCategoryList;
        }
    }
}
