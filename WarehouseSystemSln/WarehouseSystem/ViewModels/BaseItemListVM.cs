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
    private ObservableCollection<T>? _itemList;
    private string _statusTextValue;

    // constructor
    protected BaseItemListVM(string baseUrl, MainViewModel mainViewModel,
        string noRowsStatus, string loadingStatus)
    {
        ReloadItemsCommand = new RelayCommand(ReloadItems);
        RemoveItemCommand = new RelayCommand<int>(RemoveItem);
        AddNewItemCommand = new RelayCommand(AddItem);
        
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
    public ICommand AddNewItemCommand { get; set; }
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
    
    // loads and shows a chosen list
    protected abstract void LoadItems();
    // makes a request to server to remove item (item / category)
    protected abstract Task<ApiResponse<object>> RemoveRequest(int itemID);
    // makes a request to server to add new item (item / category)
    protected abstract Task<ApiResponse<object>> AdditionRequest(DialogData formData);


    protected async void RemoveItem(int itemID)
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
            return;
        }

        ReloadItems();
    }

    public void ReloadItems()
    {
        ChangeToLoadingStatus();
        ItemList = null;
        LoadItems();
    }

    public void ChangeToLoadingStatus()
    {
        CanReloadItems = false;
        IsStatusTextVisible = true;
        StatusTextValue = LoadingStatus;
        IsAddItemButtonVisible = false;
    }

    // shows dialog with type and mode, that containts input form, processes its data
    protected DialogData ShowItemDialog(ItemDialogMode mode)
    {
        var additionDialog = new ItemDialogView();
        var itemDialogVM = new ItemDialogVM(ItemDialogType, mode);
        additionDialog.DataContext = itemDialogVM;

        var dialogData = new DialogData();
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
        } else
        {
            return dialogData;
        }
    }

    // item addition 
    protected async void AddItem()
    {
        var formData = ShowItemDialog(ItemDialogMode.Insert);
        if (formData == null) 
            return;
        var response = await AdditionRequest(formData);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            MessageBox.Show(response.ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        ReloadItems();
    }
}