using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
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
    private string _pageTitle = string.Empty;
    private const string _titleLoadingText = "Загрузка категории...";

    // constructor
    public CategoryVM(int categoryID, string baseUrl, MainViewModel mainVM) : base(baseUrl, mainVM,
        "Товары отсутствуют",
        "Загрузка...")
    {
        _categoryID = categoryID;

        StatusTextValue = categoryID.ToString();
        IsStatusTextVisible = true;
        BaseUrl = baseUrl;
        PageTitle = _titleLoadingText;
        ItemDialogType = ItemDialogType.Item;
        ReloadItems();
    }

    // properties
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
    protected override async Task<ApiResponse<List<Item>>> LoadItemsRequest()
    {
        var categoryService = new CategoryService(BaseUrl);
        PageTitle = _titleLoadingText;
        var response = await categoryService.GetItems(_categoryID, User.Id);
        await LoadTitle();
        return response;
    }

    protected async Task LoadTitle()
    {
        var categoryService = new CategoryService(BaseUrl);
        var response = await categoryService.GetTitle(_categoryID, User.Id);
        var processResult = categoryService.ProcessTitleRequest(response);
        if (processResult != string.Empty)
            PageTitle = processResult;
    }

    protected override async Task<ApiResponse<object>> RemoveRequest(int itemID)
    {
        var categoryService = new CategoryService(BaseUrl);
        var response = await categoryService.RemoveItem(itemID, User.Id);
        return response;
    }

    protected override async Task<ApiResponse<object>> AdditionRequest(DialogData dialogData)
    {
        var categoryService = new CategoryService(BaseUrl);
        var item = ProcessDialogData(itemID: 0, dialogData);
        var response = await categoryService.InsertItem(User.Id, item);
        return response;
    }

    protected override async Task<ApiResponse<object>> UpdatingRequest(int itemID, DialogData dialogData)
    {
        var categoryService = new CategoryService(BaseUrl);
        var item = ProcessDialogData(itemID, dialogData);
        var response = await categoryService.UpdateItem(User.Id, item);
        return response;
    }

    protected override DialogData GetItemData(int itemID)
    {
        if (ItemList == null)
            return null;

        Item? item = ItemList.FirstOrDefault(item => item.ID == itemID);
        if (item == null)
            return null;

        return new DialogData()
        {
            Title = item.Title,
            Description = item.Description,
            Amount = item.Amount
        };
    }

    private Item ProcessDialogData(int itemID, DialogData dialogData)
    {
        return new Item()
        {
            ID = itemID,
            Title = dialogData.Title,
            Description = dialogData.Description,
            Amount = dialogData.Amount,
            CategoryID = _categoryID
        };
    }
}