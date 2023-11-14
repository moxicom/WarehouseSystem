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
        public async Task<ApiResponse<List<User>>> GetUser(int userID)
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
    }
}
