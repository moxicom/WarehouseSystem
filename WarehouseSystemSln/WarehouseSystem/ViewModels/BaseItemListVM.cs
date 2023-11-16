using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;
using WarehouseSystem.Views;

namespace WarehouseSystem.ViewModels;

internal abstract class BaseItemListVM<T> : BaseViewModel
{
    private bool _canReloadItems;
    private bool _isStatusTextVisible;
    private bool _isAddItemButtonVisible;
    private bool _isRemoveButtonVisible;
    private bool _isUpdateButtonVisible;
    private ObservableCollection<T>? _itemList;
    private string _statusTextValue;

    // constructor
    public BaseItemListVM(string baseUrl, MainViewModel mainViewModel,
        string noRowsStatus, string loadingStatus)
    {
        _statusTextValue = string.Empty;

        ReloadItemsCommand = new RelayCommand(ReloadItems);
        RemoveItemCommand = new RelayCommand<int>(RemoveItem);
        AddNewItemCommand = new RelayCommand(AddItem);
        UpdateItemCommand = new RelayCommand<int>(UpdateItem);
        
        //User = new User { Id = 1, Name = "Test Name", Surname = "Test Surname" 
        MainVM = mainViewModel;
        User = mainViewModel.User;
        BaseUrl = baseUrl;
        NoRowsStatus = noRowsStatus;
        LoadingStatus = loadingStatus;
        StatusTextValue = "";
    }

    // properties
    protected string NoRowsStatus { get; set; }
    protected string LoadingStatus { get; set; }
    public ICommand ReloadItemsCommand { get; set; }
    public ICommand RemoveItemCommand { get; set; }
    public ICommand AddNewItemCommand { get; set; }
    public ICommand UpdateItemCommand { get; set; }
    public string BaseUrl { get; protected set; }
    public ItemDialogType ItemDialogType { get; set; }

    public User User { get; }
    public MainViewModel MainVM { get; }

    public string StatusTextValue
    {
        get => _statusTextValue;
        set
        {
            _statusTextValue = value;
            OnPropertyChanged();
        }
    }

    public bool CanReloadItems
    {
        get => _canReloadItems;
        set
        {
            _canReloadItems = value;
            OnPropertyChanged();
        }
    }

    public bool IsStatusTextVisible
    {
        get => _isStatusTextVisible;
        set
        {
            _isStatusTextVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsAddItemButtonVisible
    {
        get => _isAddItemButtonVisible;
        set
        {
            _isAddItemButtonVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsRemoveButtonVisible
    {
        get => _isRemoveButtonVisible;
        set
        {
            _isRemoveButtonVisible = value;
            OnPropertyChanged();
        }
    }

    public bool IsUpdateButtonVisible
    {
        get => _isUpdateButtonVisible;
        set
        {
            _isUpdateButtonVisible = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<T>? ItemList
    {
        get => _itemList;
        set
        {
            _itemList = value;
            OnPropertyChanged();
        }
    }

    // methods
    
    // makes a request to server to get all items of chosen list (item / category)
    protected abstract Task<ApiResponse<List<T>>> LoadItemsRequest();
    // makes a request to server to remove item (item / category)
    protected abstract Task<ApiResponse<object>> RemoveRequest(int itemID);
    // makes a request to server to add new item (item / category)
    protected abstract Task<ApiResponse<object>> AdditionRequest(ItemDialogData dialogData);
    // makes a request to server to update chosen item (item / category)
    protected abstract Task<ApiResponse<object>> UpdatingRequest(int itemID, ItemDialogData dialogData);
    //
    protected abstract ItemDialogData GetItemData(int itemID);

    // loads items of chosen list
    private async Task LoadItems()
    {
        ChangeToLoadingStatus();
        var response = await LoadItemsRequest();
        ProcessItemsLoading(response);
    }

    private void ProcessItemsLoading(ApiResponse<List<T>> response)
    {
        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (response.Data != null)
            {
                ItemList = new ObservableCollection<T>(response.Data);
                IsStatusTextVisible = false;
                ShowAddItemButton();
            }
            else
            {
                StatusTextValue = NoRowsStatus;
                ShowAddItemButton();
            }
        }
        else
        {
            StatusTextValue = response.ErrorMessage;
        }

        CanReloadItems = true;
    }

    public async void ReloadItems()
    {
        ItemList = null;
        await LoadItems();
        ShowButtonsRole();
    }

    private async void RemoveItem(int itemID)
    {
        var confirmationDialog = new ConfirmationDialog();
        var message = "Вы уверены, что хотите удалить этот объект?";
        if (await confirmationDialog.ShowConfirmationDialog(message) == false)
            return;

        ChangeToLoadingStatus();
        ItemList = null;

        var response = await RemoveRequest(itemID);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            MessageBox.Show("Не удалось удалить объект", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        ReloadItems();
    }

    // item addition 
    private async void AddItem()
    {
        var dialogData = ShowItemDialog(ItemDialogMode.Insert);
        if (dialogData == null) 
            return;
        var response = await AdditionRequest(dialogData);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            MessageBox.Show(response.ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        ReloadItems();
    }

    // item updating
    private async void UpdateItem(int itemID)
    {
        var itemData = GetItemData(itemID);
        var dialogData = ShowItemDialog(ItemDialogMode.Update, itemData);
        if (dialogData == null)
            return;
        var response = await UpdatingRequest(itemID, dialogData);
        if (response.StatusCode != HttpStatusCode.OK)
            MessageBox.Show(response.ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        ReloadItems();
    }

    // shows dialog with type and mode, that containts input form, processes its data
    private ItemDialogData ShowItemDialog(ItemDialogMode mode, ItemDialogData? itemData = null)
    { 
        var additionDialog = new ItemDialogView();
        var itemDialogVM = new ItemDialogVM(ItemDialogType, mode);
        additionDialog.DataContext = itemDialogVM;

        if (itemData != null)
        {
            itemDialogVM.Title = itemData.Title;
            itemDialogVM.Description = itemData.Description;
            itemDialogVM.Amount = itemData.Amount;
        }

        var dialogData = new ItemDialogData();
        string errorText = "";
        bool hasData = false;

        itemDialogVM.DialogClosing += (sender, data) =>
        {
            if (data != null)
            {
                hasData = true;
                if (data.Title == "")
                {
                    errorText += "Название не может быть пустым\n";
                }
                if (data.Description == "")
                {
                    errorText += "Описание не может быть пустым\n";
                }
                dialogData = data;
            }
            additionDialog.Close();
        };

        additionDialog.ShowDialog();

        if (!hasData)
            return null;

        if (errorText != "")
        {
            MessageBox.Show(errorText, "Ошибка добавления", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
        else
        {
            return dialogData;
        }
    }

    private void ChangeToLoadingStatus()
    {
        CanReloadItems = false;
        IsStatusTextVisible = true;
        StatusTextValue = LoadingStatus;
        IsAddItemButtonVisible = false;
    }

    private void ShowAddItemButton()
    {
        if (MainVM.User.Role >= UserRoles.Moderator)
        {
            IsAddItemButtonVisible = true;
            return;
        }
        IsAddItemButtonVisible = false;
    }

    private void ShowButtonsRole()
    {
        var role = MainVM.User.Role;
        // add admin panel visibility
        IsUpdateButtonVisible = false;
        IsRemoveButtonVisible = false;
        switch (role)
        {
            case UserRoles.Admin:
                IsUpdateButtonVisible = true;
                IsRemoveButtonVisible = true;
                break;
            case UserRoles.Moderator:
                IsUpdateButtonVisible = true;
                IsRemoveButtonVisible = true;
                break;
            case UserRoles.BasicEmployee:
                IsUpdateButtonVisible = true;
                break;
            case UserRoles.Guest:
                break;
        }
    }
}