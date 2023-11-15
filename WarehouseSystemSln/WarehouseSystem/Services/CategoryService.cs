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
        // Constructor
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
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return "Ошибка загрузки заголовка";
            }
            if (response.Data == null) { 
                MessageBox.Show("Заголовок отсутствует у выбранной категории");
                return "";
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
            return new ApiResponse<List<Item>>
            {
                Data = response.Data,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }

        // Makes a request to remove an item with the received itemID
        public async Task<ApiResponse<object>> RemoveItem(int itemID, int userID)
        {
            var request = new RestRequest($"/items/{itemID}", Method.Delete)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID });
            
            var response = await Client.ExecuteAsync(request);
            return new ApiResponse<object>
            {
                Data = null,
                StatusCode = response.StatusCode,
                ErrorMessage = ProcessRequestStatus(response.StatusCode)
            };
        }

        // Makes a request to insert the received item
        public async Task<ApiResponse<object>> InsertItem(int userID, Item item)
        {
            return await SubmitItemData("/items", Method.Post, userID, item);
        }

        // Makes a request to server to update the received item
        public async Task<ApiResponse<object>> UpdateItem(int userID, Item item)
        {
            return await SubmitItemData($"/items/{item.ID}", Method.Put, userID, item);
        }

        // Makes a request to the server with the json body `userID = userID, itemData = item`
        private async Task<ApiResponse<object>> SubmitItemData(string endPoint, Method method, int userID, Item item)
        {
            var request = new RestRequest(endPoint, method)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };
            request.AddJsonBody(new { userID = userID, data = item });

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
