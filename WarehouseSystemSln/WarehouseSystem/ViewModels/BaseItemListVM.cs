using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels
{
    internal class BaseItemListVM<T> : BaseViewModel
    {
        protected ObservableCollection<T>? _itemList;
        protected User _user;
        protected string _statusTextValue;
        protected bool _canReloadItems;
        protected bool _isStatusTextVisible;

        protected MainViewModel _mainVM;

        // properties
        protected string NoRowsStatus { get; set; }
        protected string LoadingStatus { get; set; }
        public ICommand ReloadItemsCommand { get; set; }
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
        public BaseItemListVM(string baseUrl, MainViewModel mainViewModel, string noRowsStatus, string loadingStatus)
        {
            ReloadItemsCommand = new RelayCommand(ReloadItems);
            _mainVM = mainViewModel;
            _user = new User(){ Id = 1 };
            NoRowsStatus = noRowsStatus;
            LoadingStatus = loadingStatus;
        }

        // methods
        public void ReloadItems()
        {
            CanReloadItems = false;
            ItemList = null;
            IsStatusTextVisible = true;
            StatusTextValue = LoadingStatus;
            LoadItems();
        }

        protected virtual async void LoadItems() { }
    }
}
