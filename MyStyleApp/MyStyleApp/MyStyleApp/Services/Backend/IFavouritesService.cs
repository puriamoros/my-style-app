using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IFavouritesService
    {
        Task<IList<Establishment>> GetFavouritesAsync(int idClient);
        Task AddFavouriteAsync(Favourite favourite);
        Task DeleteFavouriteAsync(Favourite favourite);
    }
}
