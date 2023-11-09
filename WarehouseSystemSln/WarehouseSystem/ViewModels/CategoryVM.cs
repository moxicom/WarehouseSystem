using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private string _pageTitle;
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
        var CategoryService = new CategoryService(BaseUrl);
        var response = await CategoryService.GetItems(_categoryID, User.Id);
        LoadTitle();
        return response;
    }

    protected async void LoadTitle()
    {
        PageTitle = _titleLoadingText;
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

    protected override async Task<ApiResponse<object>> AdditionRequest(DialogData formData)
    {
        var categoryService = new CategoryService(BaseUrl);
        var item = new Item()
        {
            ID = 0,
            Title = formData.Title,
            Description = formData.Description,
            Amount = formData.Amount,
            CategoryID = _categoryID
        };
        var response = await categoryService.InsertItem(User.Id, item);
        return response;
    }
}