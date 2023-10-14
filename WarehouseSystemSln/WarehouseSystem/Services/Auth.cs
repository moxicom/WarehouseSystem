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

namespace WarehouseSystem.Services
{
    internal class Auth : BaseRestClient
    {
        public Auth(string baseURL) : base(baseURL) { }

        public async Task<string> VerifyUserRequest(string username, string password)
        {
            try
            {
                var request = new RestRequest("/login", Method.Get)
                {
                    RequestFormat = RestSharp.DataFormat.Json
                };

                request.AddJsonBody(new { username, password });
                var response = await Client.GetAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject jsonResponse = JObject.Parse(response.Content);
                    string token = jsonResponse["token"].ToString();
                    return token;
                }
                else
                {
                    return "<There should be a handler for various errors>";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
