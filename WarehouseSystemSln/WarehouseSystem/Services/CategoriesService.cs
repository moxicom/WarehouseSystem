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

        // Makes a request to insert received category
        public async Task<ApiResponse<object>> InsertCategory(int userID, Category category)
        {
            return await SubmitCategoryData("/categories", Method.Post, userID, category);
        }

        // Makes a request to server to update received category
        public async Task<ApiResponse<object>> UpdateCategory(int userID, Category category)
        {
            return await SubmitCategoryData($"/categories/{category.ID}", Method.Put, userID, category);
        }

        // Makes a request to the server with the json body `userID = userID, itemData = category`.
        private async Task<ApiResponse<object>> SubmitCategoryData(string endPoint, Method method, int userID, Category category) 
        {
            var request = new RestRequest(endPoint, method)
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

    }
}
