using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestSharp;
using WarehouseSystem.Models;

namespace WarehouseSystem.Services
{
    internal class CategoryService : BaseRestClient
    {
        // Categoies
        public CategoryService(string baseUrl) : base(baseUrl) { }

        // Methods

        public async Task<ApiResponse<string>> GetTitle(int categoryID, int userID)
        {
            var request = new RestRequest($"/categories/{categoryID}/title", Method.Get)
            {
                RequestFormat = RestSharp.DataFormat.Json,
            };
            request.AddBody(new { userID });

            var response = await Client.ExecuteAsync<string>(request);
            return new ApiResponse<string> { Data = response.Data, StatusCode = response.StatusCode };
        }

        public string ProcessTitleRequest(ApiResponse<string> response)
        {
            if (response.Data == null) { 
                MessageBox.Show("Заголовок отсутствует у выбранной категории");
                return string.Empty;
            }
            if (response.StatusCode != HttpStatusCode.OK) { 
                return "Ошибка загрузки заголовка";
            }
            return response.Data;
        }

        public async Task<ApiResponse<List<Item>>> GetItems(int categoryID, int userID)
        {
            var request = new RestRequest($"/categories/{categoryID}", Method.Get)
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

        public async Task<ApiResponse<object>> InsertItem(int userID, Item item)
        {
            var request = new RestRequest("/items", Method.Post)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID = userID, itemData = item });         

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
