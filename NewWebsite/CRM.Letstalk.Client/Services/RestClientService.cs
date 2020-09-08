namespace CRM.Letstalk.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services.Interfaces;
    using Newtonsoft.Json;
    using RestSharp;

    public class RestClientService : IRestClientService
    {
        private const string ContentType = "text/plain";
        private const int ExpiredTokenPeriodInMiliseconds = 5000;
        private readonly IRestClient restClient;
        private readonly IJwtService jwtService;
        private readonly BotConfig botConfig;

        public RestClientService(IRestClient restClient, IJwtService jwtService, BotConfig botConfig)
        {
            this.restClient = restClient;
            this.jwtService = jwtService;
            this.botConfig = botConfig;
        }

        public T ExecuteGet<T>(string endPoint, IDictionary<string, object> data = null)
            where T : new()
            => Execute<T>(endPoint, Method.GET, data);

        public T ExecutePost<T>(string endPoint, IDictionary<string, object> data)
            where T : new()
            => Execute<T>(endPoint, Method.POST, data);

        public T ExecutePut<T>(string endPoint, IDictionary<string, object> data)
            where T : new()
            => Execute<T>(endPoint, Method.PUT, data);

        public T ExecuteDelete<T>(string endPoint, IDictionary<string, object> data)
            where T : new()
            => Execute<T>(endPoint, Method.DELETE, data);

        private T Execute<T>(string endPoint, Method method, IDictionary<string, object> data)
            where T : new()
        {
            ValidateEndPoint(endPoint);
            var request = CreateRequest(endPoint, method);

            if (data != null)
            {
                var encodedData = Encode(data);
                request.AddParameter(ContentType, encodedData, ParameterType.RequestBody);
            }

            var response = restClient.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }

            throw new InvalidOperationException(response.ErrorMessage);
        }

        private RestRequest CreateRequest(string endPoint, Method method)
        {
            var request = new RestRequest(endPoint, method);

            request.AddHeader("Content-Type", ContentType);
            request.AddHeader("Authorization", $"Bearer {EncodeBase64(botConfig.ApiKey)}");

            return request;
        }

        private async Task<T> ExecuteAsync<T>(string endPoint, Method method, IDictionary<string, object> data)
            where T : new()
        {
            ValidateEndPoint(endPoint);

            var request = CreateRequest(endPoint, method);

            if (data != null)
            {
                var encodedData = Encode(data);
                request.AddParameter(ContentType, encodedData, ParameterType.RequestBody);
            }

            var response = await restClient.ExecuteTaskAsync<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }

            var errorResponseJson = JsonConvert.SerializeObject(response);

            throw new InvalidOperationException(errorResponseJson);
        }

        private static void ValidateEndPoint(string endPoint)
        {
            if (string.IsNullOrWhiteSpace(endPoint))
            {
                throw new ArgumentNullException(nameof(endPoint));
            }
        }

        public Task<T> ExecuteGetAsync<T>(string endPoint, IDictionary<string, object> data = null) where T : new()
            => ExecuteAsync<T>(endPoint, Method.GET, data);

        public Task<T> ExecutePostAsync<T>(string endPoint, IDictionary<string, object> data) where T : new()
            => ExecuteAsync<T>(endPoint, Method.POST, data);

        public Task<T> ExecutePutAsync<T>(string endPoint, IDictionary<string, object> data) where T : new()
            => ExecuteAsync<T>(endPoint, Method.PUT, data);

        public Task<T> ExecuteDeleteAsync<T>(string endPoint, IDictionary<string, object> data) where T : new()
            => ExecuteAsync<T>(endPoint, Method.DELETE, data);

        public static IRestClient CreateDefaultClient(string apiEndpoint, string proxyAddress, bool byPassProxyLocal, bool isUseProxy = false)
        {
            if (isUseProxy)
            {
                return new RestClient
                {
                    BaseUrl = new Uri(apiEndpoint),
                    Proxy = new WebProxy(proxyAddress, byPassProxyLocal)
                };
            }

            return new RestClient { BaseUrl = new Uri(apiEndpoint) };
        }

        private string Encode(IDictionary<string, object> data)
        {
            var totalMilisecondNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            data.Add("iat", totalMilisecondNow);
            data.Add("exp", totalMilisecondNow + ExpiredTokenPeriodInMiliseconds);

            var result = jwtService.Encode(botConfig.SecureKey, data);
            return result;
        }

        private static string EncodeBase64(string input)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }
}