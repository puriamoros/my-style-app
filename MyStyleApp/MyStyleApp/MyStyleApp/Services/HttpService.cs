using MyStyleApp.Constants;
using MyStyleApp.Exceptions;
using MyStyleApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyStyleApp.Services
{
    public class HttpService
    {
        private const string API_KEY_FILE_NAME = "apikey.xml";
        private const string API_KEY_AUTHORIZATION = "ApiKey {0}";
        private ObjectStorageService<string> _apiKeyStorageService;
        private string _apiKey;

        public HttpService(ObjectStorageService<string> apiKeyStorageService)
        {
            this._apiKeyStorageService = apiKeyStorageService;
        }

        private string GetQuery(string url, object[] parameters)
        {
            var query = url;
            if (parameters != null)
            {
                query = string.Format(query, parameters);
            }

            query = string.Concat(BackendConstants.BASE_URL, query);
            return query;
        }

        private HttpClient GetHttpClient(string credentials = null)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(BackendConstants.TIMEOUT_MS);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            
            if(credentials != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", credentials);
            }

            return client;
        }

        private HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string requestUri)
        {
            // Prevent cached responses by adding a custom parameter to the url
            string finalRequestUri = requestUri;
            finalRequestUri += (finalRequestUri.Contains("?")) ? "&" : "?";
            finalRequestUri += "_ts=" + DateTime.UtcNow.Ticks;
            return new HttpRequestMessage(method, finalRequestUri);
        }

        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            JsonSerializerSettings serializeSettings = new JsonSerializerSettings();

            // Ignore null values
            serializeSettings.NullValueHandling = NullValueHandling.Ignore;

            // DateTime format "yyyy-MM-dd HH:mm:ss.fff"
            serializeSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });

            return serializeSettings;
        }

        private void LogRequest(
            HttpClient client,
            HttpRequestMessage message,
            Type tContent,
            string contentData)
        {
            // Request Log
            string logMsg =
                "\r\n###### Request ###### (" + message.RequestUri.ToString() + "):\r\n" +
                "HTTP Method: " + message.Method + "\r\n" +
                "HTTP Headers:\r\n" +
                client.DefaultRequestHeaders.ToString();
            // Request Content Log
            if (!string.IsNullOrEmpty(contentData))
            {
                logMsg += "\r\nRequest Content:\r\n";
                object jsonObj = JsonConvert.DeserializeObject(contentData, tContent);
                logMsg += JsonConvert.SerializeObject(jsonObj, this.GetJsonSerializerSettings());
            }
            System.Diagnostics.Debug.WriteLine(logMsg);
        }

        private void LogResponse(
            HttpClient client,
            HttpRequestMessage message,
            Type tResult,
            HttpResponseMessage response,
            string resultData)
        {
            // Response Log
            string logMsg = "\r\n###### Response ######(" + message.RequestUri.ToString() + "):\r\n" +
                "HTTP Method: " + message.Method + "\r\n" +
                "StatusCode: " + ((int)response.StatusCode) + " (" + response.StatusCode.ToString() + ")" +
                ", ReasonPhrase: " + response.ReasonPhrase;
            // Response Content Log
            if (!string.IsNullOrEmpty(resultData))
            {
                logMsg += "\r\nResponse Content:\r\n";
                object jsonObj = null;
                try
                {
                    jsonObj = JsonConvert.DeserializeObject(resultData, tResult);
                }
                catch (JsonSerializationException)
                {
                    jsonObj = JsonConvert.DeserializeObject(resultData, typeof(BackendError));
                }
                catch (Exception)
                {
                }

                if(jsonObj != null)
                {
                    logMsg += JsonConvert.SerializeObject(jsonObj, this.GetJsonSerializerSettings());
                }
                else
                {
                    logMsg += resultData;
                }
            }
            System.Diagnostics.Debug.WriteLine(logMsg);
        }

        private async Task<Tuple<HttpResponseMessage, string>> HttpSendAsync(
            HttpClient client,
            HttpRequestMessage message,
            Type tContent,
            string contentData,
            Type tResult)
        {
            // Log request
            this.LogRequest(client, message, tContent, contentData);

            // Get response
            HttpResponseMessage response = await client.SendAsync(message);
            byte[] data = await response.Content.ReadAsByteArrayAsync();
            var resultData = Encoding.UTF8.GetString(data, 0, data.Length);

            // Log response
            LogResponse(client, message, tResult, response, resultData);

            // Return result
            return new Tuple<HttpResponseMessage, string>(response, resultData);
        }

        private async Task<Tuple<HttpResponseMessage, string>> HttpSendAsync(
            HttpClient client,
            HttpRequestMessage message)
        {
            return await HttpSendAsync(client, message, null, null, null);
        }

        private async Task<Tuple<HttpResponseMessage, string>> HttpSendAsync(
            HttpClient client,
            HttpRequestMessage message,
            Type tContent,
            string contentData)
        {
            return await HttpSendAsync(client, message, tContent, contentData, null);
        }

        private async Task<Tuple<HttpResponseMessage, string>> HttpSendAsync(
            HttpClient client,
            HttpRequestMessage message,
            Type tResult)
        {
            return await HttpSendAsync(client, message, null, null, tResult);
        }

        private TResult DeserializeResponse<TResult>(string content)
            where TResult: class
        {
            var value = JsonConvert.DeserializeObject<TResult>(content);
            return value;
        }

        public async Task SaveApiKeyAuthorizationAsync(string apiKey, bool rememberApiKey)
        {
            this._apiKey = apiKey;
            if (rememberApiKey)
            {
                await this._apiKeyStorageService.SaveToFileAsync(API_KEY_FILE_NAME, apiKey);
            }
        }

        public async Task<string> GetApiKeyAuthorizationAsync()
        {
            if (this._apiKey == null)
            {
                try
                {
                    this._apiKey = await this._apiKeyStorageService.LoadFromFileAsync(API_KEY_FILE_NAME);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return string.Format(API_KEY_AUTHORIZATION, this._apiKey);
        }

        public async Task DeleteApiKeyAuthorizationAsync()
        {
            try
            {
                await this._apiKeyStorageService.DeleteFileAsync(API_KEY_FILE_NAME);
            }
            catch (Exception) { }
        }

        public string GetBasicAuthorization(string email, string password)
        {
            string str = email + ":" + password;
            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        public async Task InvokeAsync(HttpMethod method, string url, string credentials, params object[] parameters)
        {
            HttpClient client = null;
            HttpRequestMessage message = null;

            var query = this.GetQuery(url, parameters);

            client = this.GetHttpClient(credentials);

            message = this.GetHttpRequestMessage(method, query);
            
            var result = await HttpSendAsync(client, message);
            HttpResponseMessage response = result.Item1;
            string resultContentString = result.Item2;

            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Created &&
                response.StatusCode != HttpStatusCode.NoContent)
            {
                var value = DeserializeResponse<BackendError>(resultContentString);
                throw new BackendException(value);
            }
        }

        public async Task<TResult> InvokeAsync<TResult>(HttpMethod method, string url, string credentials, params object[] parameters)
            where TResult: class
        {
            HttpClient client = null;
            HttpRequestMessage message = null;

            var query = this.GetQuery(url, parameters);

            client = this.GetHttpClient(credentials);

            message = this.GetHttpRequestMessage(method, query);

            var result = await HttpSendAsync(client, message, typeof(TResult));
            HttpResponseMessage response = result.Item1;
            string resultContentString = result.Item2;

            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Created &&
                response.StatusCode != HttpStatusCode.NoContent)
            {
                var value = this.DeserializeResponse<BackendError>(resultContentString);
                throw new BackendException(value);
            }

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                return this.DeserializeResponse<TResult>(resultContentString);
            }
            else
            {
                return null;
            }
        }

        public async Task InvokeWithContentAsync<TContent>(
            HttpMethod method,
            string url,
            string credentials,
            TContent content,
            params object[] parameters)
            where TContent : class
        {
            HttpClient client = null;
            HttpRequestMessage message = null;

            var query = this.GetQuery(url, parameters);

            client = this.GetHttpClient(credentials);

            message = this.GetHttpRequestMessage(method, query);

            string requestContentString = JsonConvert.SerializeObject(content, this.GetJsonSerializerSettings());
            message.Content = new StringContent(requestContentString, Encoding.UTF8, "application/json");

            var result = await HttpSendAsync(client, message, typeof(TContent), requestContentString);
            HttpResponseMessage response = result.Item1;
            string resultContentString = result.Item2;

            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Created &&
                response.StatusCode != HttpStatusCode.NoContent)
            {
                var value = DeserializeResponse<BackendError>(resultContentString);
                throw new BackendException(value);
            }
        }

        public async Task<TResult> InvokeWithContentAsync<TResult, TContent>(
            HttpMethod method,
            string url,
            string credentials,
            TContent content,
            params object[] parameters)
             where TContent : class
             where TResult : class
        {
            HttpClient client = null;
            HttpRequestMessage message = null;

            var query = this.GetQuery(url, parameters);

            client = this.GetHttpClient(credentials);

            message = this.GetHttpRequestMessage(method, query);

            string requestContentString = JsonConvert.SerializeObject(content, this.GetJsonSerializerSettings());
            message.Content = new StringContent(requestContentString, Encoding.UTF8, "application/json");

            var result = await HttpSendAsync(client, message, typeof(TContent), requestContentString, typeof(TResult));
            HttpResponseMessage response = result.Item1;
            string resultContentString = result.Item2;

            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Created &&
                response.StatusCode != HttpStatusCode.NoContent)
            {
                var value = DeserializeResponse<BackendError>(resultContentString);
                throw new BackendException(value);
            }

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                return this.DeserializeResponse<TResult>(resultContentString);
            }
            else
            {
                return null;
            }
        }

    }
}
