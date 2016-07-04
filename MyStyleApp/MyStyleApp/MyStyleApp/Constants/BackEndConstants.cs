﻿namespace MyStyleApp.Constants
{
    public class BackendConstants
    {
        public const long TIMEOUT_MS = 5000;
        public const string DATETIME_FORMAT = "dd-MM-yyyy HH:mm:ss";
        public static string BASE_URL = "http://192.168.1.34/api.mystyleapp.com/v1/";
        //public static string BASE_URL = "http://10.95.126.126/api.mystyleapp.com/v1/";
        //public static string BASE_URL = "http://10.95.122.67/api.mystyleapp.com/v1/";
        public const string LOGIN_URL = "login";
        public const string REGISTER_URL = "register";
        public const string USERS_URL = "users";
        public const string USER_URL = "users/{0}";
        public const string PASSWORD_URL = "users/{0}/password";
        public const string ME_URL = "users/me";
        public const string SERVICE_CATEGORIES_URL = "servicecategories?lang={0}";
        public const string SERVICES_URL = "services?lang={0}";
        public const string GET_ESTABLISHMENTS_URL = "establishments?idProvince={0}&idService={1}";
        public const string GET_ESTABLISHMENT_URL = "establishments/{0}";
        public const string GET_FAVOURITES_URL = "favourites?idClient={0}";
        public const string ADD_FAVOURITES_URL = "favourites";
        public const string DELETE_FAVOURITES_URL = "favourites/{0}";
        public const string GET_CLIENT_APPOINTMENTS_URL = "appointments?idClient={0}&from={1}";
        public const string GET_ESTABLISHMENT_APPOINTMENTS_URL = "appointments?idEstablishment={0}&from={1}&to={2}";
        public const string APPOINTMENTS_URL = "appointments";
        public const string APPOINTMENT_STATUS_URL = "appointments/{0}/status";
    }
}
