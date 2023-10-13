using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WarehouseSystem.Models
{
    public class APIService
    {
        private RestClient _client;

        public APIService(string baseURL)
        {
            _client = new RestClient(new RestClientOptions { BaseUrl = new Uri(baseURL), MaxTimeout = 10000 });
        }

        public async Task<string> VerifyUserRequest(string username, string password)
        {
            try
            {
                var request = new RestRequest("/login", Method.Get)
                {
                    RequestFormat = RestSharp.DataFormat.Json
                };
                // should add hash in the future without real password
                request.AddJsonBody(new {username =  username, password = password});
                var response = await _client.GetAsync(request);
            
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject jsonResponse = JObject.Parse(response.Content);
                    string token = jsonResponse["token"].ToString();
                    return token;
                }
                else
                {
                    return "Ошибка подключения к серверу";
                }
            }
            catch (Exception ex)
            {
                return "Превышен лимит ожидания ответа от сервера";
            }

        }
    }
}
