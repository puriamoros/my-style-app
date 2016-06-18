using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStyleApp.Models;
using System.Net.Http;
using MyStyleApp.Constants;

namespace MyStyleApp.Services.Backend
{
    public class FavouritesService : BackendServiceBase, IFavouritesService
    {
        private IList<Establishment> _list;

        public FavouritesService(HttpService httpService):
            base(httpService)
        {
        }

        public async Task<IList<Establishment>> GetFavouritesAsync(int idClient)
        {
            if(this._list == null)
            {
                string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

                this._list = await this.HttpService.InvokeAsync<IList<Establishment>>(
                    HttpMethod.Get,
                    BackendConstants.GET_FAVOURITES_URL,
                    credentials,
                    new object[] { idClient });
            }
            return this._list;
        }

        public async Task AddFavouriteAsync(Favourite favourite)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeWithContentAsync(HttpMethod.Post, BackendConstants.ADD_FAVOURITES_URL, credentials, favourite, null);

            this._list = null;
        }

        public async Task DeleteFavouriteAsync(Favourite favourite)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeAsync(HttpMethod.Delete, BackendConstants.DELETE_FAVOURITES_URL, credentials, new object[] { favourite.Id });

            this._list = null;
        }  
    }
}
