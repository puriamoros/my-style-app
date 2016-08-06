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
        Task<List<Establishment>> GetFavouritesAsync();
        Task<Establishment> AddFavouriteAsync(Establishment favourite);
        Task DeleteFavouriteAsync(Establishment favourite);
    }
}
