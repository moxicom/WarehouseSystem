using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSystem.Services
{
    internal class BaseRestClient
    {
        public RestClient Client { get; private set; }
        public string BaseURL { get; private set; }

        public BaseRestClient(string baseURL)
        {
            ChangeClientUrl(baseURL);   
        }

        public void ChangeClientUrl(string baseURL)
        {
            BaseURL = baseURL;
            Client = new RestClient(new RestClientOptions { BaseUrl = new Uri(baseURL), MaxTimeout = 20000 });
        }
    }
}
