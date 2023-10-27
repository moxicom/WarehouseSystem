using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class CategoriesVM : BaseViewModel
    {
        private ObservableCollection<Category> _categories;
        private string _baseURL;
        private User _user;

        public CategoriesService CategoriesService { get; set; }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public CategoriesVM(string baseURL)
        {
            CategoriesService = new CategoriesService(baseURL);
            _user = new User() { Id = 1 };

            LoadCategories();
        }

        public async void LoadCategories()
        {
            ApiResponse<List<Category>> response = await CategoriesService.GetCategories(_user.Id);
            Categories = new ObservableCollection<Category>(response.Data);
        }
    }
}
