using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Input;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class CategoriesVM : BaseViewModel
    {
        // fields
        private const string NoRowsStatus = "Категории отсутствуют";
        private const string LoadingStatus  = "Загрузка...";

        private ObservableCollection<Category>? _categories;
        private readonly User _user;
        private string _statusTextValue;
        private bool _canReloadCategories;
        private bool _isStatusTextVisible;

        private MainViewModel _mainViewModel;


        // properties
        public ICommand ReloadCategoriesCommand { get; set; }
        public ICommand OpenCategoryCommand { get; set; }
        public CategoriesService CategoriesService { get; set; }
        
        public string StatusTextValue
        {
            get => _statusTextValue;
            set { 
                _statusTextValue = value; 
                OnPropertyChanged(nameof(StatusTextValue));
            }
        }

        public bool CanReloadCategories
        {
            get => _canReloadCategories;
            set
            {
                _canReloadCategories = value;
                OnPropertyChanged(nameof(CanReloadCategories));
            }
        }

        public bool IsStatusTextVisible
        {
            get => _isStatusTextVisible;
            set
            {
                _isStatusTextVisible = value;
                OnPropertyChanged(nameof(IsStatusTextVisible));
            }
        }

        public ObservableCollection<Category>? Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        // Constructor
        public CategoriesVM(string baseUrl, MainViewModel mainViewModel)
        {
            ReloadCategoriesCommand = new RelayCommand(ReloadCategories);
            OpenCategoryCommand = new RelayCommand<int>(OpenCategory);

            CategoriesService = new CategoriesService(baseUrl);
            _user = new User() { Id = 1 };
            _mainViewModel = mainViewModel;

            ReloadCategories();

        }

        // Methods
        public void ReloadCategories()
        {
            CanReloadCategories = false;
            Categories = null;
            IsStatusTextVisible = true;
            StatusTextValue = LoadingStatus;
            LoadCategories();
        }

        public async void LoadCategories()
        {
            var response = await CategoriesService.GetCategories(_user.Id);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Check if there is no category
                if (response.Data != null)
                {
                    Categories = new ObservableCollection<Category>(response.Data);
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
            CanReloadCategories = true;
        }

        public void OpenCategory(int ID)
        {
            _mainViewModel.OpenCategoryView();
        }
    }
}
