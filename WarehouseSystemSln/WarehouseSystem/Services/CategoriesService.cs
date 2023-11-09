using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Models;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WarehouseSystem.Services
{
    internal class CategoriesService : BaseRestClient
    {
        // Construcor
        public CategoriesService(string name) : base(name) { }
        
        // Methods
        public async Task<ApiResponse<List<Category>>> GetCategories (int userID)
        {
            var request = new RestRequest("/categories", Method.Get) 
            { 
                RequestFormat = RestSharp.DataFormat.Json
            };

            request.AddJsonBody(new { userID });

            var response = await Client.ExecuteAsync<List<Category>>(request);
            return new ApiResponse<List<Category>> 
            {
                Data = response.Data,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }

        public async Task<ApiResponse<object>> InsertCategory(int userID, Category category)
        {
            var request = new RestRequest("/categories", Method.Post)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID = userID, itemData = category });

            var response = await Client.ExecuteAsync(request);
            return new ApiResponse<object>
            {
                Data = null,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }

        public async Task<ApiResponse<object>> RemoveCategory(int userID, int categoryID)
        {
            var request = new RestRequest($"/categories/{categoryID}", Method.Delete)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new {userID = userID});

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
