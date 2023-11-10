using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Enums;
using WarehouseSystem.Models;
using WarehouseSystem.Services;

namespace WarehouseSystem.ViewModels;

internal class CategoriesVM : BaseItemListVM<Category>
{
    // Constructor
    public CategoriesVM(string baseUrl, MainViewModel mainViewModel) : base(baseUrl, mainViewModel,
        "Категории отсутствуют", "Загрузка...")
    {
        OpenCategoryCommand = new RelayCommand<int>(OpenCategory);
        //CategoriesService = new CategoriesService(baseUrl);
        ItemDialogType = ItemDialogType.Category;
        BaseUrl = baseUrl;
        ReloadItems();
    }

    // properties
    public ICommand OpenCategoryCommand { get; set; }
    //public CategoriesService CategoriesService { get; set; }

    // Methods
    protected override async Task<ApiResponse<List<Category>>> LoadItemsRequest()
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var response = await categoriesService.GetCategories(User.Id);
        return response;
    }

    protected override async Task<ApiResponse<object>> RemoveRequest(int categoryID)
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var response = await categoriesService.RemoveCategory(User.Id, categoryID);
        return response;
    }

    protected override async Task<ApiResponse<object>> AdditionRequest(DialogData dialogData)
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var category = ProcessDialogData(categoryID: 0, dialogData);
        var response = await categoriesService.InsertCategory(User.Id, category);
        return response;
    }

    protected override async Task<ApiResponse<object>> UpdatingRequest(int categoryID, DialogData dialogData)
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var category = ProcessDialogData(categoryID, dialogData);
        var response = await categoriesService.UpdateCategory(User.Id, category);
        return response;
    }

    protected override DialogData? GetItemData(int itemID)
    {
        if (ItemList == null)
            return null;

        Category? item = ItemList.FirstOrDefault(item => item.ID == itemID);
        if (item == null)
            return null;
        
        return new DialogData()
        {
            Title = item.Title,
            Description = "",
            Amount = 0
        };
    }

    private Category ProcessDialogData(int categoryID, DialogData dialogData)
    {
        return new Category()
        {
            ID = categoryID,
            Title = dialogData.Title,
            CreatorID = User.Id,
            CreatedAt = DateTime.Now,
        };
    }

    public void OpenCategory(int ID)
    {
        MainVM.OpenCategoryView(ID);
    }
}