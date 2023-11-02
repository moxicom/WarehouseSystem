using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Services
{
    internal abstract class BaseRestClient
    {
        public RestClient Client { get; private set; }
        public string BaseUrl { get; private set; }

        public BaseRestClient(string baseUrl)
        {
            ChangeClientUrl(baseUrl);  
            BaseUrl = baseUrl;
        }

        public void ChangeClientUrl(string baseUrl)
        {
            BaseUrl = baseUrl;
            Client = new RestClient(new RestClientOptions { BaseUrl = new Uri(baseUrl), MaxTimeout = 5000 });
        }
    }
}
