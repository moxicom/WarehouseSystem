using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class CategoriesVM : BaseItemListVM<Category>
    {
        // properties
        public ICommand OpenCategoryCommand { get; set; }
        public CategoriesService CategoriesService { get; set; }

        // Constructor
        public CategoriesVM(string baseUrl, MainViewModel mainViewModel) : base(baseUrl, mainViewModel,  PageItemType.Category, "Категории отсутствуют", "Загрузка...")
        {
            OpenCategoryCommand = new RelayCommand<int>(OpenCategory);
            CategoriesService = new CategoriesService(baseUrl);
            ReloadItems();
        }

        // Methods
        protected override async void LoadItems()
        {
            var response = await CategoriesService.GetCategories(User.Id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Check if there is no category
                if (response.Data != null)
                {
                    ItemList = new ObservableCollection<Category>(response.Data);
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
            return new ApiResponse<object>();
        }

        public void OpenCategory(int ID)
        {
            MainVM.OpenCategoryView(ID);
        }
    }
}
