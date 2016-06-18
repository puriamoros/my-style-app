using MyStyleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services.Backend
{
    public interface IEstablishmentsService
    {
        Task<IList<Establishment>> GetEstablishmentsAsync(
            Province province,
            Service service);
    }
}
