using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;
using WarehouseSystem.Views;

namespace WarehouseSystem.ViewModels;

internal class CategoryVM : BaseItemListVM<Item>
{
    // Fields
    private readonly int _categoryID;
    private string _pageTitle;

    // constructor
    public CategoryVM(int categoryID, string baseUrl, MainViewModel mainVM) : base(baseUrl, mainVM, PageItemType.Item,
        "Товары отсутствуют",
        "Загрузка...")
    {
        AddNewItemCommand = new RelayCommand(AddNewItemRequest);
        _categoryID = categoryID;

        StatusTextValue = categoryID.ToString();
        IsStatusTextVisible = true;
        BaseUrl = baseUrl;
        PageTitle = "Загрузка категории...";
        //CategoryService = new CategoryService(baseUrl);
        ReloadItems();
    }

    // Properties
    public ICommand AddNewItemCommand { get; set; }

    public string PageTitle
    {
        get => _pageTitle;
        set
        {
            _pageTitle = value;
            OnPropertyChanged();
        }
    }

    // methods
    protected override async void LoadItems()
    {
        var CategoryService = new CategoryService(BaseUrl);
        var response = await CategoryService.GetItems(_categoryID, User.Id);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            PageTitle = response.Data.Title;
            // Check if there is no items
            if (response.Data.Items != null)
            {
                ItemList = new ObservableCollection<Item>(response.Data.Items);
                IsStatusTextVisible = false;
                IsAddItemButtonVisible = true;
            }
            else
            {
                StatusTextValue = NoRowsStatus;
                IsAddItemButtonVisible = true;
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
        var categoryService = new CategoryService(BaseUrl);
        var response = await categoryService.RemoveItem(itemID, User.Id);
        return response;
    }

    protected void AddNewItemRequest()
    {
        var additionDialog = new ItemDialogView();
        var itemDialogVM = new ItemDialogVM(ItemDialogType.Item, ItemDialogMode.Insert);
        additionDialog.DataContext = itemDialogVM;
        itemDialogVM.DialogClosing += (sender, data) =>
        {
            if (data != null)
            {
                IsStatusTextVisible = true;
                StatusTextValue = data.Title;
            }
            additionDialog.Close();
        };

        additionDialog.ShowDialog();
    }
}