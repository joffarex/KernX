using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KernX.Network.HTTP
{
    public static class HTTPClientHelpers
    {
        public static string GenerateQueryString(Dictionary<string, string> queryParams)
        {
            var queryString = new StringBuilder();
            if (queryParams is not null)
            {
                queryString.Append('?');
                string joinedParams = string.Join("&", queryParams.Select(p => $"{p.Key}={p.Value}").ToArray());
                queryString.Append(joinedParams);

                return queryString.ToString();
            }

            return string.Empty;
        }

        public static async Task<StringContent> GenerateRequestBody<T>(T body)
        {
            await using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, body);
            using var streamReader = new StreamReader(memoryStream);
            string serializedBody = await streamReader.ReadToEndAsync();

            return new StringContent(serializedBody);
        }
    }
}