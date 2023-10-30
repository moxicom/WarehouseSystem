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
        public CategoryService CategoryService { get; set; }

        // constructor
        public CategoryVM(int categoryID, string baseUrl, MainViewModel mainVM) : base(baseUrl, mainVM, PageItemType.Item, "Товары отсутствуют",
            "Загрузка...")
        {
            _categoryID = categoryID;
            StatusTextValue = categoryID.ToString();
            IsStatusTextVisible = true;
            CategoryService = new CategoryService(baseUrl);
            ReloadItems();
        }

        // methods
        protected override async void LoadItems()
        {
            var response = await CategoryService.GetItems(_categoryID, _user.Id);

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
    }
}
