using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal abstract class BaseItemListVM<T> : BaseViewModel
    {
        protected ObservableCollection<T>? _itemList;
        protected User _user;
        protected string _statusTextValue;
        protected bool _canReloadItems;
        protected bool _isStatusTextVisible;
        protected PageItemType _pageItemType;

        protected MainViewModel _mainVM;

        // properties
        protected string NoRowsStatus { get; set; }
        protected string LoadingStatus { get; set; }
        public ICommand ReloadItemsCommand { get; set; }
        public ICommand RemoveItemCommand { get; set; }
        public string BaseUrl { get; protected set; }

        public string StatusTextValue
        {
            get => _statusTextValue;
            set
            {
                _statusTextValue = value;
                OnPropertyChanged(nameof(StatusTextValue));
            }
        }

        public bool CanReloadItems
        {
            get => _canReloadItems;
            set
            {
                _canReloadItems = value;
                OnPropertyChanged(nameof(CanReloadItems));
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
        
        public ObservableCollection<T> ItemList
        {
            get => _itemList;
            set
            {
                _itemList = value;
                OnPropertyChanged(nameof(ItemList));
            }
        }

        // constructor
        protected BaseItemListVM(string baseUrl, MainViewModel mainViewModel, PageItemType pageItemType, string noRowsStatus, string loadingStatus)
        {
            ReloadItemsCommand = new RelayCommand(ReloadItems);
            RemoveItemCommand = new RelayCommand<int>(RemoveItem);

            _mainVM = mainViewModel;
            _pageItemType = pageItemType;
            _user = new User(){ Id = 1, Name = "Test Name", Surname = "Test Surname"};

            BaseUrl = baseUrl;
            NoRowsStatus = noRowsStatus;
            LoadingStatus = loadingStatus;
            StatusTextValue = "";
        }

        // methods
        protected abstract void LoadItems();
        
        protected abstract Task<ApiResponse<object>> RemoveRequest(int itemID, int userID);
         
        protected async void RemoveItem(int itemID)
        {
            ConfirmationDialog confirmationDialog = new ConfirmationDialog();
            
            string message = "Вы уверены, что хотите удалить этот объект?";

            if (await confirmationDialog.ShowConfirmationDialog(message) == false)
                return;

            var response = await RemoveRequest(itemID, _user.Id);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Не удалось удалить объект", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ReloadItems();
        }

        public void ReloadItems()
        {
            CanReloadItems = false;
            ItemList = null;
            IsStatusTextVisible = true;
            StatusTextValue = LoadingStatus;
            LoadItems();
        }
    }
}
