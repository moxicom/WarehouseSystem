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
    private readonly AdminPanelVM _adminPanelVM;
    private BaseViewModel _currentViewModel;

    // constructor
    public MainViewModel(string baseUrl, User user)
    {
        User = user;
        CurrentViewModel = new HomeVM();
        _baseUrl = baseUrl;
        _categoriesVM = new CategoriesVM(baseUrl, this);
        _adminPanelVM = new AdminPanelVM(baseUrl, this);
        OpenHomeCommand = new RelayCommand(OpenHomeView);
        OpenCategoriesCommand = new RelayCommand(OpenCategoriesView);
        OpenAdminPanelCommand = new RelayCommand(OpenAdminPanel);
    }

    // properties
    public ICommand OpenHomeCommand { get; }
    public ICommand OpenCategoriesCommand { get; }
    public ICommand OpenAdminPanelCommand { get; }
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
    public void OpenAdminPanel() => CurrentViewModel = _adminPanelVM;
}