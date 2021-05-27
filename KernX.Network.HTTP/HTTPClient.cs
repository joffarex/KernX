using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KernX.Network.HTTP
{
    public class HTTPClient : IHTTPClient
    {
        private static readonly HttpClient Client = new();
        private readonly string _baseURL;

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
            HttpResponseMessage response = await Client.GetAsync($"{BaseURL}{request.Endpoint}{queryString}");

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> PostRequestAsync<TRequestBody>(HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.PostAsync($"{BaseURL}{request.Endpoint}", request.Body);
            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> PostRequestAsync<TRequestBody, TResponse>(HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.PostAsync($"{BaseURL}{request.Endpoint}", request.Body);
            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> PatchRequestAsync<TRequestBody>(HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.PatchAsync($"{BaseURL}{request.Endpoint}", request.Body);
            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> PatchRequestAsync<TRequestBody, TResponse>
            (HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.PatchAsync($"{BaseURL}{request.Endpoint}", request.Body);

            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> PutRequestAsync<TRequestBody>(HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.PutAsync($"{BaseURL}{request.Endpoint}", request.Body);
            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> PutRequestAsync<TRequestBody, TResponse>
            (HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.PutAsync($"{BaseURL}{request.Endpoint}", request.Body);
            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        public async Task<HTTPResponse> DeleteRequestAsync(HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.DeleteAsync($"{BaseURL}{request.Endpoint}");
            return HTTPResponse.CreateFromClient(response);
        }

        public async Task<HTTPResponse<TResponse>> DeleteRequestAsync<TResponse>
            (HTTPRequest request)
        {
            SetupHeaders(request);

            HttpResponseMessage response = await Client.DeleteAsync($"{BaseURL}{request.Endpoint}");
            return await HTTPResponse<TResponse>.CreateFromClient(response);
        }

        private void SetupHeaders(HTTPRequest request)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(request.ContentType));
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            HTTPClientHelpers.AddHeaders(Client, SharedHeaders);
            HTTPClientHelpers.AddHeaders(Client, request.Headers);
        }
    }
}