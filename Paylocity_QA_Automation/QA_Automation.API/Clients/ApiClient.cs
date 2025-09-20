using RestSharp;
using System.Threading.Tasks;
using System.Linq;

namespace QA_Automation.API.Clients
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient(RestClient client)
        {
            _client = client;
        }


        private void LogRequest(RestRequest request)
        {
            Console.WriteLine("----- REQUEST -----");
            Console.WriteLine($"{request.Method} {_client.Options.BaseUrl}{request.Resource}");

            foreach (var header in request.Parameters.Where(p => p.Type == ParameterType.HttpHeader))
            {
                Console.WriteLine($"Header: {header.Name}: {header.Value}");
            }

            var bodyParam = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            if (bodyParam != null)
            {
                Console.WriteLine("Body:");
                Console.WriteLine(bodyParam.Value);
            }
            Console.WriteLine("-------------------");
        }
        
        /*
        private void LogResponse(RestResponse response)
        {
            Console.WriteLine("----- RESPONSE -----");
            Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine("Body:");
            Console.WriteLine(response.Content);
            Console.WriteLine("--------------------");
        }*/


        public async Task<RestResponse<T>> GetAsync<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            LogRequest(request);
            var response = await _client.ExecuteAsync(request);
            return await _client.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> PostAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Post).AddJsonBody(body);
            return await _client.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> PutAsync<T>(string endpoint, object body)
        {
            var request = new RestRequest(endpoint, Method.Put).AddJsonBody(body);
            return await _client.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse> DeleteAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            return await _client.ExecuteAsync(request);
        }
    }
}