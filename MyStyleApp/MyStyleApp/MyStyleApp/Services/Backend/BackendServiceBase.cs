namespace MyStyleApp.Services.Backend
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
