using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KernX.Network.HTTP
{
    public sealed class HTTPResponse
    {
        public HttpStatusCode StatusCode { get; init; }

        public static HTTPResponse CreateFromClient
            (HttpResponseMessage response) => new() {StatusCode = response.StatusCode};
    }

    public sealed class HTTPResponse<T>
    {
        public T Content { get; init; }
        public HttpStatusCode StatusCode { get; init; }

        public static async Task<HTTPResponse<T>> CreateFromClient(HttpResponseMessage response)
        {
            Stream stream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<T>(stream);

            return new HTTPResponse<T>
            {
                Content = content, StatusCode = response.StatusCode
            };
        }
    }
}