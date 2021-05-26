using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace KernX.Network.HTTP
{
    public class HTTPClient
    {
        private static readonly HttpClient Client = new();

        /// <summary>
        ///     Should be mainly domain of the server, which ends with a trailing "/" symbol
        ///     TODO: validate that it ends with "/" and is an actual HTTP endpoint
        /// </summary>
        public string BaseURL { get; init; }

        public string UserAgent { get; init; }

        public async Task<T> GetRequestAsync<T>(string endpoint, Dictionary<string, string> queryParams = null)
        {
            // TODO: validate that endpoint does not start with "/"
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            string queryString = HTTPClientHelpers.GenerateQueryString(queryParams);
            HttpResponseMessage response = await Client.GetAsync($"{BaseURL}{endpoint}{queryString}");

            Stream stream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<T>(stream);
            return content;
        }

        public async Task PostRequestAsync<TRequestBody>(string endpoint, TRequestBody body)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            StringContent data = await HTTPClientHelpers.GenerateRequestBody(body);
            HttpResponseMessage response = await Client.PostAsync($"{BaseURL}{endpoint}", data);
        }

        public async Task<TResponse> PostRequestAsync<TRequestBody, TResponse>(string endpoint, TRequestBody body)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);

            StringContent data = await HTTPClientHelpers.GenerateRequestBody(body);
            HttpResponseMessage response = await Client.PostAsync($"{BaseURL}{endpoint}", data);

            Stream stream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<TResponse>(stream);
            return content;
        }
    }
}