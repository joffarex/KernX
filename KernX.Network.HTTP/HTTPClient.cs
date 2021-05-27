using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KernX.Logger;
using Microsoft.Extensions.Logging;

namespace KernX.Network.HTTP
{
    public class HTTPClient : IHTTPClient
    {
        private static readonly HttpClient Client = new();
        private readonly string _baseURL;
        private readonly ILogger<HTTPClient> _logger;

        public HTTPClient(ILogger<HTTPClient> logger) => _logger = logger ?? KXLogger.CreateLogger<HTTPClient>();

        public string BaseURL
        {
            get => _baseURL;
            init
            {
                if (value.EndsWith("/"))
                {
                    _baseURL = value;
                }
                else
                {
                    throw new ArgumentException("Base URL must end with \"/\"");
                }
            }
        }

        public string UserAgent { get; init; }
        public Dictionary<string, string> SharedHeaders { get; init; } = null;

        public async Task<HTTPResponse<TResponse>> GetRequestAsync<TResponse>(HTTPRequest request)
        {
            SetupHeaders(request);

            string queryString = HTTPClientHelpers.GenerateQueryString(request.QueryParams);

            var url = $"{BaseURL}{request.Endpoint}{queryString}";
            _logger.LogInformation($"Sending - GET {url}");
            HttpResponseMessage response = await Client.GetAsync(url);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> PostRequestAsync<TRequestBody>(HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - POST {url}");

            HttpResponseMessage response = await Client.PostAsync(url, request.Body);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> PostRequestAsync<TRequestBody, TResponse>(HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - POST {url}");

            HttpResponseMessage response = await Client.PostAsync(url, request.Body);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> PatchRequestAsync<TRequestBody>(HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - PATCH {url}");

            HttpResponseMessage response = await Client.PatchAsync(url, request.Body);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> PatchRequestAsync<TRequestBody, TResponse>
            (HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - PATCH {url}");

            HttpResponseMessage response = await Client.PatchAsync($"{BaseURL}{request.Endpoint}", request.Body);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> PutRequestAsync<TRequestBody>(HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - PUT {url}");

            HttpResponseMessage response = await Client.PutAsync(url, request.Body);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> PutRequestAsync<TRequestBody, TResponse>
            (HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - PUT {url}");

            HttpResponseMessage response = await Client.PutAsync(url, request.Body);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> DeleteRequestAsync(HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - DELETE {url}");

            HttpResponseMessage response = await Client.DeleteAsync(url);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> DeleteRequestAsync<TResponse>
            (HTTPRequest request)
        {
            SetupHeaders(request);

            var url = $"{BaseURL}{request.Endpoint}";
            _logger.LogInformation($"Sending - DELETE {url}");

            HttpResponseMessage response = await Client.DeleteAsync(url);
            _logger.LogInformation($"Native HttpClient response\n{response}");

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        private void SetupHeaders(HTTPRequest request)
        {
            _logger.LogInformation("Setting up headers");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(request.ContentType));
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            HTTPClientHelpers.AddHeaders(Client, SharedHeaders);
            HTTPClientHelpers.AddHeaders(Client, request.Headers);
            _logger.LogInformation($"Headers List\n{Client.DefaultRequestHeaders}");
        }
    }
}