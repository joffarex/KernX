using System;
using System.Collections.Generic;
using System.Net.Http;

namespace KernX.Network.HTTP
{
    public sealed class HTTPRequest
    {
        private readonly StringContent _body;
        private readonly string _endpoint;

        public StringContent Body
        {
            get => _body;
            init => _body = HTTPClientHelpers.GenerateRequestBody(value).GetAwaiter().GetResult();
        }

        public Dictionary<string, string> Headers { get; init; } = null;
        public string ContentType { get; init; } = "application/json";

        public string Endpoint
        {
            get => _endpoint;
            init
            {
                if (!value.StartsWith("/"))
                {
                    _endpoint = value;
                }
                else
                {
                    throw new ArgumentException("Endpoint must not start with \"/\"");
                }
            }
        }

        public Dictionary<string, string> QueryParams { get; init; } = null;
    }
}