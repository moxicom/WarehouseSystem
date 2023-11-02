using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public async Task<ApiResponse<GetCategoryResponseData>> GetItems(int categoryID, int userID)
        {
            var request = new RestRequest($"/categories/{categoryID}", Method.Get)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };

            request.AddJsonBody(new { userID });

            var response = await Client.ExecuteAsync<GetCategoryResponseData>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ApiResponse<GetCategoryResponseData>
                {
                    Data = response.Data,
                    ErrorMessage = "",
                    StatusCode = response.StatusCode
                };
            }
            else
            {
                return new ApiResponse<GetCategoryResponseData>
                {
                    Data = response.Data,
                    ErrorMessage = "Не удалось подключиться к серверу",
                    StatusCode = response.StatusCode
                };
            }
        }

        public async Task<ApiResponse<object>> RemoveItem(int itemID, int userID)
        {
            var request = new RestRequest($"/items/{itemID}", Method.Delete)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID });
            
            var response = await Client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new ApiResponse<object>
                {
                    Data = null,
                    ErrorMessage = "",
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResponse<object>
                {
                    Data = null,
                    ErrorMessage = "Не удалось подключиться к серверу",
                    StatusCode = response.StatusCode
                };
        }
    }
}
