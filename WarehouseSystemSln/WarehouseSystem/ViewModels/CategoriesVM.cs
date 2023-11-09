using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    protected override async Task<ApiResponse<object>> RemoveRequest(int itemID)
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var response = await categoriesService.RemoveCategory(User.Id, itemID);
        return response;
    }

    protected override async Task<ApiResponse<object>> AdditionRequest(DialogData formData)
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var category = new Category()
        {
            ID = 0,
            Title = formData.Title,
            CreatorID = User.Id,
            CreatedAt = DateTime.Now,
        };
        var response = await categoriesService.InsertCategory(User.Id, category);
        return response;
    }

    public void OpenCategory(int ID)
    {
        MainVM.OpenCategoryView(ID);
    }
}