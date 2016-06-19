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
        private IUsersService _userService;

        public FavouritesService(HttpService httpService, IUsersService userService):
            base(httpService)
        {
            this._userService = userService;
        }

        public async Task<IList<Establishment>> GetFavouritesAsync()
        {
            if(this._list == null)
            {
                string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

                this._list = await this.HttpService.InvokeAsync<IList<Establishment>>(
                    HttpMethod.Get,
                    BackendConstants.GET_FAVOURITES_URL,
                    credentials,
                    new object[] { this._userService.LoggedUser.Id });
            }
            return this._list;
        }

        public async Task<Establishment> AddFavouriteAsync(Establishment establishment)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            Favourite favourite = new Favourite()
            {
                Id = 0,
                IdClient = this._userService.LoggedUser.Id,
                IdEstablishment = establishment.Id
            };

            Favourite fav = await this.HttpService.InvokeWithContentAsync<Favourite,Favourite>(
                HttpMethod.Post,
                BackendConstants.ADD_FAVOURITES_URL,
                credentials,
                favourite,
                null);

            this._list = null;

            establishment.IdFavourite = fav.Id;
            return establishment;
        }

        public async Task DeleteFavouriteAsync(Establishment establishment)
        {
            string credentials = await this.HttpService.GetApiKeyAuthorizationAsync();

            await this.HttpService.InvokeAsync(
                HttpMethod.Delete,
                BackendConstants.DELETE_FAVOURITES_URL,
                credentials,
                new object[] { establishment.IdFavourite });

            this._list = null;

            establishment.IdFavourite = 0;
        }  
    }
}
