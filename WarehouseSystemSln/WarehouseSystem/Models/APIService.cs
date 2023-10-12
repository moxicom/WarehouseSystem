using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Models
{
    public class APIService
    {
        private RestClient _client;
        
        public APIService(string baseURL)
        {
            _client = new RestClient(baseURL);
        }

        public async Task<string> GetAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            var response = await _client.GetAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content;
            }
            else
            {
                return null;
            }
        }
    }
}
