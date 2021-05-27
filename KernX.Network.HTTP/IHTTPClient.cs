using System.Threading.Tasks;

namespace KernX.Network.HTTP
{
    public interface IHTTPClient
    {
        Task<HTTPResponse<TResponse>> GetRequestAsync<TResponse>(HTTPRequest request);
        Task<HTTPResponse> PostRequestAsync<TRequestBody>(HTTPRequest request);
        Task<HTTPResponse<TResponse>> PostRequestAsync<TRequestBody, TResponse>(HTTPRequest request);
        Task<HTTPResponse> PatchRequestAsync<TRequestBody>(HTTPRequest request);
        Task<HTTPResponse<TResponse>> PatchRequestAsync<TRequestBody, TResponse>(HTTPRequest request);
        Task<HTTPResponse> PutRequestAsync<TRequestBody>(HTTPRequest request);
        Task<HTTPResponse<TResponse>> PutRequestAsync<TRequestBody, TResponse>(HTTPRequest request);
        Task<HTTPResponse> DeleteRequestAsync(HTTPRequest request);
        Task<HTTPResponse<TResponse>> DeleteRequestAsync<TResponse>(HTTPRequest request);
    }
}