using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
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
    protected override async void LoadItems()
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var response = await categoriesService.GetCategories(User.Id);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Check if there is no category
            if (response.Data != null)
            {
                ItemList = new ObservableCollection<Category>(response.Data);
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

    protected override async Task<ApiResponse<object>> RemoveRequest(int itemID)
    {
        return new ApiResponse<object>();
    }

    public void OpenCategory(int ID)
    {
        MainVM.OpenCategoryView(ID);
    }

    protected override async Task<ApiResponse<object>> AdditionRequest(DialogData formData)
    {
        var categoriesService = new CategoriesService(BaseUrl);
        var category = new Category()
        {
            Title = formData.Title,
            Description = formData.Description,
        };
        var response = await categoriesService.InsertCategory(User.Id, category);
        return response;
    }
}