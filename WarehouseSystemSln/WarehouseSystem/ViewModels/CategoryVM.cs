using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class CategoryVM : BaseItemListVM<Item>
    {
        // Fields
        private int _categoryID;

        // Properties
        //public CategoryService CategoryService { get; set; }

        // constructor
        public CategoryVM(int categoryID, string baseUrl, MainViewModel mainVM) : base(baseUrl, mainVM, PageItemType.Item, "Товары отсутствуют",
            "Загрузка...")
        {
            _categoryID = categoryID;

            StatusTextValue = categoryID.ToString();
            IsStatusTextVisible = true;
            BaseUrl = baseUrl;
            //CategoryService = new CategoryService(baseUrl);
            ReloadItems();
        }

        // methods
        protected override async void LoadItems()
        {
            CategoryService CategoryService = new CategoryService(BaseUrl);
            var response = await CategoryService.GetItems(_categoryID, User.Id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Check if there is no items
                if (response.Data != null)
                {
                    ItemList = new ObservableCollection<Item>(response.Data);
                    IsStatusTextVisible = false;
                }
                else
                {
                    StatusTextValue = NoRowsStatus;
                }
            }
            else
            {
                StatusTextValue = response.ErrorMessage;
            }

            CanReloadItems = true;
        }

        protected override async Task<ApiResponse<object>> RemoveRequest(int itemID, int userID)
        {
            CategoryService CategoryService = new CategoryService(BaseUrl);
            var response = await CategoryService.RemoveItem(itemID, User.Id);
            return response;
        }

    }
}
