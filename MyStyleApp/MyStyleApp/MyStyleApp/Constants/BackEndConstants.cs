using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Constants
{
    public class BackendConstants
    {
        public const long TIMEOUT_MS = 5000;
        public const string BASE_URL = "http://192.168.1.39/api.mystyleapp.com/v1/";
        public const string LOGIN_URL = "login";
        public const string REGISTER_URL = "register";
        public const string USERS_URL = "user";
        public const string USER_URL = "user/{0}";
    }
}
