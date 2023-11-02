using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels;

internal abstract class BaseItemListVM<T> : BaseViewModel
{
    private bool _canReloadItems;
    private bool _isStatusTextVisible;
    private ObservableCollection<T>? _itemList;

    private PageItemType _pageItemType;
    private string _statusTextValue;

    // constructor
    protected BaseItemListVM(string baseUrl, MainViewModel mainViewModel, PageItemType pageItemType,
        string noRowsStatus, string loadingStatus)
    {
        ReloadItemsCommand = new RelayCommand(ReloadItems);
        RemoveItemCommand = new RelayCommand<int>(RemoveItem);

        _pageItemType = pageItemType;
        
        User = new User { Id = 1, Name = "Test Name", Surname = "Test Surname" };
        MainVM = mainViewModel;

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
    public string BaseUrl { get; protected set; }

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
    protected abstract void LoadItems();

    protected abstract Task<ApiResponse<object>> RemoveRequest(int itemID, int userID);

    protected async void RemoveItem(int itemID)
    {
        var confirmationDialog = new ConfirmationDialog();

        var message = "Вы уверены, что хотите удалить этот объект?";

        if (confirmationDialog.ShowConfirmationDialog(message) == false)
            return;

        var response = await RemoveRequest(itemID, User.Id);

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