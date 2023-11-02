﻿using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;

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
        _categoryID = categoryID;

        StatusTextValue = categoryID.ToString();
        IsStatusTextVisible = true;
        BaseUrl = baseUrl;
        PageTitle = "Категория";
        //CategoryService = new CategoryService(baseUrl);
        ReloadItems();
    }

    // Properties
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

        CanReloadItems = true;
    }

    protected override async Task<ApiResponse<object>> RemoveRequest(int itemID, int userID)
    {
        var CategoryService = new CategoryService(BaseUrl);
        var response = await CategoryService.RemoveItem(itemID, User.Id);
        return response;
    }
}