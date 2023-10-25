using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Models;

namespace WarehouseSystem.Services
{
    internal class CategoriesService : BaseRestClient
    {
        
        public CategoriesService(string name) : base(name) { }

        public async Task<ApiResponse<List<Category>>> GetCategories (int userID)
        {
            var request = new RestRequest("/categories", Method.Get) 
            { 
                RequestFormat = RestSharp.DataFormat.Json
            };

            request.AddJsonBody(new { userID });

            var response = await Client.ExecuteAsync<List<Category>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ApiResponse<List<Category>>
                {
                    Data = response.Data,
                    ErrorMessage = "",
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResponse<List<Category>>
            {
                Data = null,
                ErrorMessage = "Пусто",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
