﻿using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using RestSharp;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;

namespace WarehouseSystem.Services;

internal class Auth : BaseRestClient
{
    public Auth(string baseURL) : base(baseURL)
    {
    }

    public static string Sha256Hash(string value)
    {
        var sb = new StringBuilder();
        using (var hash = SHA256.Create())
        {
            var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

            foreach (var b in result) sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public async Task<ApiResponse<User>> VerifyUserRequest(string username, string password)
    {
        try
        {
            var passwordHash = Sha256Hash(password);

            var request = new RestRequest("/auth") { RequestFormat = RestSharp.DataFormat.Json };
            request.AddJsonBody(new { username, password = passwordHash });

            var response = await Client.ExecuteAsync<User>(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    //MessageBox.Show(response.Data.Name + " " + response.Data.Surname);
                    return new ApiResponse<User>
                    {
                        Data = response.Data,
                        ErrorMessage = "",
                        StatusCode = response.StatusCode
                    };
                }
                case HttpStatusCode.Unauthorized:
                    return new ApiResponse<User>
                    {
                        Data = null,
                        ErrorMessage = "Введенные данные не верны",
                        StatusCode = response.StatusCode
                    };
                default:
                    return new ApiResponse<User>
                    {
                        Data = null,
                        ErrorMessage = "Не удалось подключиться к серверу.",
                        StatusCode = response.StatusCode
                    };
            }
        }
        catch (WebException ex)
        {
            return new ApiResponse<User>
            {
                Data = null,
                ErrorMessage = "Ошибка сети." + ex.Message,
                StatusCode = HttpStatusCode.Conflict
            };
        }
    }
}