using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WarehouseSystem.Models;
using WarehouseSystem.Utilities;

namespace WarehouseSystem.ViewModels;

public class MainViewModel : BaseViewModel
{
    // fields
    private readonly string _baseUrl;
    private readonly CategoriesVM _categoriesVM;
    private BaseViewModel _currentViewModel;

    // constructor
    public MainViewModel(string baseUrl, User user)
    {
        User = user;
        CurrentViewModel = new HomeVM();
        _baseUrl = baseUrl;
        _categoriesVM = new CategoriesVM(baseUrl, this);
        HomeBtnClick = new RelayCommand(OpenHomeView);
        CategoriesBtnClick = new RelayCommand(OpenCategoriesView);
    }

    // properties
    public ICommand HomeBtnClick { get; }
    public ICommand CategoriesBtnClick { get; }
    public User User { get; }

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if (_currentViewModel != value)
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
    }

    // methods
    public void OpenCategoriesView() => CurrentViewModel = _categoriesVM;
    public void OpenHomeView() => CurrentViewModel = new HomeVM();
    public void OpenCategoryView(int ID) => CurrentViewModel = new CategoryVM(ID, _baseUrl, this);
}