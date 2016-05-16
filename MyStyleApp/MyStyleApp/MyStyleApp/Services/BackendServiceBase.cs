using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services
{
    public abstract class BackendServiceBase
    {
        private HttpService _httpService;

        public BackendServiceBase(HttpService httpService)
        {
            this._httpService = httpService;
        }

        public HttpService HttpService
        {
            get
            {
                return this._httpService;
            }
        }
    }
}
