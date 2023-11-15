using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Models;
using WarehouseSystem.ViewModels;

namespace WarehouseSystem.Services
{
    internal class UsersService : BaseRestClient
    {
        // Constructor
        public UsersService(string baseUrl) : base(baseUrl) { }

        // Methods
        public async Task<ApiResponse<List<User>>> GetUsers(int userID)
        {
            var request = new RestRequest($"/users", Method.Get)
            {
                RequestFormat = RestSharp.DataFormat.Json,
            };
            request.AddJsonBody(new { userID });

            var response = await Client.ExecuteAsync<List<User>>(request);
            return new ApiResponse<List<User>> 
            { 
                Data = response.Data,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode) 
            };
        }

        public async Task<ApiResponse<object>> RemoveUser(int senderID, int userIDtoDelete)
        {
            var request = new RestRequest($"/users/{userIDtoDelete}", Method.Delete)
            {
                RequestFormat = RestSharp.DataFormat.Json,
            };
            request.AddJsonBody(new { userID = senderID });

            var response = await Client.ExecuteAsync(request);
            return new ApiResponse<object>
            {
                Data = null,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }

        public async Task<ApiResponse<object>> InsertUser(int senderID, User user)
        {
            var request = new RestRequest($"/users", Method.Post)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID = senderID, data = user});

            var response = await Client.ExecuteAsync(request);
            return new ApiResponse<object>
            {
                Data = null,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }

        public async Task<ApiResponse<object>> UpdateUser(int senderID, User user)
        {
            var request = new RestRequest($"/users/{user.Id}", Method.Put)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID = senderID, data = user });

            var response = await Client.ExecuteAsync(request);
            return new ApiResponse<object>
            {
                Data = null,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }
    }
}
