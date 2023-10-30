using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using WarehouseSystem.Models;

namespace WarehouseSystem.Services
{
    internal class CategoryService : BaseRestClient
    {
        // Categoies
        public CategoryService(string baseUrl) : base(baseUrl) { }

        // Methods
        public async Task<ApiResponse<List<Item>>> GetItems(int categoryID, int userID)
        {
            var request = new RestRequest($"/items/{categoryID}", Method.Get)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };

            request.AddJsonBody(new { userID });

            var response = await Client.ExecuteAsync<List<Item>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ApiResponse<List<Item>>
                {
                    Data = response.Data,
                    ErrorMessage = "",
                    StatusCode = response.StatusCode
                };
            }
            else
            {
                return new ApiResponse<List<Item>>
                {
                    Data = response.Data,
                    ErrorMessage = "Не удалось подключиться к серверу",
                    StatusCode = response.StatusCode
                };
            }
        }

        //public async Task<ApiResponse<object>> DeleteItemse(int categoryID, int userID)
        //{
        //}
    }
}
