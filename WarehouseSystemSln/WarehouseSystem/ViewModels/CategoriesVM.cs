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
        CategoriesService = new CategoriesService(baseUrl);
        ItemDialogType = ItemDialogType.Category;
        ReloadItems();
    }

    // properties
    public ICommand OpenCategoryCommand { get; set; }
    public CategoriesService CategoriesService { get; set; }

    // Methods
    protected override async void LoadItems()
    {
        var response = await CategoriesService.GetCategories(User.Id);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Check if there is no category
            if (response.Data != null)
            {
                ItemList = new ObservableCollection<Category>(response.Data);
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

    protected override async Task<ApiResponse<object>> RemoveRequest(int itemID)
    {
        return new ApiResponse<object>();
    }

    public void OpenCategory(int ID)
    {
        MainVM.OpenCategoryView(ID);
    }

    protected override Task<ApiResponse<object>> AdditionRequest(DialogData formData)
    {
        return null;
    }
}