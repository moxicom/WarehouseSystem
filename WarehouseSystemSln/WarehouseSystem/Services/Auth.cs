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
using WarehouseSystem.Models;

namespace WarehouseSystem.Services
{
    internal class Auth : BaseRestClient
    {
        public Auth(string baseURL) : base(baseURL) { }

        public async Task<ApiResponse<User>> VerifyUserRequest(string username, string password)
        {
            try
            {
                var request = new RestRequest("/auth", Method.Get) { RequestFormat = RestSharp.DataFormat.Json };
                request.AddJsonBody(new { username, password });

                var response = await Client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject jsonResponse = JObject.Parse(response.Content);

                    int id = (int)jsonResponse["ID"];
                    string name = jsonResponse["Name"].ToString();
                    string surname = jsonResponse["Surname"].ToString();
                    UserRole role = (UserRole)((int)jsonResponse["Role"]);

                    User user = new Models.User
                    {
                        Id = id,
                        Name = name,
                        Surname = surname,
                        Role = role
                    };

                    return new ApiResponse<User>
                    {
                        Data = user,
                        ErrorMessage = "",
                        StatusCode = response.StatusCode
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    return new ApiResponse<User>
                    {
                        Data = null,
                        ErrorMessage = "Введенные данные не верны",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    return new ApiResponse<User>
                    {
                        Data = null,
                        ErrorMessage = "Bad status",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return new ApiResponse<User>
                {
                    Data = null,
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.RequestTimeout
                }; 
            }

        }
    }
}
